using MoreMountains.Feedbacks;
using UnityEngine;

public class LookAtConnector : MonoBehaviour
{

    [SerializeField] private MMF_Player _MMFplayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _MMFplayer.GetFeedbackOfType<MMF_LookAt>().LookAtTarget = GameManager.Instance.playerGameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
