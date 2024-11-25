using TMPro;
using UnityEngine;
using VHierarchy.Libs;

public class MineralCountScript : MonoBehaviour
{
    [SerializeField] private int totalHP = 5;
    public int currentHP = 5;

    [SerializeField] private int totalMineralCount = 500;
    public int currentMineralCount = 50;

    private int healCounter = 0;

    [SerializeField] private TMP_Text mineralCountDisplay;
    [SerializeField] private PhysicsBasedPlayerMovement playerController;

    [SerializeField] private float damageCoolDownTimer = 1;
    private float damageTime = 0;

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
            collision.gameObject.Destroy();

            if(currentHP < totalHP) 
            {
                healCounter++;
                if(healCounter >= 10)
                {
                    currentHP++;
                    healCounter = 0;
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
            currentMineralCount -= 10;
            damageTime = damageCoolDownTimer;
            UpdateMineralCount();
        }
    }

    private void UpdateMineralCount()
    {
        if (mineralCountDisplay != null)
            mineralCountDisplay.text = "Minerals: " + currentMineralCount + "\n HP: "  + currentHP;

        if(playerController != null)
            playerController.UpdateStats(currentMineralCount);
    }
}
