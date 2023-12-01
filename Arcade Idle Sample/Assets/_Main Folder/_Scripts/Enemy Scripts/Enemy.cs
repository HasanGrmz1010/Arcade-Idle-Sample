using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    bool dead;

    public Treasure _treasureScript;
    int Health;

    // ========= EVENTS ==========
    public event EventHandler onBulletHit;
    public event EventHandler onTreasureHit;
    public event EventHandler onEnemyDead;

    [SerializeField] NavMeshAgent agent;
    void Start()
    {
        onBulletHit += TakeDamage;
        onTreasureHit += InGameCanvas.instance.onTreasureHealthChanged;
        onEnemyDead += InGameCanvas.instance.onMoneyChanged;
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
            dead = true;
            Player.instance.GiveMoney(GameManager.instance._managerData.Walker_prize);
            onEnemyDead?.Invoke(this, EventArgs.Empty);
            GameManager.instance.SpawnPlayDestroyParticle(GameManager.instance._managerData.enemy_poof,
                    new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z),
                    Quaternion.identity);
            WaveManager.instance.AddEnemyToPool(this.gameObject);
        }
    }

    public bool isDead() { return dead; }
    public void setDead(bool _val) { dead = _val; }
    public void SetHealth(int _val) { Health = _val; }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 8: // BULLET HIT
                onBulletHit?.Invoke(this, EventArgs.Empty);
                GameManager.instance.SpawnPlayDestroyParticle(GameManager.instance._managerData.enemy_hit,
                    other.transform.position,
                    Quaternion.identity);
                Destroy(other.gameObject);
                break;
            case 10: // TREASURE HIT
                dead = true;
                GameManager.instance.SpawnPlayDestroyParticle(GameManager.instance._managerData.enemy_poof,
                    new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z),
                    Quaternion.identity);
                _treasureScript.TakeDamage(GameManager.instance._managerData.Walker_damage);
                onTreasureHit?.Invoke(this, EventArgs.Empty);
                WaveManager.instance.AddEnemyToPool(this.gameObject);
                break;
            default:
                break;
        }
    }
}
