using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Boulder : MonoBehaviour
{
    #region Variables
    [Header("Health")]
    [Space]
    [SerializeField] private int maxHp = 100;
    [SerializeField] private int hpThreshold = 10;

    private int _currentHealth;
    private Collider _collider;
    private Rigidbody _rigidbody;

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
    
    
    
    #region Unity

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
        _collider.enabled = false;
        _pieceManager.Break(lastAttacktype);
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
}