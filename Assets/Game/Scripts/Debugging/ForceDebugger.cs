using UnityEngine;

public class ForceDebugger : MonoBehaviour
{
    private Rigidbody rb;

    // A record to store forces and their sources
    private struct ForceRecord
    {
        public Vector3 force;
        public GameObject source;

        public ForceRecord(Vector3 force, GameObject source)
        {
            this.force = force;
            this.source = source;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("ForceDebugger requires a Rigidbody component.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Vector3 relativeVelocity = collision.relativeVelocity;
            float mass = collision.rigidbody != null ? collision.rigidbody.mass : 1f;
            Vector3 force = relativeVelocity * mass;

            // Log the force and the source GameObject
            LogForce(new ForceRecord(force, collision.gameObject));
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Vector3 relativeVelocity = collision.relativeVelocity;
            float mass = collision.rigidbody != null ? collision.rigidbody.mass : 1f;
            Vector3 force = relativeVelocity * mass;

            // Log the force and the source GameObject
            LogForce(new ForceRecord(force, collision.gameObject));
        }
    }

    private void LogForce(ForceRecord record)
    {
        Debug.Log($"Force Applied: {record.force} | Source: {record.source.name}");
    }
}
