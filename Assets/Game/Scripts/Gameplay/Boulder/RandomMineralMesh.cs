using UnityEngine;

public class RandomMineralMesh : MonoBehaviour
{
    [SerializeField] private GameObject[] mineralMeshs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int num = Random.Range(0, mineralMeshs.Length);
        mineralMeshs[num].gameObject.SetActive(true);
    }
}
