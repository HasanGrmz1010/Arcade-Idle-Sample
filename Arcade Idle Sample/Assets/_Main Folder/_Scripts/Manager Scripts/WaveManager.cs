using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    
    #region Singleton
    public static WaveManager instance;
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

    List<GameObject> AliveEnemies = new List<GameObject>();
    bool readyToSpawn = false;

    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject _prefab;
        public int size;
    }
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict;

    [SerializeField] Button startWaveButton;
    [SerializeField] GameObject Zombie;
    public Transform GoalTreasure;
    [SerializeField] List<Transform> spawners;

    public event EventHandler onLevelChanged;

    void Start()
    {
        onLevelChanged += InGameCanvas.instance.onLevelChanged;

        // =========== Spawn inactive pool of enemies =========== OBJECT POOLING
        poolDict = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool._prefab, transform.position, Quaternion.identity);
                obj.transform.SetParent(this.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDict.Add(pool.tag, objectPool);
        }
    }

    void Update()
    {
        if (AliveEnemies.Count == 0 && GameManager.instance._state == GameManager.State.wave_active)
        {
            GameManager.instance.IncreaseLevel();
            startWaveButton.gameObject.SetActive(true);
            GameManager.instance._state = GameManager.State.wave_inactive;
            onLevelChanged?.Invoke(this, EventArgs.Empty);
        }
        if (readyToSpawn)
        {
            StartCoroutine(StartWave(GameManager.instance._managerData.Walker_spawn_time));
            readyToSpawn = false;
        }
    }

    IEnumerator StartWave(float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);

        StartWave();
        readyToSpawn = true;
    }

    public void StartWave() // START WAVE BUTTON FUNC
    {
        startWaveButton.gameObject.SetActive(false);
        GameManager.instance._state = GameManager.State.wave_active;
        int enemyAmount = GameManager.instance._managerData.Level_EnemySeries[GameManager.instance.GetLevel()];
        for (int i = 0; i < enemyAmount / 3; i++)
        {
            int _rand_spawner = UnityEngine.Random.Range(0, spawners.Count);
            SpawnFromPool("zombie", spawners[_rand_spawner].position, Quaternion.identity);
            SpawnFromPool("goblin", spawners[_rand_spawner].position, Quaternion.identity);
            SpawnFromPool("golem", spawners[_rand_spawner].position, Quaternion.identity);
        }
    }

    #region Object Pooling Functions

    public GameObject SpawnFromPool(string _tag, Vector3 _pos, Quaternion _rot)
    {
        if (!poolDict.ContainsKey(_tag)) { return null; }

        else
        {
            GameObject obj_toSpawn = poolDict[_tag].Dequeue();
            AliveEnemies.Add(obj_toSpawn);
            obj_toSpawn.SetActive(true);
            obj_toSpawn.transform.SetParent(null);
            obj_toSpawn.transform.position = _pos;
            obj_toSpawn.transform.rotation = _rot;
            obj_toSpawn.GetComponent<IEnemy>().SetTarget(GoalTreasure);
            return obj_toSpawn;
        }
    }

    public void ReturnToPool(GameObject _obj, string _tag)
    {
        if (!poolDict.ContainsKey(_tag)) { return; }

        else
        {
            _obj.transform.position = transform.position;
            _obj.transform.SetParent(this.transform);
            _obj.SetActive(false);
            AliveEnemies.Remove(_obj);
            _obj.GetComponent<IEnemy>().setDead(true);
            poolDict[_tag].Enqueue(_obj);
        }
    }
    #endregion

}
