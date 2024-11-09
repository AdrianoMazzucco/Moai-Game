using UnityEngine;


public class MineralStats : MonoBehaviour
{

    private Animator animator;
    private Vector3 initialVelocity; // The initial velocity applied to the object
    private Rigidbody rb; // Reference to the Rigidbody component
    private bool isDecelerating = false; // Whether the object is decelerating to become stationary
    private bool activePhysics = true;
    [SerializeField] private float decelerationSpeed = 10f; // Speed at which the object decelerates (modifiable in the inspector)
    private bool velocityRegistered = false; // Flag to check if the initial velocity has been registered
    private Vector3 currentRotation;

    void Start()
    {
        // Initialize the Rigidbody based on the initial state
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; // Ensure Rigidbody is not kinematic at the start
        initialVelocity = Vector3.zero; // Initialize initial velocity to zero

        animator = GetComponent<Animator>();
        
        
    }

    void Update()
    {
        if (activePhysics == true)
        {
            // Register the initial velocity when it is first applied (non-zero velocity)
            if (!velocityRegistered && rb.linearVelocity.magnitude > 0)
            {
                initialVelocity = rb.linearVelocity; // Register the first non-zero velocity as the initial velocity
                velocityRegistered = true; // Mark that the velocity has been registered
            }

            // Debug log the current velocity (force)
            Debug.Log("Current Velocity: " + rb.linearVelocity);

            // Check if deceleration should start (velocity has dropped below half of initial velocity)
            if (velocityRegistered && !isDecelerating && rb.linearVelocity.magnitude <= initialVelocity.magnitude / 2f)
            {
                isDecelerating = true;
            }

            // Apply deceleration force in the opposite direction of the velocity
            if (isDecelerating)
            {
                // Apply a force in the opposite direction of the velocity to slow down
                Vector3 decelerationForce = -rb.linearVelocity.normalized * decelerationSpeed;
                rb.AddForce(decelerationForce, ForceMode.Acceleration); // Apply the deceleration force

                // Stop decelerating once the velocity is close to zero
                if (rb.linearVelocity.magnitude < 0.1f) // Small threshold to stop decelerating
                {
                    
                    rb.linearVelocity = Vector3.zero; // Ensure velocity is exactly zero
                    isDecelerating = false; // Stop the deceleration process
                    rb.isKinematic = true; // Set Rigidbody to kinematic to freeze it
                    velocityRegistered = false;
                    animator.SetBool("isKinematic", true);
                    animator.SetBool("activePhysics", false);

                    currentRotation = transform.rotation.eulerAngles;
                    
                   
                }
            }
        }
        else
        {
            // Check if the object's up direction is aligned with the global Y-axis
            if (Mathf.Abs(currentRotation.x) > 0.01f || Mathf.Abs(currentRotation.z) > 0.01f)
            {
                // Set the target rotation with X and Z set to zero, Y unchanged
                Quaternion targetRotation = Quaternion.Euler(0, currentRotation.y, 0);

                // Smoothly blend from the current rotation to the target rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2f * Time.deltaTime);
            }
        }
    }

    // Call this method to apply a new velocity at any time
    public void ApplyVelocity(Vector3 velocity)
    {
        rb.isKinematic = false; // Ensure Rigidbody is not kinematic when applying a new velocity
        initialVelocity = velocity; // Update the initial velocity
        rb.linearVelocity = velocity; // Set the Rigidbody's velocity directly
        isDecelerating = false; // Reset decelerating state in case velocity is reapplied
        velocityRegistered = true; // Mark that the velocity has been registered
    }

    // Method to be called by another script to fetch the current velocity magnitude
    public float GetCurrentVelocityMagnitude()
    {
        return rb.linearVelocity.magnitude; // Get the magnitude of the velocity as the current applied velocity
    }

    // Allow the deceleration speed to be modified in the inspector
    public float DecelerationSpeed
    {
        get { return decelerationSpeed; }
        set { decelerationSpeed = value; }
    }
}
