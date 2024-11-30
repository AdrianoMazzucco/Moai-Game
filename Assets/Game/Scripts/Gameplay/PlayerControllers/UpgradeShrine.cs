using UnityEngine;

public class UpgradeShrine : MonoBehaviour
{
    [SerializeField] private float changeToVolcanoSpeed = 0.3f;
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
                _gameManager.volcanoSpawnTimeOffset += changeToVolcanoSpeed;
            }
        }
    }
}
