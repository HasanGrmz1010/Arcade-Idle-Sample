using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    int Health;

    public event EventHandler onBulletHit;

    [SerializeField] NavMeshAgent agent;
    void Start()
    {
        onBulletHit += TakeDamage;
        Health = GameManager.instance._managerData.Walker_hitpoint;
    }

    public void SetTarget(Transform _target)
    {
        agent.SetDestination(_target.position);
    }

    public void TakeDamage(object sender, EventArgs e)
    {
        Health -= GameManager.instance._managerData.GT_bullet_damage;
        if (Health <= 0)
        {
            WaveManager.instance.AddEnemyToPool(this.gameObject);
        }
    }

    public bool isDead() { return true ? Health <= 0 : false; }
    public void SetHealth(int _val) { Health = _val; }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 8:
                onBulletHit?.Invoke(this, EventArgs.Empty);
                Destroy(other.gameObject);
                break;

            default:
                break;
        }
    }


}
