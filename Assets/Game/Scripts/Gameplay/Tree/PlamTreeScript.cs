using System;
using System.Collections;
using UnityEngine;

public class PlamTreeScript : MonoBehaviour
{
    #region Variables
    [Header("Connections")]
    [SerializeField] private GameObject[] GameObjectsToActivateOnContact;
    [SerializeField] private GameObject[] GameObjectsToDEactivateOnContact;
    
    
    
    [SerializeField] private Rigidbody[] Leaves;
    [SerializeField] private Transform LeafExplosionCenter;
    [SerializeField] private Rigidbody[] Trunks;
    [SerializeField] private Rigidbody[] Coconuts;
    [Header("Values")]
    [SerializeField] private LayerMask LayersToInteractWith;

    [SerializeField] private float SmallBreakThreshold = 5f;
    [SerializeField] private float BigBreakThreshold = 10f;

    [Header("BigBreak")]
    [SerializeField] private float LeavesExplosionForce = 3f;
    [SerializeField] private float CoconutExplosionForce = 5f;
    [SerializeField] private float TrunkForceMultiplier = 2f;

    [Header("BaseSettings")] 
    [SerializeField] private float RespawnTime = 25f;


    private Vector3[] initalLeavesPositions;
    private Vector3[] initalTrunksPositions;
    private Vector3[] initalCoconutsPositions;
    
    private Quaternion[] initalLeavesRotations;
    private Quaternion[] initalTrunksRotations;
    private Quaternion[] initalCoconutsRotations;

    
    #endregion
    
    


    #region UnityFunctions

    private void Awake()
    {
        initalCoconutsPositions = new Vector3[Coconuts.Length];
        initalLeavesPositions = new Vector3[Leaves.Length];
        initalTrunksPositions = new Vector3[Trunks.Length];  
        
        initalCoconutsRotations = new Quaternion[Coconuts.Length];
        initalLeavesRotations = new Quaternion[Leaves.Length];
        initalTrunksRotations = new Quaternion[Trunks.Length];  
    }

    void Start()
    {
     
    }

    private void OnEnable()
    {
         StorePositionsAndRotations();
    }

    void Update()
    {
        
    }


    private Vector3 _collisionForce;

    private void OnTriggerEnter(Collider other)
    {
        if (Util.CompareWithLayerMask(LayersToInteractWith,other.gameObject))
        {
            _collisionForce = other.GetComponent<Rigidbody>().linearVelocity;
            if (_collisionForce.magnitude > BigBreakThreshold)
            {
                BigBreak(_collisionForce);
            }
            // else if(_collisionForce.magnitude > SmallBreakThreshold)
            // {
            //     SmallBreak(_collisionForce);
            // }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
      
    }

    #endregion

    #region Functions

    private void StorePositionsAndRotations()
    {
        for (int i = 0; i < Leaves.Length; i++)
        {
            initalLeavesPositions[i] = Leaves[i].transform.position;
            initalLeavesRotations[i] = Leaves[i].transform.rotation;
        }
        for (int i = 0; i < Trunks.Length; i++)
        {
            initalTrunksPositions[i] = Trunks[i].transform.position;
            initalTrunksRotations[i] = Trunks[i].transform.rotation;
        }
        for (int i = 0; i < Coconuts.Length; i++)
        {
            initalCoconutsPositions[i] = Coconuts[i].transform.position;
            initalCoconutsRotations[i] = Coconuts[i].transform.rotation;
        }
    }

    private void RestorePositions()
    {
        for (int i = 0; i < Leaves.Length; i++)
        {
            Leaves[i].isKinematic = true;
            Leaves[i].position = initalLeavesPositions[i];
            Leaves[i].rotation = initalLeavesRotations[i];
            
        }
        for (int i = 0; i < Trunks.Length; i++)
        {
            Trunks[i].isKinematic = true;
            Trunks[i].position = initalTrunksPositions[i];
            Trunks[i].rotation = initalTrunksRotations[i];
          
        }
        for (int i = 0; i < Coconuts.Length; i++)
        {
            Coconuts[i].isKinematic = true;
            Coconuts[i].position = initalCoconutsPositions[i];
            Coconuts[i].rotation = initalCoconutsRotations[i];
          
        }
    }
    public void RespawnTree()
    {
        RestorePositions();
        
    }
    public void SwapModelsToRigidbodies(bool b = true)
    {
        foreach (var go in GameObjectsToDEactivateOnContact)
        {
            go.SetActive(!b);
        }

        foreach (var go in GameObjectsToActivateOnContact)
        {
            go.SetActive(b);
        }
    }
    
    public void BigBreak(Vector3 collisionForce)
    {
        SwapModelsToRigidbodies();
        
        foreach (var leaf in Leaves)
        {
            leaf.isKinematic = false;
            //leaf.excludeLayers = LayersToInteractWith | 1<< 3;
            leaf.AddExplosionForce(LeavesExplosionForce,LeafExplosionCenter.position,5f,1f ,ForceMode.Impulse);
        }

        foreach (var trunk in Trunks)
        {
            trunk.isKinematic = false;
           // trunk.excludeLayers = LayersToInteractWith | 1<< 3;
            trunk.AddForce(collisionForce *TrunkForceMultiplier,ForceMode.Impulse);
        }

        foreach (var coconut in Coconuts)
        {
            coconut.isKinematic = false;
            //coconut.excludeLayers = LayersToInteractWith | 1<< 3;
            coconut.AddExplosionForce(LeavesExplosionForce,LeafExplosionCenter.position,5f,1f ,ForceMode.Impulse);
        }
        
        StartCoroutine(RespawnDelay());
       // Invoke(nameof(ReturnPlayerCollision),0.1f);
    }

    public void SmallBreak(Vector3 collisionForce)
    {
        SwapModelsToRigidbodies();
        StartCoroutine(RespawnDelay());
       // Invoke(nameof(ReturnPlayerCollision),0.1f);
    }


    private void ReturnPlayerCollision()
    {
        foreach (var coconut in Coconuts)
        {
            coconut.excludeLayers = LayersToInteractWith & ~(1 << 3);
        }

        foreach (var trunk in Trunks)
        {
           trunk.excludeLayers =LayersToInteractWith & ~(1 << 3);
        }

        foreach (var leaf in Leaves)
        {
          leaf.excludeLayers = LayersToInteractWith & ~(1 << 3);
        }
    }
    #endregion

    #region Coroutines

    private IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(RespawnTime);
       
        RespawnTree();
        yield return new WaitForSeconds(0.1f);
        SwapModelsToRigidbodies(false);
    }

    #endregion
}
