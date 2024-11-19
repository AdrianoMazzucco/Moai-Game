using UnityEngine;

public class CanvasFaceCamera : MonoBehaviour
{
    [SerializeField] private Camera targetCamera; // Assign the target camera in the inspector

    private void Start()
    {
        // If no camera is assigned, use the main camera
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        if (targetCamera != null)
        {
            // Make the canvas face the camera
            transform.LookAt(transform.position + targetCamera.transform.forward);
        }
    }
}

