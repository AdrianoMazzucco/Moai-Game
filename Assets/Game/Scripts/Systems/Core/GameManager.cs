using System;
using QFSW.MOP2;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : Singleton<GameManager>
{
    #region Globals

    public GameObject playerGameObject;

    public Terrain CurrentTerrain;

    public ObjectPool BoulderPool;
    #endregion

    #region Privates

    [SerializeField] private GameObject boulderGameObject;

    #endregion
    private void Start()
    {
        BoulderPool = ObjectPool.Create(boulderGameObject);
    }
}
