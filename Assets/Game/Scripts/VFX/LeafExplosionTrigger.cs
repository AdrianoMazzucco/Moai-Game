
using UnityEngine;

public class LeafExplosionTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Collider treeTopCollider;
    public GameObject leafMesh;
    public GameObject leafExplosion;

    void Start()
    {
        treeTopCollider = this.GetComponent<Collider>();
        
        // Ensure the tree top collider is assigned
        if (!treeTopCollider)
        {
            Debug.LogError("TreeTopCollider is not assigned in the inspector.");
        }
        else if (!treeTopCollider.isTrigger)
        {
            Debug.LogError("TreeTopCollider must be set as a trigger.");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // Trigger impact logic when tree top collider hits the terrain
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))

        {
            Debug.Log("Tree Collider hit ground");
            HandleTreeTopImpact();
        }
    }

    private void HandleTreeTopImpact()
    {

        // Disable the leaf mesh
        if (leafMesh) leafMesh.SetActive(false);

        
            leafExplosion.SetActive(true);
            foreach (ParticleSystem ps in leafExplosion.GetComponentsInChildren<ParticleSystem>())
            {
                ps.Play(); // Ensure all particles are manually triggered
            }
        
    }
}
