using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Manager", menuName = "Manager")]
public class ManagerSO : ScriptableObject
{
    public int Start_Money;
    public float Treasure_Health;

    [Header("==== GEM COLLECTOR VARIABLES ====")]
    public int GC_unlock_cost;
    public int GC_production_rate;
    public int GC_gem_value;

    [Header("==== GUARD TOWER VARIABLES ====")]
    public int GT_unlock_cost;
    public float GT_fire_rate;
    public int GT_bullet_damage;
    public float GT_bullet_speed;

    public Dictionary<int, int> Level_EnemySeries = new Dictionary<int, int>();

    [Header("==== WALKER ENEMY VARIABLES ====")]
    public int Walker_pool_size;
    public float Walker_spawn_time;
    public int Walker_damage;
    public int Walker_hitpoint;
    public int Walker_prize;

    [Header("==== PARTICLES ====")]
    public GameObject enemy_hit;
    public GameObject enemy_poof;
}
