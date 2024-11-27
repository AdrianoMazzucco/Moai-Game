using UnityEngine;

public class LogSpeed : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        // Get the Rigidbody component attached to this GameObject
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody attached to " + gameObject.name + ". Speed cannot be logged.");
        }
    }

    private void Update()
    {
        if (rb != null)
        {
            // Log the speed (magnitude of velocity) to the console
            float speed = rb.linearVelocity.magnitude;
            Debug.Log(gameObject.name + " Speed: " + speed);
        }
    }
}

