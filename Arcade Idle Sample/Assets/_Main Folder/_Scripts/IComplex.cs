using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComplex
{
    IEnumerator Unlocking(float _sec);
    void RaiseBuilding(object _sender, EventArgs e);
    void Animate();
    bool isRaised();
}
