using System.Collections;
using UnityEngine;

public class BoulderPieceManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject[] Pieces;
    [SerializeField] private GameObject UnFracturedGameobject;

    [Header("Destruction")] [SerializeField]
    private float ExplosiveForce = 4f;
    private float ExplosiveRadius = 0.5f;
    private float PieceDisappearDelay = 1.5f;
    #endregion

    #region Events

    #endregion


    #region Methods

    public void Break(Enums.Attacktype attacktype)
    {
        UnFracturedGameobject.SetActive(false);
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
        
    }

    #endregion
}
