using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemGrinder : MonoBehaviour
{
    bool gemGrind = true;

    GemGrinderComplex gg_complex;

    public event EventHandler onGemGrind;

    private void Start()
    {
        onGemGrind += InGameCanvas.instance.onMoneyChanged;
        gg_complex = transform.parent.GetComponent<GemGrinderComplex>();
    }

    private void Update()
    {
        if (gemGrind && gg_complex.isRaised())
        {
            StartCoroutine(GrindGems());
            gemGrind = false;
        }
    }

    IEnumerator GrindGems()
    {
        yield return new WaitForSeconds(GameManager.instance._managerData.GG_production_rate);
        Player.instance.GiveMoney(GameManager.instance._managerData.GG_gem_value);
        onGemGrind?.Invoke(this, EventArgs.Empty);
        gemGrind = true;
    }
}
