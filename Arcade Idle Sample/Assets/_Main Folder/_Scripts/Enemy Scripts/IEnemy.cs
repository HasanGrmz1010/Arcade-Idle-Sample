using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IEnemy
{
    public void SetTarget(Transform _target);
    public void TakeDamage(object _sender, EventArgs _args);
    public bool isDead();
    public void setDead(bool _val);
    public void SetHealth(int _val);

}
