using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MineralCountScript : MonoBehaviour
{
    #region Stats
    //[SerializeField] private int totalHP = 3;
    //public int currentHP = 3;

    [SerializeField] private int totalMineralCount = 50;
    [SerializeField] private int startingMineralCount = 0;
    [SerializeField] private int mineralsLostOnHit = 10;
    public int currentMineralCount = 0;

    //private int healCounter = 0;
    //[SerializeField] private int mineralRequiredtoHeal = 20;
    //[SerializeField] private int maxMineralToHealth = 60;

    [SerializeField] private TMP_Text mineralCountDisplay;
    [SerializeField] private TMP_Text healthCountDisplay;
    [SerializeField] private PhysicsBasedPlayerMovement playerController;

    [SerializeField] private float damageCoolDownTimer = 1;
    private float damageTime = 0;

    //[SerializeField] private GameObject respawnLocation;
    [SerializeField] private int mineralsRequiredforShrine = 40;

    [Header("Shrine Upgrade values")]
    [SerializeField] private int changeToMineralsDropped = -2;
    #endregion

    #region Events

    public UnityEvent<float> OnMineralCountChange;
    public UnityEvent<string> OnMineralCountChangeTXT;
    public UnityEvent MineralPickup;

    #endregion
    
    private void Start()
    {
        UpdateMineralCount();
        if(playerController == null) 
        {
            playerController = GetComponent<PhysicsBasedPlayerMovement>();
        }
    }

    private void Update()
    {
        if(damageTime > 0) { damageTime-=Time.deltaTime; }
    }

    private void OnTriggerEnter (Collider collider)
    {
        if (collider.gameObject.GetComponent<MineralStateMachine>())
        {
            currentMineralCount++;
            currentMineralCount = math.clamp(currentMineralCount, 0, 50);
            GameManager.Instance.MineralPool.Release(collider.gameObject);
            MineralPickup.Invoke();
            UpdateMineralCount();
        }
        
       
        if (collider.gameObject.layer == 16)
        {
            TakeDamage();
        }
   

    
        if (collider.gameObject.layer == 17)
        {
            TakeDamage();
        }
      
    }

    public void TakeDamage()
    {
        if (damageTime <= 0)
        {
            currentMineralCount -= mineralsLostOnHit;
            if(currentMineralCount < 0) { currentMineralCount = 0; }
            damageTime = damageCoolDownTimer;
            UpdateMineralCount();

            //if(currentHP <= 0) 
            //{
            //    currentHP = totalHP;
            //    currentMineralCount = startingMineralCount;
            //    UpdateMineralCount();
                
            //    if (respawnLocation != null)
            //    {
            //        transform.position = respawnLocation.transform.position;
            //    }
            //}
        }
    }

    private void UpdateMineralCount()
    {
        OnMineralCountChange.Invoke(currentMineralCount);
        OnMineralCountChangeTXT.Invoke("" + currentMineralCount);
        
        if (mineralCountDisplay != null)
            mineralCountDisplay.text = "Minerals: " + currentMineralCount;

        //if (healthCountDisplay != null)
        //    healthCountDisplay.text = "HP: " + currentHP;

        if (playerController != null)
            playerController.UpdateStats(currentMineralCount);
    }

    public bool ShrineUpgrade()
    {
        if(currentMineralCount > mineralsRequiredforShrine)
        {
            currentMineralCount = 0;
            mineralsLostOnHit += changeToMineralsDropped;
            if(mineralsLostOnHit <= 0) 
            {
                mineralsLostOnHit = 0;
                UpdateMineralCount();
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
