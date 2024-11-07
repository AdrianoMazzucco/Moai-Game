using UnityEngine;

public class PushOnEnable : MonoBehaviour
{
    public Vector3 pushDirection = Vector3.forward; // Direction to push the object
    public float forceAmount = 10f; // Amount of force to apply

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (rb != null)
        {
            rb.AddForce(pushDirection.normalized * forceAmount, ForceMode.Impulse);
        }
    }
}
