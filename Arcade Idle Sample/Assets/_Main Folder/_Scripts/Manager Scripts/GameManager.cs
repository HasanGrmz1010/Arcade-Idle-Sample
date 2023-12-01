using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    private static int Level = 1;

    public ManagerSO _managerData;

    private void Start()
    {
        if (_managerData.Level_EnemySeries.Count < 1)
        {
            _managerData.Level_EnemySeries.Add(1, 5);
            _managerData.Level_EnemySeries.Add(2, 10);
            _managerData.Level_EnemySeries.Add(3, 15);
            _managerData.Level_EnemySeries.Add(4, 15);
            _managerData.Level_EnemySeries.Add(5, 20);
        }
    }

    public void IncreaseLevel() { Level++; }
    public int GetLevel() { return Level; }

}
