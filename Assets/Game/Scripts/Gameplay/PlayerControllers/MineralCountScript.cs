using TMPro;
using UnityEngine;
using VHierarchy.Libs;

public class MineralCountScript : MonoBehaviour
{
    public int currentMineralCount = 5;
    [SerializeField] private TMP_Text mineralCountDisplay;

    private void Start()
    {
        if(mineralCountDisplay != null)
            mineralCountDisplay.text = "Minerals: " + currentMineralCount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MineralStateMachine>()) 
        {
            currentMineralCount++;
            collision.gameObject.Destroy();

            if (mineralCountDisplay != null)
                mineralCountDisplay.text = "Minerals: " + currentMineralCount;
        }
    }
}
