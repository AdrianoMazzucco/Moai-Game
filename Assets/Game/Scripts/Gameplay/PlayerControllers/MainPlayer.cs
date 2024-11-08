using UnityEngine;

public class MainPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.playerGameObject = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
