using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class FreeLookCameraController : MonoBehaviour
{
    public InputActionAsset inputActions; // Reference to the Input Action Asset
    public CinemachineFreeLook[] freeLookCameras; // Assign the 4 Cinemachine FreeLook cameras in the Inspector
    public float yAxisScrollSpeed = 0.1f; // Speed at which to adjust the Y Axis

    [Header("Camera Targets")]
    public Transform followTarget; // The target the cameras should follow
    public Transform lookAtTarget; // The target the cameras should look at

    private int currentCameraIndex = 0;
    private InputAction lookAction;

    private void Awake()
    {
        // Locate the Player action map and the Look action within it
        var playerActionMap = inputActions.FindActionMap("Player");
        lookAction = playerActionMap?.FindAction("Look");

        // Enable the Look action
        if (lookAction != null)
        {
            lookAction.Enable();
        }
        else
        {
            Debug.LogError("Look action not found in the Player action map.");
        }

        // Assign Follow and LookAt targets to each FreeLook camera
        foreach (var camera in freeLookCameras)
        {
            if (camera != null)
            {
                camera.Follow = followTarget;
                camera.LookAt = lookAtTarget;
            }
        }

        // Initialize by setting the first camera as active
        SetActiveCamera(currentCameraIndex);
    }

    private void OnEnable()
    {
        if (lookAction != null)
        {
            lookAction.performed += OnLookPerformed;
        }
    }

    private void OnDisable()
    {
        if (lookAction != null)
        {
            lookAction.performed -= OnLookPerformed;
        }
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        // Separate input components
        float rotateInput = input.x;
        float yAxisInput = input.y;

        // Rotate the camera selection based on X-axis input
        if (rotateInput > 0) // Right (Rotate clockwise)
        {
            RotateRight();
        }
        else if (rotateInput < 0) // Left (Rotate counterclockwise)
        {
            RotateLeft();
        }

        // Adjust the Y Axis of the current active FreeLook camera based on Y-axis input
        AdjustYAxis(yAxisInput);
    }

    private void RotateLeft()
    {
        // Store the current camera's Y Axis value
        float yAxisValue = freeLookCameras[currentCameraIndex].m_YAxis.Value;

        // Move to the previous camera in the array, wrapping around if needed
        currentCameraIndex = (currentCameraIndex - 1 + freeLookCameras.Length) % freeLookCameras.Length;

        // Apply the Y Axis value to the new active camera and set it as active
        freeLookCameras[currentCameraIndex].m_YAxis.Value = yAxisValue;
        SetActiveCamera(currentCameraIndex);
    }

    private void RotateRight()
    {
        // Store the current camera's Y Axis value
        float yAxisValue = freeLookCameras[currentCameraIndex].m_YAxis.Value;

        // Move to the next camera in the array, wrapping around if needed
        currentCameraIndex = (currentCameraIndex + 1) % freeLookCameras.Length;

        // Apply the Y Axis value to the new active camera and set it as active
        freeLookCameras[currentCameraIndex].m_YAxis.Value = yAxisValue;
        SetActiveCamera(currentCameraIndex);
    }

    private void AdjustYAxis(float input)
    {
        // Adjust the Y Axis of the current active FreeLook camera based on input
        if (input != 0)
        {
            float newYAxisValue = freeLookCameras[currentCameraIndex].m_YAxis.Value + (input * yAxisScrollSpeed);
            freeLookCameras[currentCameraIndex].m_YAxis.Value = Mathf.Clamp01(newYAxisValue); // Clamp to range 0-1
        }
    }

    private void SetActiveCamera(int index)
    {
        for (int i = 0; i < freeLookCameras.Length; i++)
        {
            freeLookCameras[i].Priority = (i == index) ? 10 : 0; // Only the active camera has a higher priority
        }
    }
}
