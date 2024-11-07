using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CapsuleCollider))]
public class MineralSpawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnPrefabs; // List of prefabs to spawn
    [SerializeField] private int spawnCount = 5; // Number of objects to spawn
    [SerializeField] private float forceAmount = 10f; // Force to apply to each spawned object
    [SerializeField] private Color gizmoColor = Color.green; // Color of the gizmo

    private CapsuleCollider capsuleCollider;
    private HashSet<float> usedAngles = new HashSet<float>(); // Track used angles to avoid duplicates

    private void Awake()
    {
        // Get the CapsuleCollider component on the object
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        SpawnMinerals();
    }

    private void SpawnMinerals()
    {
        // Get the radius from the CapsuleCollider
        float radius = capsuleCollider.radius;

        // Divide 360 degrees into equal segments based on spawnCount
        float segmentAngle = 360f / spawnCount;

        // Iterate for each prefab to spawn
        for (int i = 0; i < spawnCount; i++)
        {
            // Choose a random prefab from the list
            GameObject prefabToSpawn = spawnPrefabs[Random.Range(0, spawnPrefabs.Count)];

            // Call DefaultSpawn to handle the actual spawning and force application within the segment range
            DefaultSpawn(prefabToSpawn, segmentAngle * i, segmentAngle);
        }
    }

    private void DefaultSpawn(GameObject prefabToSpawn, float segmentStartAngle, float segmentAngle)
    {
        // Get the radius from the CapsuleCollider
        float radius = capsuleCollider.radius;

        // Randomly choose a position within the segment range
        float randomAngle = Random.Range(segmentStartAngle, segmentStartAngle + segmentAngle);
        float angleInRadians = randomAngle * Mathf.Deg2Rad;

        // Calculate the spawn position along the edge of the circle
        Vector3 spawnOffset = new Vector3(Mathf.Cos(angleInRadians), 0, Mathf.Sin(angleInRadians)) * radius;
        Vector3 spawnPosition = transform.position + spawnOffset;

        // Calculate rotation to align Z-axis of prefab with the vector pointing outward from the center
        Quaternion rotation = Quaternion.LookRotation(spawnOffset);

        // Instantiate the prefab at the calculated position and rotation
        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, rotation);

        // Apply force to the spawned object and log the force direction
        Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(spawnOffset.normalized * forceAmount, ForceMode.Impulse);
            Debug.Log($"Spawned Object: {spawnedObject.name}, Force Direction: {spawnOffset.normalized}, Force Magnitude: {forceAmount}");
        }
    }

    private void OnDrawGizmos()
    {
        // Ensure the gizmo color is set only if the CapsuleCollider component is attached
        if (TryGetComponent(out CapsuleCollider capsuleCollider))
        {
            Gizmos.color = gizmoColor;

            // Get the radius from the CapsuleCollider
            float radius = capsuleCollider.radius;
            Vector3 centerPosition = transform.position;

            // Calculate the angle for each segment
            float segmentAngle = 360f / spawnCount;

            // Draw the segments as lines from the center to the edge of the circle
            for (int i = 0; i < spawnCount; i++)
            {
                float startAngle = segmentAngle * i;
                float endAngle = startAngle + segmentAngle;

                // Convert angles to radians
                float startAngleRad = startAngle * Mathf.Deg2Rad;
                float endAngleRad = endAngle * Mathf.Deg2Rad;

                // Calculate the positions for the start and end of the segment
                Vector3 startPoint = new Vector3(Mathf.Cos(startAngleRad), 0, Mathf.Sin(startAngleRad)) * radius + centerPosition;
                Vector3 endPoint = new Vector3(Mathf.Cos(endAngleRad), 0, Mathf.Sin(endAngleRad)) * radius + centerPosition;

                // Draw the segment line
                Gizmos.DrawLine(centerPosition, startPoint); // Line from center to segment start
                Gizmos.DrawLine(centerPosition, endPoint);   // Line from center to segment end

                // Optionally, draw a line between the segment's start and end points to indicate the full segment
                Gizmos.DrawLine(startPoint, endPoint);
            }
        }
    }
}
