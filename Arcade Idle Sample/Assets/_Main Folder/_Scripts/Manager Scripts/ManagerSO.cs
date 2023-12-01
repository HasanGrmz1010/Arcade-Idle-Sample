using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Manager", menuName = "Manager")]
public class ManagerSO : ScriptableObject
{
    [Header("==== GUARD TOWER VARIABLES ====")]
    public int GC_unlock_cost;
    public int GC_production_rate;

    [Header("==== GUARD TOWER VARIABLES ====")]
    public int GT_unlock_cost;
    public float GT_fire_rate;
    public int GT_bullet_damage;
    public float GT_bullet_speed;

    public Dictionary<int, int> Level_EnemySeries = new Dictionary<int, int>();

    [Header("==== WALKER ENEMY VARIABLES ====")]
    public int Walker_pool_size;
    public float Walker_spawn_time;
    public float Walker_damage;
    public int Walker_hitpoint;
}
