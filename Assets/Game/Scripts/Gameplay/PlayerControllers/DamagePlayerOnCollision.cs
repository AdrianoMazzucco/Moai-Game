using UnityEngine;

public class DamagePlayerOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        MineralCountScript mineralScript = collision.gameObject.GetComponent<MineralCountScript>();
        if (mineralScript != null) 
        {
            mineralScript.TakeDamage();
        }
    }
}
