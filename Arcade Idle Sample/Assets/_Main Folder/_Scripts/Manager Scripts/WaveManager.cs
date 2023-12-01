using System.Collections;
using System.Collections.Generic;
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

    bool readyToSpawn = false;
    [SerializeField] private Queue<GameObject> EnemyPool = new Queue<GameObject>();

    [SerializeField] Button startWaveButton;
    [SerializeField] GameObject Enemy;
    [SerializeField] Transform GoalTreasure;
    [SerializeField] List<Transform> spawners;

    void Start()
    {
        // =========== Spawn inactive pool of enemies ===========
        int _pool_size = GameManager.instance._managerData.Walker_pool_size;
        for (int i = 0; i < _pool_size; i++)
        {
            GameObject _enemy = Instantiate(Enemy, transform.position, Quaternion.identity);
            _enemy.transform.SetParent(this.transform);
            EnemyPool.Enqueue(_enemy);
            _enemy.SetActive(false);
        }
    }

    void Update()
    {
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
        int enemyAmount = GameManager.instance._managerData.Level_EnemySeries[GameManager.instance.GetLevel()];
        for (int i = 0; i < enemyAmount; i++)
        {
            int _rand_spawner = Random.Range(0, spawners.Count);
            TakeEnemyFromPool(spawners[_rand_spawner].position, Quaternion.identity);
        }
    }

    public void AddEnemyToPool(GameObject _enemy)
    {
        _enemy.transform.position = transform.position;
        _enemy.transform.SetParent(this.transform);
        _enemy.SetActive(false);
        EnemyPool.Enqueue(_enemy);
    }

    public GameObject TakeEnemyFromPool(Vector3 _pos, Quaternion _qua)
    {
        GameObject _taken_Enemy = EnemyPool.Dequeue();
        _taken_Enemy.transform.SetParent(null);
        _taken_Enemy.SetActive(true);
        _taken_Enemy.GetComponent<Enemy>().SetTarget(GoalTreasure);
        _taken_Enemy.GetComponent<Enemy>().SetHealth(GameManager.instance._managerData.Walker_hitpoint);
        _taken_Enemy.transform.position = _pos; _taken_Enemy.transform.rotation = _qua;
        return _taken_Enemy;
    }
}
