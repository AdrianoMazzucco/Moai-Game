using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class VolcanoHealth : MonoBehaviour
{
    #region Events

    public UnityEvent OnDamageTaken;

    #endregion
    
    
    #region Variables
    [Header("Values")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private float invulDuration = 2f;
    [SerializeField] private float respawnDelay = 10f;
    [Header("Minerals")]
    [SerializeField] private Vector3 mineralSpawnDirection;
    [SerializeField] private float mineralLaunchMagnitude = 10f;
    
    [SerializeField] private int MineralsToSpawn = 10;
    [SerializeField] private int DamageTaken = 10;
   
    [Header("Connections")] 
    [SerializeField] private Collider _collider;

    [SerializeField] private VolcanoProjectileManager projectileManager;
    [SerializeField] private BoxCollider _triggerCollider;

    public bool isSecretVolcano = false;
    #endregion

    #region Properties

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if (value < currentHealth)
            {
                OnDamageTaken.Invoke();
                _damagedMMF.PlayFeedbacks();
                if (GameManager.Instance.playerMovementScript.CurrentState == MovementState.flying)
                {
                    for (int i = 0; i < MineralsToSpawn; i++)
                    {
                        GameManager.Instance.MineralPool.GetObject(this.transform.position + new Vector3(0,10,0)).GetComponent<Rigidbody>()
                            .linearVelocity =
                                new Vector3((mineralSpawnDirection.x + Random.Range(-2, 2)), mineralSpawnDirection.y,
                                    mineralSpawnDirection.z + Random.Range(-2, 2)).normalized * mineralLaunchMagnitude;

                    }
                }
            }
            currentHealth = value;

            if (CurrentHealth <= 0)
            {
                if(isSecretVolcano)
                {
                    isSecretVolcano=false;
                    GameManager.Instance.playerGameObject.GetComponent<ToggleSunglasses>().Toggle(true);
                }

                toBeDisabled.SetActive(false);
                projectileManager.enabled = false;
                StartCoroutine(RespawnLater());
            }
        }
        
    }
    

    #endregion

    #region Connections

    [SerializeField] private GameObject toBeDisabled;

    #endregion
    

    #region Feel

    [Header("Feel")] 
    [SerializeField] private MMF_Player _damagedMMF;
    [SerializeField] private MMF_Player _InvulMMF;
    

    #endregion


    #region Unity

    private void Start()
    {
        InvulCoroutine = Invincibility();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            CurrentHealth -= DamageTaken;
            StartCoroutine(InvulCoroutine);
            
            
        }
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        StopCoroutine(InvulCoroutine);
    }

    #endregion
    #region Functions


    private void SetInvul(bool b)
    {
        if (b)
        {
            _InvulMMF.StopFeedbacks();
        }
        else
        {
            _InvulMMF.PlayFeedbacks();
        }
       
        _triggerCollider.enabled = b;
    }

    #endregion
    
    
    #region Coroutines

    private IEnumerator InvulCoroutine ;

    private IEnumerator Invincibility()
    {
        float time = 0;
        WaitForEndOfFrame framewait = new WaitForEndOfFrame();
        
        SetInvul(false);
        while (time < invulDuration)
        {
            time += Time.deltaTime;
            yield return framewait;
        }
        
        SetInvul(true);
    }

    private IEnumerator RespawnLater()
    {
        yield return new WaitForSeconds(respawnDelay);
        toBeDisabled.SetActive(true);
        projectileManager.enabled = true;
        CurrentHealth = maxHealth;
    }


    #endregion
}
