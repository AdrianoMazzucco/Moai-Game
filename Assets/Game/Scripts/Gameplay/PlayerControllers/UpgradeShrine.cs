using UnityEngine;

public class UpgradeShrine : MonoBehaviour
{
    [SerializeField] private int changeToVolcanoSpawnCount = 1;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        MineralCountScript mineralCountScript = other.GetComponent<MineralCountScript>();
        
        if(mineralCountScript != null )
        {
            if (mineralCountScript.ShrineUpgrade())
            {
                _gameManager.volcanoSpawnCountModifier += changeToVolcanoSpawnCount;
            }
        }
    }
}
