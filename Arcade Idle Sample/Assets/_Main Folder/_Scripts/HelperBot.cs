using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HelperBot : MonoBehaviour
{
    bool reloaded = true;

    [SerializeField] Transform muzzlePoint;
    [SerializeField] Transform playerFollowPoint;
    [SerializeField] Transform bot_body;
    [SerializeField] private GameObject _botBullet;
    [SerializeField] private GameObject _currentTarget;

    Transform _targetAimPoint;

    List<GameObject> enemiesInRange = new List<GameObject>();

    IEnemy _currentTargetScript;

    private void Start()
    {
        transform.DOMoveY(transform.position.y - .4f, 1.25f).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            transform.DOMoveY(transform.position.y + .4f, 1.25f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        });
    }

    private void Update()
    {
        HelperBotFunction();
        FollowPlayer();
    }

    void HelperBotFunction()
    {
        if (enemiesInRange.Count > 0 && _currentTarget == null)
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
                transform.forward = Vector3.Slerp(transform.forward,
                    _targetAimPoint.position - muzzlePoint.position,
                    20f * Time.deltaTime);
                if (reloaded)
                {
                    StartCoroutine(WaitAndShoot(GameManager.instance._managerData.HB_fire_rate));
                    reloaded = false;
                }
            }
        }
    }

    void FollowPlayer()
    {
        if (_currentTarget == null)
        {
            transform.forward = Vector3.Lerp(transform.forward,
                playerFollowPoint.parent.forward,
                5f * Time.deltaTime);
        }
            
        transform.position = Vector3.Lerp(transform.position,
            playerFollowPoint.position,
            5f * Time.deltaTime);
    }

    #region Trigger Functions
    private void OnTriggerEnter(Collider other)// ================= ENTER
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

    private void OnTriggerStay(Collider other)// =================== STAY
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

    private void OnTriggerExit(Collider other)// ==================== EXIT
    {
        switch (other.gameObject.layer)
        {
            case 7:
                enemiesInRange.Remove(other.gameObject);
                if (_currentTarget == other.gameObject) { _currentTarget = null; }
                break;
            default:
                break;
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
            _currentTargetScript = _currentTarget.GetComponent<IEnemy>();
            _targetAimPoint = _currentTarget.transform.GetChild(0);
        }
    }

    public void ChangeTarget()
    {
        _currentTarget = null;
        _currentTargetScript = null;
    }
    #endregion

    void Shoot()
    {
        GameObject _bullet = Instantiate(_botBullet, muzzlePoint.position, Quaternion.identity);
        Ammo _ammoScript = _bullet.GetComponent<Ammo>();
        Vector3 _bullet_vec = _targetAimPoint.position - muzzlePoint.position;
        _ammoScript.SetMoveVector(_bullet_vec);
    }

    IEnumerator WaitAndShoot(float _rate)
    {
        Shoot();
        bot_body.DOPunchScale(new Vector3(.2f, .2f, .2f), .1f, 1, 1f);
        //transform.DOPunchPosition((-transform.forward / 10), .1f, 1, 1f);
        yield return new WaitForSeconds(_rate);
        reloaded = true;
    }
}
