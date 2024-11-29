using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    #region Events

    public UnityEvent OnDamageTaken;

    #endregion
    
    
    #region Variables
    [Header("Values")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [SerializeField] private int FireBallDamageValue = 10;
    
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
            }
            currentHealth = value;
        }
        
    }
    

    #endregion


    #region Feel

    [Header("Feel")] 
    [SerializeField] private MMF_Player _damagedMMF;
    

    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 16)
        {
            CurrentHealth -= FireBallDamageValue;
        }
    }
}
