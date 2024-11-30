using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class MineralCountScript : MonoBehaviour
{
    [SerializeField] private int totalHP = 3;
    public int currentHP = 3;

    [SerializeField] private int totalMineralCount = 500;
    [SerializeField] private int startingMineralCount = 0;
    [SerializeField] private int mineralsLostOnHit = 20;
    public int currentMineralCount = 0;

    private int healCounter = 0;
    [SerializeField] private int mineralRequiredtoHeal = 20;
    [SerializeField] private int maxMineralToHealth = 60;

    [SerializeField] private TMP_Text mineralCountDisplay;
    [SerializeField] private TMP_Text healthCountDisplay;
    [SerializeField] private PhysicsBasedPlayerMovement playerController;

    [SerializeField] private float damageCoolDownTimer = 1;
    private float damageTime = 0;

    [SerializeField] private GameObject respawnLocation;


    #region Events

    public UnityEvent<float> OnMineralCountChange;
    public UnityEvent<string> OnMineralCountChangeTXT;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MineralStateMachine>())
        {
            currentMineralCount++;
            Destroy( collision.gameObject);

            if(currentHP < totalHP) 
            {
                if (healCounter > maxMineralToHealth)
                {

                }
                healCounter++;
                if(mineralRequiredtoHeal > healCounter)
                {
                    currentHP = 1;
                    
                } else if (mineralRequiredtoHeal <= healCounter && mineralRequiredtoHeal *2 > healCounter)
                {
                    currentHP = 1;

                } else if (mineralRequiredtoHeal*2 <= healCounter && healCounter < maxMineralToHealth) {
                    currentHP = 2;

                } else if (healCounter == maxMineralToHealth) {
                    currentHP = 3;
                    
                }

            }

            UpdateMineralCount();
        }
    }

    public void TakeDamage()
    {
        if (damageTime <= 0 && currentHP > 0)
        {
            currentHP--;
            currentMineralCount -= mineralsLostOnHit;
            damageTime = damageCoolDownTimer;
            UpdateMineralCount();

            if(currentHP <= 0) 
            {
                currentHP = totalHP;
                currentMineralCount = startingMineralCount;
                UpdateMineralCount();
                
                if (respawnLocation != null)
                {
                    transform.position = respawnLocation.transform.position;
                }
            }
        }
    }

    private void UpdateMineralCount()
    {
        OnMineralCountChange.Invoke(currentMineralCount);
        OnMineralCountChangeTXT.Invoke("" + currentMineralCount);
        
        if (mineralCountDisplay != null)
            mineralCountDisplay.text = "Minerals: " + currentMineralCount;

        if (healthCountDisplay != null)
            healthCountDisplay.text = "HP: " + currentHP;

        if (playerController != null)
            playerController.UpdateStats(currentMineralCount);
    }
}
