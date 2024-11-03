using UnityEngine;
using Cinemachine;

public class FreeLookRigSwitcher : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;

    private int currentRig = 1; // Start at Middle rig (0 = Top, 1 = Middle, 2 = Bottom)

    void Update()
    {
        // Check for mouse scroll input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput > 0f)
        {
            MoveUp();
        }
        else if (scrollInput < 0f)
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        // Increase rig index but keep it within bounds (0 = Top)
        if (currentRig > 0)
        {
            currentRig--;
            UpdateRigPosition();
        }
    }

    void MoveDown()
    {
        // Decrease rig index but keep it within bounds (2 = Bottom)
        if (currentRig < 2)
        {
            currentRig++;
            UpdateRigPosition();
        }
    }

    void UpdateRigPosition()
    {
        switch (currentRig)
        {
            case 0:
                freeLookCamera.m_YAxis.Value = 1f; // Top rig
                break;
            case 1:
                freeLookCamera.m_YAxis.Value = 0.5f; // Middle rig
                break;
            case 2:
                freeLookCamera.m_YAxis.Value = 0f; // Bottom rig
                break;
        }
    }
}

