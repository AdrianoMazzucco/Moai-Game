using MoreMountains.Feedbacks;
using UnityEngine;

public class AnimationEventLink : MonoBehaviour
{
    [SerializeField] private MMF_Player feel;


    public void PlaySound()
    {
        feel?.PlayFeedbacks();
    }
}
