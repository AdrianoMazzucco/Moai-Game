using System;
using DG.Tweening;
using MoreMountains.Feedbacks;
using QFSW.MOP2;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : Singleton<GameManager>
{
    #region Globals

    public GameObject playerGameObject;
    public PhysicsBasedPlayerMovement playerMovementScript;
    public Terrain CurrentTerrain;

    #region Pools

    public ObjectPool BoulderPool;
    public ObjectPool DecalPool;
    public ObjectPool DestructionFXPool;

    #endregion
 
    #endregion

    #region Privates

    [SerializeField] private GameObject boulderGameObject;
    [SerializeField] private GameObject decalGameObject;
    [SerializeField] private GameObject destructionFXObject;



    [Header("MMFs")] [SerializeField] 
    private MMF_Player Camera_Shake_MMF;
    
    
    #endregion
    private void Start()
    {
        DOTween.SetTweensCapacity(500,500);
        BoulderPool = ObjectPool.Create(boulderGameObject);
        DecalPool = ObjectPool.Create(decalGameObject);
        DestructionFXPool = ObjectPool.Create(destructionFXObject);
    }

    #region Methods

    public void ShakeCamera()
    {
        Camera_Shake_MMF.PlayFeedbacks();
    }

    #endregion
}
