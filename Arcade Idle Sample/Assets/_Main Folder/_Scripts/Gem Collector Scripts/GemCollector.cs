using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollector : MonoBehaviour
{
    bool gemTaken = true;
    int gems = 0;

    GemCollectorComplex _gc_complex;
    Transform smasher;

    private void Start()
    {
        smasher = transform.GetChild(0);
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
        gems++;
        yield return new WaitForSeconds(GameManager.instance._managerData.GC_production_rate);
        gemTaken = true;
    }
}
