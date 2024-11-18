using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Boulder : MonoBehaviour
{
    #region Variables
    [Header("Health")]
    [Space]
    [SerializeField] private int maxHp = 100;
    [SerializeField] private int hpThreshold = 10;
    private int _currentHealth;
    
    [Header("Connections")] [Space] 
    [SerializeField] private GameObject shatterModel;
    [SerializeField] private GameObject projectileModel;
    [SerializeField] private GameObject VFX;
    [Space]
    private Collider _collider;
    private Rigidbody _rigidbody;
    
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private BoxCollider _triggerCollider;
    private Enums.Attacktype lastAttacktype = Enums.Attacktype.Bonk;
    private BoulderPieceManager _pieceManager;
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
            if (value > hpThreshold)
            {
                _currentHealth -= value;
                if (_currentHealth <= 0)
                {
                    Shatter();
                }
            }
        }
    }

 
    #endregion

    #region Events

    private UnityEvent OnLanded = new UnityEvent();

    #endregion
    
    
    
    #region Unity

    private void OnEnable()
    {
        _triggerCollider.enabled = true;
        OnLanded.AddListener(SwapModels);
        SwapModels();
    }

    private void OnDisable()
    {
        OnLanded.RemoveListener(SwapModels);
    }

    private void Start()
    {
        _pieceManager = this.GetComponent<BoulderPieceManager>();
        _collider = this.GetComponent<Collider>();
        _rigidbody = this.GetComponent<Rigidbody>();
        
       
        
        _currentHealth = maxHp;
    }

    private void Update()
    {
        
    }

    #endregion

    #region Methods
    
    
    //What happens when HP hits 0
    private void Shatter()
    {
        _pieceManager.Break(lastAttacktype);
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
        Debug.Log("YES");
        if (collision.gameObject == GameManager.Instance.playerGameObject)
        {
            Debug.Log("YES");
            Shatter();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _triggerCollider.enabled = false;
            _particleSystem.Play();
        }
    }
}
