using UnityEditor.PackageManager;
using UnityEngine;

public class TreeRandomization : MonoBehaviour
{
    void OnEnable()
    {
        // Randomize Y rotation
        float randomYRotation = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, randomYRotation, transform.rotation.eulerAngles.z);

        // Adjust position so root is 1 unit below the terrain
        AdjustPositionToTerrain();

        // Randomize uniform scale
        float randomScale = Random.Range(0.75f, 1f);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }

    private void AdjustPositionToTerrain()
    {
        // Raycast down from the GameObject's position to find the terrain
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity))
        {
            // Check if the hit object is the terrain
            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                // Set position to 1 unit below the hit point
                Vector3 newPosition = transform.position;
                newPosition.y = hitInfo.point.y - 1f;
                transform.position = newPosition;
            }
        }
    }
}

