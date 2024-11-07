using UnityEngine;

public class DestructableRockStats : MonoBehaviour
{
    public int currentHealth = 100;
    public int maxHealth = 100;

    // Amount of health reduced per click
    public int damageAmount = 10;

    void Update()
    {
        // Detect if the left mouse button was clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits this object
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    // Reduce health
                    TakeDamage(damageAmount);
                }
            }
        }
    }

    

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject); // Destroy the box if health reaches zero
        }
    }
}