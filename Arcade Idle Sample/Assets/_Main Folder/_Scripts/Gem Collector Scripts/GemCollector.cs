using System;
using System.Collections;
using UnityEngine;

public class GemCollector : MonoBehaviour
{
    bool gemTaken = true;

    GemCollectorComplex _gc_complex;

    public event EventHandler onGemMined;

    private void Start()
    {
        onGemMined += InGameCanvas.instance.onMoneyChanged;
        _gc_complex = transform.parent.GetComponent<GemCollectorComplex>();
    }

    private void Update()
    {
        if (gemTaken && _gc_complex.isRaised())
        {
            StartCoroutine(CollectGems());
            gemTaken = false;
        }
    }

    IEnumerator CollectGems()
    {
        yield return new WaitForSeconds(GameManager.instance._managerData.GC_production_rate);
        Player.instance.GiveMoney(GameManager.instance._managerData.GC_gem_value);
        onGemMined?.Invoke(this, EventArgs.Empty);
        gemTaken = true;
    }
}
