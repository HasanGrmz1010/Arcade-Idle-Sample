using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardTower : MonoBehaviour
{
    bool reloaded = true;

    [SerializeField] Transform muzzlePoint;

    [SerializeField] private GameObject _towerBullet;
    [SerializeField] private GameObject _currentTarget;
    Transform _targetAimPoint;
    [SerializeField] Transform turret;

    List<GameObject> enemiesInRange = new List<GameObject>();
    
    GuardTowerComplex _towerComplex;

    Enemy _currentTargetScript;

    private void Start()
    {
        _towerComplex = transform.GetComponentInParent<GuardTowerComplex>();
    }

    private void Update()
    {
        if ((enemiesInRange.Count > 0 && _currentTarget == null))
        {
            FindTarget();
        }

        if (_currentTarget != null && _currentTargetScript.isDead())
        {
            enemiesInRange.Remove(_currentTarget);
            ChangeTarget();
        }

        if (_currentTarget != null)
        {
            if (enemiesInRange.Contains(_currentTarget))
            {
                turret.forward = Vector3.Slerp(turret.forward, _targetAimPoint.position - muzzlePoint.position, 5f * Time.deltaTime);
                if (reloaded)
                {
                    StartCoroutine(WaitAndShoot(GameManager.instance._managerData.GT_fire_rate));
                    reloaded = false;
                }
            }

        }
    }

    #region Trigger Functions
    private void OnTriggerEnter(Collider other)// ================= ENTER
    {
        if (_towerComplex.isRaised())
        {
            switch (other.gameObject.layer)
            {
                case 7:
                    enemiesInRange.Add(other.gameObject);
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other)// =================== STAY
    {
        if (_towerComplex.isRaised())
        {
            switch (other.gameObject.layer)
            {
                case 7:
                    if (!enemiesInRange.Contains(other.gameObject)) { enemiesInRange.Add(other.gameObject); }
                    FindTarget();
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)// ==================== EXIT
    {
        if (_towerComplex.isRaised())
        {
            switch (other.gameObject.layer)
            {
                case 7:
                    enemiesInRange.Remove(other.gameObject);
                    if (_currentTarget == other.gameObject)
                    {
                        _currentTarget = null;
                    }
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    #region Target Functions
    public void FindTarget()
    {
        if (_currentTarget == null && enemiesInRange.Count > 0)
        {
            int _rand = UnityEngine.Random.Range(0, enemiesInRange.Count);
            _currentTarget = enemiesInRange[_rand];
            _currentTargetScript = _currentTarget.GetComponent<Enemy>();
            _targetAimPoint = _currentTarget.transform.GetChild(0);
        }
    }

    public void ChangeTarget() {
        _currentTarget = null;
        _currentTargetScript = null;
    }

    public void SetTarget(GameObject _object) { if (_object != null) _currentTarget = _object; else _currentTarget = null; }

    public GameObject GetTarget() { return _currentTarget != null ? _currentTarget : null; }
    #endregion

    void Shoot()
    {
        GameObject _bullet = Instantiate(_towerBullet, muzzlePoint.position, Quaternion.identity);
        GuardTowerAmmo _ammoScript = _bullet.GetComponent<GuardTowerAmmo>();
        Vector3 _bullet_vec = _targetAimPoint.position - muzzlePoint.position;
        _ammoScript.SetMoveVector(_bullet_vec);
      
    }

    IEnumerator WaitAndShoot(float _rate)
    {
        Shoot();
        yield return new WaitForSeconds(_rate);
        reloaded = true;
    }
}
