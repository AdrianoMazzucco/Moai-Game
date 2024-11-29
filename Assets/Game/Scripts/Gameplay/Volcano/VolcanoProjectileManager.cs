using System;
using System.Collections;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
public class VolcanoProjectileManager : MonoBehaviour
{
    #region Variables


    [Header("Checks")] 
    public bool bCheckForPlayer;
    
    
    [SerializeField] private float BurstCount = 2f;
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private float projectileAirTime;
    [SerializeField] private float innerRadius = 75f;
    [SerializeField] private float outerRadius = 250f;
    [SerializeField] private float verticalJumpHeight = 30f;
    [SerializeField] private GameObject projectileGameObject;
    [SerializeField] private Ease projectileEase;
    [SerializeField] private Vector3 spawnPos;


    private Boulder _boulder;
    private float DistanceToPlayer;
    private IEnumerator fireCoroutine ;
    private bool isFiring;
    #endregion

    #region Feel

    [SerializeField] private MMF_Player fireMMF;

    #endregion
    
    #region Unity

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        fireCoroutine = FirePojectile();
    }

    void Start()
    {
       
        _boulder = projectileGameObject.GetComponent<Boulder>();
    }

    private void OnEnable()
    {
        
        if (bCheckForPlayer)
        {
            InvokeRepeating(nameof(CheckDistanceToPlayer),1f,1f);
        }
        else
        {
             isFiring = true;
             StartCoroutine(fireCoroutine);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        isFiring = false;
        StopCoroutine(fireCoroutine);
        CancelInvoke(nameof(CheckDistanceToPlayer));
    }

    #endregion

    #region Functions

    public void FireVolcano(int _burstCount,Vector3 center,float inner,float outer,float _duration)
    {
        WaitForSeconds waitTime = new WaitForSeconds(timeBetweenSpawns);
        GameObject projectile;
        Vector3 endPos;
        for (int i = 0; i < _burstCount; i++)
        {
            fireMMF?.PlayFeedbacks();
            Vector2 direction = Util.GetRandomPointBetweenTwoRadii(inner, outer);
            projectile =  GameManager.Instance.BoulderPool.GetObject(this.transform.position + spawnPos);
            endPos = new Vector3(direction.x + center.x,
                0,
                direction.y + center.z);
            endPos.y = GameManager.Instance.CurrentTerrain.SampleHeight(endPos);

            projectile.transform.DOJump(endPos, verticalJumpHeight, 1, _duration).SetEase(projectileEase);
            GameObject g =  GameManager.Instance.DecalPool.GetObject(endPos);
            StartCoroutine(Util.ReleaseAfterDelay(GameManager.Instance.DecalPool,g,projectileAirTime));
        }
    }

    public void CheckDistanceToPlayer()
    {
        DistanceToPlayer = Vector3.Distance(GameManager.Instance.playerGameObject.transform.position,
            this.gameObject.transform.position);

        if (DistanceToPlayer < outerRadius && !isFiring)
        {
            isFiring = true;
            StartCoroutine(fireCoroutine);
        }
        else if(DistanceToPlayer > outerRadius)
        {
            isFiring = false;
            StopCoroutine(fireCoroutine);
        }
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
                fireMMF?.PlayFeedbacks();
                Vector2 direction = Util.GetRandomPointBetweenTwoRadii(innerRadius, outerRadius);
                projectile =  GameManager.Instance.BoulderPool.GetObject(this.transform.position + spawnPos);


                if (bCheckForPlayer)
                {
                    endPos = new Vector3(GameManager.Instance.playerGameObject.transform.position.x,
                        0,
                        GameManager.Instance.playerGameObject.transform.position.z);
                    endPos.y = GameManager.Instance.CurrentTerrain.SampleHeight(endPos);
                }
                else
                {
                    endPos = new Vector3(direction.x + transform.position.x,
                        0,
                        direction.y + transform.position.z);
                    endPos.y = GameManager.Instance.CurrentTerrain.SampleHeight(endPos);
                }

                projectile.GetComponent<Boulder>().FireProjectile(endPos, verticalJumpHeight, projectileAirTime,projectileEase);
               
                GameObject g =  GameManager.Instance.DecalPool.GetObject(endPos);
                StartCoroutine(Util.ReleaseAfterDelay(GameManager.Instance.DecalPool,g,projectileAirTime));
            }
           
            

        }
    }



    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, innerRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, outerRadius);
    }



    #endregion
}
