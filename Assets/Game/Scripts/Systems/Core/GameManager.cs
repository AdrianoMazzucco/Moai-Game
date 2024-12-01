using System;
using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using QFSW.MOP2;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class GameManager : MMSingleton<GameManager>
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        DOTween.SetTweensCapacity(500,500);
        BoulderPool = ObjectPool.Create(boulderGameObject);
        DecalPool = ObjectPool.Create(decalGameObject);
        DestructionFXPool = ObjectPool.Create(destructionFXObject);
        MineralPool = ObjectPool.Create(mineralRuby);


        MineralPool.GetObject(new Vector3(122.679352f, 3.46000004f, 767.54425f));
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region Methods

    public void ShakeCamera()
    {
        Camera_Shake_MMF.PlayFeedbacks();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        volcanoSpawnCountModifier = 0;

        foreach (var terrain in Terrain.activeTerrains)
        {
            if (terrain.name == "LevelTerrain")
            {
                CurrentTerrain = terrain;
                break;
            }
        }
       
    }

    #endregion
}
