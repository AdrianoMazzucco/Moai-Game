using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField] private int DamageTaken = 10;

    [Header("Connections")] 
    [SerializeField] private Collider _collider;

    [SerializeField] private BoxCollider _triggerCollider;
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


    #endregion
}
