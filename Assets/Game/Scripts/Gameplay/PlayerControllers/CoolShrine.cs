using UnityEngine;

public class CoolShrine : MonoBehaviour
{
    [SerializeField] private int volcanoSpawnCountModifier = 3;

    private void OnTriggerEnter(Collider other)
    {
        bool sunglasses = other.GetComponent<ToggleSunglasses>().currentlyActive;
        if (sunglasses)
        {
            //Should refactor
            GameManager.Instance.volcanoSpawnCountModifier = volcanoSpawnCountModifier;
            GameManager.Instance.playerGameObject.GetComponent<MineralCountScript>().CoolShrine();
            GameManager.Instance.playerGameObject.GetComponent<ToggleSunglasses>().Toggle(false);
            GetComponent<ToggleSunglasses>().Toggle(true);
        }
    }
}
