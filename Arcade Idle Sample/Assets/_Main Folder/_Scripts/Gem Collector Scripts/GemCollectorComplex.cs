using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GemCollectorComplex : MonoBehaviour
{
    [Header("==== Collector Body Meshes ====")]
    [SerializeField] GameObject COLLECTOR;
    [SerializeField] Transform base_cube;
    [SerializeField] Transform smasher;

    bool raised, unlocked, player_inside;

    int cost;
    [Header("==== Other Variables ====")]
    [SerializeField] Image fill_img;
    [SerializeField] Image corner_img;
    [SerializeField] TextMeshProUGUI cost_text;
    [SerializeField] TextMeshProUGUI name_text;

    private Coroutine unlocking_C;
    public event EventHandler GCUnlocked;


    Sequence seq;
    void Start()
    {
        seq = DOTween.Sequence();
        seq.Pause();
        seq.Append(smasher.DOMoveY(-1f, .4f).SetEase(Ease.InOutBounce));
        seq.Append(smasher.DOMoveY(2f, 2f).SetEase(Ease.InCubic).SetDelay(.4f));
        seq.SetDelay(1.25f);
        seq.OnComplete(SmashGround);


        cost = GameManager.instance._managerData.GC_unlock_cost;
        GCUnlocked += RaiseGemCollector;
        unlocked = false; player_inside = false; raised = false;
    }

    void Update()
    {
        if (player_inside && !unlocked)
        {
            unlocking_C = StartCoroutine(Unlocking(.05f));
            player_inside = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 3:
                player_inside = true;
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 3:
                player_inside = false;
                if (unlocking_C != null) { StopCoroutine(unlocking_C); }
                break;
            default:
                break;
        }
    }

    IEnumerator Unlocking(float _sec)
    {
        if (cost < 0)
        {
            cost_text.gameObject.SetActive(false);
            name_text.gameObject.SetActive(false);
            GCUnlocked?.Invoke(this, EventArgs.Empty);
            unlocked = true;
        }
        yield return new WaitForSeconds(_sec);
        cost -= 5;
        fill_img.fillAmount -= .025f;
        player_inside = true;
    }

    void RaiseGemCollector(object sender, EventArgs e)
    {
        COLLECTOR.SetActive(true);
        base_cube.DOMoveY(0f, .4f).SetEase(Ease.OutSine);
        smasher.DOMoveY(2f, .4f).SetEase(Ease.OutSine).SetDelay(.4f).OnComplete(() =>
        {
            raised = true;
            seq.Play();
        });
    }

    void SmashGround()
    {
        seq.Rewind();
        seq.Restart();
    }

    public bool isRaised() { return raised; }
}
