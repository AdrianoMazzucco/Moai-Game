using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class VolcanoProjectileManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private float BurstCount = 2f;
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private float projectileAirTime;
    [SerializeField] private float innerRadius = 75f;
    [SerializeField] private float outerRadius = 250f;
    [SerializeField] private float verticalJumpHeight = 30f;
    [SerializeField] private GameObject projectileGameObject;
    [SerializeField] private Ease projectileEase;
    #endregion

    #region Unity

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FirePojectile());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        StopCoroutine(FirePojectile());
    }

    #endregion

    #region Coroutines and Invokes

    IEnumerator FirePojectile()
    {
        WaitForSeconds waitTime = new WaitForSeconds(timeBetweenSpawns);
        GameObject projectile;
        Vector3 endPos;
        while (true)
        {
            yield return waitTime;

            for (int i = 0; i < BurstCount; i++)
            {
                 
                Vector2 direction = Util.GetRandomPointBetweenTwoRadii(innerRadius, outerRadius);
                projectile =  GameManager.Instance.BoulderPool.GetObject(this.transform.position);
                endPos = new Vector3(direction.x + transform.position.x,
                    0,
                    direction.y + transform.position.z);
                endPos.y = GameManager.Instance.CurrentTerrain.SampleHeight(endPos);

                projectile.transform.DOJump(endPos, verticalJumpHeight, 1, projectileAirTime).SetEase(projectileEase);
                GameObject g =  GameManager.Instance.DecalPool.GetObject(endPos);
                StartCoroutine(Util.ReleaseAfterDelay(GameManager.Instance.DecalPool,g,projectileAirTime));
            }
           
            

        }
    }
    #endregion
}
