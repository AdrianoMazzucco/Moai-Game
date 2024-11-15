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
    [SerializeField] private float ExplosiveForce = 4f;
    [SerializeField] private float ExplosiveRadius = 0.5f;
    [SerializeField] private float PieceDisappearDelay = 1.5f;
    #endregion

    #region Events

    #endregion

    #region UnityEvents

    private void OnEnable()
    {
        _boulder = this.GetComponent<Boulder>();
    }

    #endregion
    
    

    #region Methods

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
                switch (attacktype)
                {
                    case Enums.Attacktype.Lunge:
                        rigidbody.linearVelocity = rigidbody.linearVelocity ;
                        rigidbody.AddExplosionForce(ExplosiveForce,GameManager.Instance.playerGameObject.transform.position,ExplosiveRadius);
                        break;
                    case Enums.Attacktype.Bonk:
                        rigidbody.linearVelocity = rigidbody.linearVelocity ;
                        rigidbody.AddExplosionForce(ExplosiveForce,this.transform.position,ExplosiveRadius);
                        break;
                }
              
            }
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
