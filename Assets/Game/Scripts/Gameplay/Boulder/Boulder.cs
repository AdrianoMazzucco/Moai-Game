using System;
using DG.Tweening;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Boulder : MonoBehaviour,IDestructable
{
    #region Variables
    [Header("Health")]
    [Space]
    [SerializeField] private int maxHp = 100;
    [SerializeField] private int hpThreshold = 10;
    private int _currentHealth;

    [Header("Other values")] [Space] 
    [SerializeField] private float totalLifeTime = 15f;
    [SerializeField] private float collideWithPlayerDamageMultipler = 0.1f;
    [Header("ShockWave")] 
    [SerializeField] private float shockwaveRadius = 4f;
    [Header("Connections")] [Space] 
    [SerializeField] private GameObject shatterModel;
    [SerializeField] private GameObject projectileModel;
    [SerializeField] private GameObject VFX;
    [Space]
    private Rigidbody _rigidbody;
    
    [SerializeField] private GameObject _particleSystem;
    [SerializeField] private BoxCollider _triggerCollider;
    [SerializeField] private BoxCollider _collider;
    private Enums.Attacktype lastAttacktype = Enums.Attacktype.Bonk;
    private BoulderPieceManager _pieceManager;

    private Coroutine selfDestroyCoroutine;
    
    #endregion

    #region Properties
    
    public int CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            int previousHealth = _currentHealth;
            
            _currentHealth = value;
            if (_currentHealth <= 0)
            {
                Shatter();
            }
            
        }
    }

 
    #endregion

    #region Events

    private UnityEvent OnLanded = new UnityEvent();

    #endregion

    #region Feel

    [SerializeField] private MMF_Player DamageMMF;

    #endregion
    
    
    #region Unity

    private void OnEnable()
    {  
        _triggerCollider.enabled = true;
        OnLanded.AddListener(SwapModels);
        OnLanded.AddListener(CollideWithTerrain);
        OnLanded.AddListener(ExplosionForceAroundLand);
        SwapModels();
         selfDestroyCoroutine = StartCoroutine(Util.ReleaseAfterDelay(GameManager.Instance.BoulderPool, this.gameObject, totalLifeTime));
         _currentHealth = maxHp;
         _collider.enabled = true;
    }

    private void OnDisable()
    {
        OnLanded.RemoveListener(SwapModels);
        OnLanded.RemoveListener(CollideWithTerrain);
        
        OnLanded.RemoveListener(ExplosionForceAroundLand);
        StopCoroutine(selfDestroyCoroutine);
        _particleSystem.SetActive(false);
    }

    private void Awake()
    {
       
    }

    private void Start()
    {
        _pieceManager = this.GetComponent<BoulderPieceManager>();
        _rigidbody = this.GetComponent<Rigidbody>();
        
       
        
       
    }

    private void Update()
    {
        
    }

    #endregion

    #region Methods

    private void ExplosionForceAroundLand()
    {
        Vector3 center = transform.position;

        Collider[] colliders = Physics.OverlapSphere(center, shockwaveRadius);
        foreach (var collider in colliders)
        {
            if(collider.TryGetComponent(out IDestructable destructable) && collider != this._collider)
            {
                destructable.Destruct(this.transform.position);
            }
            // if (collider.attachedRigidbody && !col.rigidbody.isKinematic)
            // {
            //     // if it's a non kinematic rigidbody...
            //     // var dir = col.transform.position - center; // dir = player->object
            //     // col.rigidbody.velocity = dir.normalized * dragSpeed; // kick it out
            // }
        }
    }
    public void Destruct(Vector3 pos)
    {
        Shatter();
    }
    
    //What happens when HP hits 0
    private void Shatter()
    {
        _collider.enabled = false;
        _pieceManager.Break(lastAttacktype);
    }

    private void CollideWithTerrain()
    {
        _triggerCollider.enabled = false;
        _particleSystem.SetActive(true);
    }

    private void SwapModels()
    {

        if (projectileModel.activeSelf == false)
        {
            shatterModel.SetActive(false);
            projectileModel.SetActive(true);
            VFX.SetActive(true);
        }
        else
        {
            shatterModel.SetActive(true);
            projectileModel.SetActive(false);
            VFX.SetActive(false);
        }
      
        
    }

    public void FireProjectile(Vector3 endPos,float verticalJumpHeight, float projectileAirTime,Ease projectileEase)
    {
        transform.DOJump(endPos, verticalJumpHeight, 1, projectileAirTime).SetEase(projectileEase).OnComplete( OnLanded.Invoke);
    }
        
    #endregion
    
    
    //TODO: REMOVE THIS

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.layer == 3)
        {
            DamageMMF?.PlayFeedbacks();
            CurrentHealth -= (int)(collision.impulse.magnitude *
                             collideWithPlayerDamageMultipler);
         
        }
        
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.layer == 6)
    //     {
    //        
    //     }
    // }
    
}
