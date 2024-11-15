using UnityEngine;

public class TriggerForceApplier : MonoBehaviour
{
    public float forceAmount = 10f; // The amount of force to apply

    void OnTriggerStay(Collider other)
    {
        // Get the Rigidbody of the other object (the one that entered the trigger)
        Rigidbody otherRb = other.GetComponent<Rigidbody>();

        if (otherRb != null) // Make sure the other object has a Rigidbody
        {
            // Calculate direction from this object towards the other object
            Vector3 direction = (transform.position - other.transform.position).normalized;

            // Apply force to the other object in that direction
            otherRb.AddForce(direction * forceAmount, ForceMode.Force);
            Debug.Log("Force applied to the other object!");
        }
    }
}
