using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Manager", menuName = "Manager")]
public class ManagerSO : ScriptableObject
{
    public int Start_Money;
    public float Treasure_Health;

    [Header("==== GEM GRINDER VARIABLES ====")]
    public int GG_unlock_cost;
    public int GG_production_rate;
    public int GG_gem_value;

    [Header("==== GEM COLLECTOR VARIABLES ====")]
    public int GC_unlock_cost;
    public int GC_production_rate;
    public int GC_gem_value;

    [Header("==== GUARD TOWER VARIABLES ====")]
    public int GT_unlock_cost;
    public float GT_fire_rate;
    public int GT_bullet_damage;
    public float GT_bullet_speed;

    public Dictionary<int, int> Level_EnemySeries = new Dictionary<int, int>
    {
        {1, 6},
        {2, 9},
        {3, 12},
        {4, 12},
        {5, 15},
        {6, 18},
        {7, 18},
        {8, 21},
        {9, 24},
        {10, 30}
    };

    [Header("==== WALKER VARIABLES ====")]
    public int Walker_pool_size;
    public float Walker_spawn_time;
    public int Walker_damage;
    public int Walker_hitpoint;
    public int Walker_prize;

    [Header("==== GOBLIN VARIABLES ====")]
    public int Goblin_pool_size;
    public float Goblin_spawn_time;
    public int Goblin_damage;
    public int Goblin_hitpoint;
    public int Goblin_prize;

    [Header("==== GOLEM VARIABLES ====")]
    public int Golem_pool_size;
    public float Golem_spawn_time;
    public int Golem_damage;
    public int Golem_hitpoint;
    public int Golem_prize;

    [Header("==== PARTICLES ====")]
    public GameObject enemy_hit;
    public GameObject enemy_poof;
}
