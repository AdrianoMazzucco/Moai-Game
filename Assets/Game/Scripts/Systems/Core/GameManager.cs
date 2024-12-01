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

    [HideInInspector] public float volcanoSpawnCountModifier = 0;

    #region Pools

    public ObjectPool BoulderPool;
    public ObjectPool MineralPool;
    public ObjectPool DecalPool;
    public ObjectPool DestructionFXPool;

    #endregion
 
    #endregion

    #region Privates

    [SerializeField] private GameObject boulderGameObject;
    [SerializeField] private GameObject decalGameObject;
    [SerializeField] private GameObject destructionFXObject;
    [SerializeField] private GameObject mineralRuby;


    [Header("MMFs")] [SerializeField] 
    private MMF_Player Camera_Shake_MMF;
    
    
    #endregion
    private void Start()
    {
        DOTween.SetTweensCapacity(500,500);
        BoulderPool = ObjectPool.Create(boulderGameObject);
        DecalPool = ObjectPool.Create(decalGameObject);
        DestructionFXPool = ObjectPool.Create(destructionFXObject);
        MineralPool = ObjectPool.Create(mineralRuby);
    }   

    #region Methods

    public void ShakeCamera()
    {
        Camera_Shake_MMF.PlayFeedbacks();
    }

    #endregion
}
