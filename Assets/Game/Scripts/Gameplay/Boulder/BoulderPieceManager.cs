using System;
using System.Collections;
using UnityEngine;

public class BoulderPieceManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject[] Pieces;
    [SerializeField] private GameObject UnFracturedGameobject;
    

    private Boulder _boulder;

    [Header("Destruction")] 
    [SerializeField] private int NumberOfMinerals = 1;
    [SerializeField] private float ExplosiveForce = 4f;
    [SerializeField] private float ExplosiveRadius = 0.5f;
    [SerializeField] private float PieceDisappearDelay = 1.5f;


    private Vector3[] initalLocations;
    private Quaternion[] initalRotations;
    
    
    #endregion

    #region Events

    #endregion

    #region UnityEvents

    private void Awake()
    {
        initalLocations = new Vector3[Pieces.Length];
        initalRotations = new Quaternion[Pieces.Length];
        StoreInitalTransform();
    }

    private void OnEnable()
    {
       
        RestoreTransforms( GetComponentsInChildren<Rigidbody>());
     
    }

    #endregion

    private void Start()
    {
        _boulder = this.GetComponent<Boulder>();
     
    }

    #region Methods


    public void StoreInitalTransform()
    {
        
        for (int i = 0; i < Pieces.Length; i++)
        {
            initalLocations[i] = Pieces[i].transform.localPosition;
            initalRotations[i] = Pieces[i].transform.localRotation;
        }
    }

    public void RestoreTransforms(Rigidbody[] rb)
    {
        foreach (var piece in Pieces)
        {
            piece.SetActive(true);
        }
        for (int i = 0; i < rb.Length; i++)
        {
            rb[i].isKinematic = true;
            rb[i].position = initalLocations[i] + transform.position;
            rb[i].rotation = initalRotations[i];
            
        }
        UnFracturedGameobject.SetActive(true);
    }
    public void Break(Enums.Attacktype attacktype)
    {
        UnFracturedGameobject.SetActive(false);
        GameManager.Instance.DestructionFXPool.GetObject(this.transform.position);
        foreach (var piece in Pieces)
        {
            piece.SetActive(true);
        }

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (var rigidbody in rigidbodies)
        {
            if (rigidbody != null)
            {
                rigidbody.isKinematic = false;
                switch (attacktype)
                {
                    case Enums.Attacktype.Lunge:
                        //rigidbody.linearVelocity = rigidbody.linearVelocity ;
                        rigidbody.AddExplosionForce(ExplosiveForce,GameManager.Instance.playerGameObject.transform.position,ExplosiveRadius);
                        break;
                    case Enums.Attacktype.Bonk:
                        //rigidbody.linearVelocity = rigidbody.linearVelocity ;
                        rigidbody.AddExplosionForce(ExplosiveForce,this.transform.position,ExplosiveRadius);
                        break;
                }
              
            }
        }
        
        for (int i = 0; i < NumberOfMinerals; i++)
        {
            GameManager.Instance.MineralPool.GetObject(this.transform.position).GetComponent<Rigidbody>().AddExplosionForce(ExplosiveForce * 5f,this.transform.position,ExplosiveRadius);
            
        }

        StartCoroutine(FadeOutRigidbodies(Pieces));

    }

    #endregion

    #region Coroutines

    private IEnumerator FadeOutRigidbodies(GameObject[] pieces)
    {
       

        yield return new WaitForSeconds(PieceDisappearDelay);

        foreach (var piece in pieces)
        {
            piece.SetActive(false);
            yield return null;
        }
        GameManager.Instance.BoulderPool.Release(_boulder.gameObject);
    }

    #endregion
}
