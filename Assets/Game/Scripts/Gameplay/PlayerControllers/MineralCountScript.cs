using TMPro;
using UnityEngine;
using VHierarchy.Libs;

public class MineralCountScript : MonoBehaviour
{
    public int currentMineralCount = 5;
    [SerializeField] private TMP_Text mineralCountDisplay;
    [SerializeField] private PhysicsBasedPlayerMovement playerController;

    private void Start()
    {
        UpdateMineralCount();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MineralStateMachine>())
        {
            currentMineralCount++;
            collision.gameObject.Destroy();

            UpdateMineralCount();
        }
    }

    private void UpdateMineralCount()
    {
        if (mineralCountDisplay != null)
            mineralCountDisplay.text = "Minerals: " + currentMineralCount;

        if(playerController != null)
            playerController.UpdateStats(currentMineralCount);
    }
}
