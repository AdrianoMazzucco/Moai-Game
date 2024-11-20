using UnityEngine;

public class TestForCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision detected with: {collision.collider.name}");

        
    }
}
