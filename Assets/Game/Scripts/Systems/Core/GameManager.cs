using System;
using QFSW.MOP2;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : Singleton<GameManager>
{
    #region Globals

    public GameObject playerGameObject;

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

    #endregion
    private void Start()
    {
        BoulderPool = ObjectPool.Create(boulderGameObject);
        DecalPool = ObjectPool.Create(decalGameObject);
        DestructionFXPool = ObjectPool.Create(destructionFXObject);
    }
}
