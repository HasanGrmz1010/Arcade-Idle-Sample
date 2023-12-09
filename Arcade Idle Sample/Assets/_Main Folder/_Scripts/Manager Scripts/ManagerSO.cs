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

    [Header("==== HELPER BOT VARIABLES ====")]
    public float HB_fire_rate;
    public int HB_bullet_damage;
    public float HB_bullet_speed;

    public Dictionary<int, int> Level_EnemySeries = new Dictionary<int, int>
    {
        {1, 15},
        {2, 24},
        {3, 30},
        {4, 33},
        {5, 36},
        {6, 42},
        {7, 45},
        {8, 48},
        {9, 51},
        {10,54}
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
