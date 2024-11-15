using UnityEngine;

public class CameraTargetMoverScript : MonoBehaviour
{
    [SerializeField] private GameObject target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position;
    }
}
