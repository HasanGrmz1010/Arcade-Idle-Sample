using System;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour, IEnemy
{
    const string obj_tag = "zombie";
    bool dead;
    float _speed;

    int Health;

    // ========= EVENTS ==========
    public event EventHandler onBulletHit;
    public event EventHandler onBotBulletHit;
    public event EventHandler onTreasureHit;
    public event EventHandler onEnemyDead;

    [SerializeField] NavMeshAgent agent;

    GameManager manager = GameManager.instance;
    void Start()
    {
        _speed = agent.speed;

        Treasure.instance.onGameOver_Treasure += DisableMovement;
        onBulletHit += TakeDamage;
        onBotBulletHit += TakeBotDamage;
        onTreasureHit += InGameCanvas.instance.onTreasureHealthChanged;
        onEnemyDead += InGameCanvas.instance.onMoneyChanged;
        Health = manager._managerData.Walker_hitpoint;
    }

    public void SetTarget(Transform _target)
    {
        agent.SetDestination(_target.position);
    }

    public void TakeDamage(object sender, EventArgs e)
    {
        Health -= manager._managerData.GT_bullet_damage;
        if (Health <= 0)
        {
            dead = true;
            Player.instance.GiveMoney(manager._managerData.Walker_prize);
            onEnemyDead?.Invoke(this, EventArgs.Empty);
            manager.SpawnPlayDestroyParticle(manager._managerData.enemy_poof,
                    new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z),
                    Quaternion.identity);
            WaveManager.instance.ReturnToPool(this.gameObject, obj_tag);
        }
    }

    public void TakeBotDamage(object sender, EventArgs e)
    {
        Health -= manager._managerData.HB_bullet_damage;
        if (Health <= 0)
        {
            dead = true;
            Player.instance.GiveMoney(manager._managerData.Walker_prize);
            onEnemyDead?.Invoke(this, EventArgs.Empty);
            manager.SpawnPlayDestroyParticle(manager._managerData.enemy_poof,
                    new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z),
                    Quaternion.identity);
            WaveManager.instance.ReturnToPool(this.gameObject, obj_tag);
        }
    }

    public bool isDead() { return dead; }
    public void setDead(bool _val) { dead = _val; }
    public void SetHealth(int _val) { Health = _val; }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 11:
                onBotBulletHit?.Invoke(this, EventArgs.Empty);
                manager.SpawnPlayDestroyParticle(manager._managerData.enemy_hit,
                    other.transform.position,
                    Quaternion.identity);
                Destroy(other.gameObject);
                break;
            case 8: // BULLET HIT
                onBulletHit?.Invoke(this, EventArgs.Empty);
                manager.SpawnPlayDestroyParticle(manager._managerData.enemy_hit,
                    other.transform.position,
                    Quaternion.identity);
                Destroy(other.gameObject);
                break;
            case 10: // TREASURE HIT
                dead = true;
                manager.SpawnPlayDestroyParticle(manager._managerData.enemy_poof,
                    new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z),
                    Quaternion.identity);
                WaveManager.instance.GoalTreasure.GetComponent<Treasure>().TakeDamage_Treasure(manager._managerData.Walker_damage);
                onTreasureHit?.Invoke(this, EventArgs.Empty);
                WaveManager.instance.ReturnToPool(this.gameObject, obj_tag);
                break;
            default:
                break;
        }
    }

    public void CanMove(bool _val)
    {
        if (!_val)
        {
            agent.speed = 0f;
        }
        else
        {
            agent.speed = _speed;
        }
    }

    public void DisableMovement(object _sender, EventArgs _args)
    {
        agent.speed = 0f;
        WaveManager.instance.ReturnToPool(this.gameObject, obj_tag);
    }
}
