using System.Collections;
using UnityEngine;

public class TreeDestruction : MonoBehaviour
{
    [Header("Tree Parts")]
    public GameObject staticTree; // Static tree model (child object with collider)
    public GameObject brokenTree; // Broken tree model (child object with Rigidbody)
    public GameObject leafMesh; // Top leaf mesh of the tree
    public Collider treeTopCollider; // SphereCollider for tree top detection (child of BrokenTree)

    [Header("Particle Effects")]
    public ParticleSystem brokenTreeBase; // Particle effect at the base
    public ParticleSystem leafExplosion; // Particle effect for leaves

    [Header("Force Settings")]
    public float destructionForceThreshold = 1f; // Force required to destroy the tree

    [Header("Dissolve Settings")]
    public float dissolveDelay = 5f; // Delay before dissolving the trunk

    private Rigidbody brokenTreeRb;
    private bool isBroken = false;

    void Start()
    {
        // Ensure only the static tree is active at the start
        staticTree.SetActive(true);
        brokenTree.SetActive(false);

        // Automatically fetch the Rigidbody component from the broken tree
        brokenTreeRb = brokenTree.GetComponentInChildren<Rigidbody>();
        if (!brokenTreeRb)
        {
            Debug.LogError("Broken tree needs a Rigidbody component.");
        }

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

    void OnCollisionEnter(Collision collision)
    {

        // Check the impact force
        if (collision.relativeVelocity.magnitude >= destructionForceThreshold)
        {
            //Debug.Log("We should break");
            BreakTree(collision);
        }
    }

    private void BreakTree(Collision collision)
    {
        

        isBroken = true;

        Debug.Log(isBroken);

        // Switch tree models
        staticTree.SetActive(false);
        brokenTree.SetActive(true);

        // Enable physics on the broken tree
        brokenTreeRb.isKinematic = false;
        brokenTreeRb.AddForce(collision.relativeVelocity, ForceMode.Impulse);

        // Play particle effect at the base
        if (brokenTreeBase) brokenTreeBase.Play();

        StartCoroutine(DissolveTrunk());
    }

    private IEnumerator DissolveTrunk()
    {
        yield return new WaitForSeconds(dissolveDelay);

        // Destroy the broken tree after delay
        Destroy(this.gameObject);
    }

    /*
    private void HandleTreeTopImpact()
    {
        if (!isBroken) return;

        // Disable the leaf mesh
        if (leafMesh) leafMesh.SetActive(false);

        // Play leaf explosion effect
        if (leafExplosion) leafExplosion.Play();

        // Start the dissolve timer for the trunk
        StartCoroutine(DissolveTrunk());
    }
       

    private void OnTriggerEnter(Collider other)
    {
        // Trigger impact logic when tree top collider hits the terrain
        if (isBroken && other == treeTopCollider && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            
        {
            Debug.Log();
            HandleTreeTopImpact();
        }
    }
    */
}
