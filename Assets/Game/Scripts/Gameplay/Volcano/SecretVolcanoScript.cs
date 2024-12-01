using UnityEngine;

public class SecretVolcanoScript : MonoBehaviour
{
    [SerializeField] private GameObject[] potentialSectetVolcano;

    void Start()
    {
        int num = Random.Range(0, potentialSectetVolcano.Length);
        potentialSectetVolcano[num].gameObject.GetComponent<ToggleSunglasses>().Toggle(true);
        potentialSectetVolcano[num].gameObject.GetComponent<VolcanoHealth>().isSecretVolcano = true;        
    }

}
