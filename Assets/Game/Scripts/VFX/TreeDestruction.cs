using System.Collections;
using UnityEngine;

public class TreeDestruction : MonoBehaviour
{
    [Header("Tree Parts")]
    public GameObject staticTree; // Static tree model (child object with collider)
    public GameObject brokenTree; // Broken tree model (child object with Rigidbody)
    public GameObject brokenTreeExplode;
    public GameObject leafMesh; // Top leaf mesh of the tree
    public Collider treeTopCollider; // SphereCollider for tree top detection (child of BrokenTree)

    [Header("Particle Effects")]
    public GameObject leafExplosion; // Particle effect for leaves

    [Header("Force Settings")]
    public float destructionForceThreshold = 1f;
    public float explodeForceThreshold = 2f;

    [Header("Dissolve Settings")]
    public float dissolveDelay = 5f; // Delay before dissolving the trunk

    private Rigidbody brokenTreeRb;

    public Rigidbody explodeTreeRb1;
    public Rigidbody explodeTreeRb2;
    public Rigidbody explodeTreeRb3;
    public Rigidbody explodeTreeRb4;


    private bool isBroken = false;

    void Start()
    {
        // Ensure only the static tree is active at the start
        staticTree.SetActive(true);
        brokenTree.SetActive(false);
        brokenTreeExplode.SetActive(false);

        // Automatically fetch the Rigidbody component from the broken tree
        brokenTreeRb = brokenTree.GetComponentInChildren<Rigidbody>();
        if (!brokenTreeRb){Debug.LogError("Broken tree needs a Rigidbody component.");}
        // Ensure the tree top collider is assigned
        if (!treeTopCollider){Debug.LogError("TreeTopCollider is not assigned in the inspector.");}
        else if (!treeTopCollider.isTrigger){Debug.LogError("TreeTopCollider must be set as a trigger.");}

        // Ensure the leaf explosion parent is assigned
        if (!leafExplosion){Debug.LogError("LeafExplosionParent is not assigned in the inspector.");}
        else{leafExplosion.SetActive(false); }
    }

    void OnCollisionEnter(Collision collision)
    {

        Debug.Log(collision.relativeVelocity.magnitude);

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
        
        /*
        // Switch tree models
        staticTree.SetActive(false);
        brokenTree.SetActive(true);

        // Enable physics on the broken tree
        brokenTreeRb.isKinematic = false;
        brokenTreeRb.AddForce(collision.relativeVelocity, ForceMode.Impulse);
        */

        
        staticTree.SetActive(false);

        
        if (collision.relativeVelocity.magnitude >= explodeForceThreshold)
        {
            //brokenTree.SetActive(false);
            brokenTreeExplode.SetActive(true);

            explodeTreeRb1.AddForce(collision.relativeVelocity, ForceMode.Impulse);
            explodeTreeRb2.AddForce(collision.relativeVelocity, ForceMode.Impulse);
            explodeTreeRb3.AddForce(collision.relativeVelocity, ForceMode.Impulse);
            explodeTreeRb4.AddForce(collision.relativeVelocity, ForceMode.Impulse);
        } else
        {
            brokenTree.SetActive(true);
            //brokenTreeExplode.SetActive(false);

            brokenTreeRb.isKinematic = false;
            brokenTreeRb.AddForce(collision.relativeVelocity, ForceMode.Impulse);
        }



        // Start the dissolve timer for the trunk
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

        /// Enable the parent object containing leaf explosion particles
        if (leafExplosion)
        {
            leafExplosion.SetActive(true);
            foreach (ParticleSystem ps in leafExplosion.GetComponentsInChildren<ParticleSystem>())
            {
                ps.Play(); // Ensure all particles are manually triggered
            }
        }

        // Start the dissolve timer for the trunk
        //StartCoroutine(DissolveTrunk());
    }
       

    private void OnTriggerEnter(Collider other)
    {        
        // Trigger impact logic when tree top collider hits the terrain
        if (isBroken && other == treeTopCollider && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            
        {
            Debug.Log("Tree Collider hit ground");
            HandleTreeTopImpact();
        }
    }
    */
}
