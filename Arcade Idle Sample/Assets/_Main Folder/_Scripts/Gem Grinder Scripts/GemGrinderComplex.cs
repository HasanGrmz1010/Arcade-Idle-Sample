using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GemGrinderComplex : MonoBehaviour, IComplex
{
    [Header("==== Grinder Body Meshes ====")]
    [SerializeField] GameObject GRINDER;
    [SerializeField] Transform Head;
    [SerializeField] Transform Left_cyc;
    [SerializeField] Transform Right_cyc;
    [SerializeField] Transform walls;

    int cost;
    bool raised, unlocked, player_inside;

    [Header("==== Other Variables ====")]
    [SerializeField] Image corner_img;
    [SerializeField] Image fill_img;
    [SerializeField] TextMeshProUGUI cost_text;
    [SerializeField] TextMeshProUGUI name_text;

    private Coroutine unlocking_C;
    public event EventHandler GGUnlocked;
    public event EventHandler onMoneyChanged;

    Sequence seq;
    void Start()
    {
        onMoneyChanged += InGameCanvas.instance.onMoneyChanged;


        seq = DOTween.Sequence();
        seq.Pause();
        seq.Append(Left_cyc.DORotate(new Vector3(0f, 180f, 0f), 0.25f));
        seq.Append(Left_cyc.DORotate(new Vector3(0f, 360f, 0f), 0.25f));
        seq.Append(Right_cyc.DORotate(new Vector3(0f, 180f, 0f), 0.25f));
        seq.Append(Right_cyc.DORotate(new Vector3(0f, 360f, 0f), 0.25f));
        seq.OnComplete(Animate);

        cost = GameManager.instance._managerData.GG_unlock_cost;
        GGUnlocked += RaiseBuilding;
        unlocked = false; player_inside = false; raised = false;
    }

    void Update()
    {
        if (player_inside && !unlocked && Player.instance.GetMoney() >= cost)
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

    public void Animate()
    {
        seq.Rewind();
        seq.Restart();
    }

    public void RaiseBuilding(object _sender, EventArgs e)
    {
        Player.instance.TakeMoney(GameManager.instance._managerData.GG_unlock_cost);
        onMoneyChanged?.Invoke(this, EventArgs.Empty);
        GRINDER.SetActive(true);
        Head.DOMoveY(2f, .4f).SetEase(Ease.OutSine);
        Left_cyc.DOMoveY(1f, .4f).SetEase(Ease.OutSine).SetDelay(.3f);
        Right_cyc.DOMoveY(1f, .4f).SetEase(Ease.OutSine).SetDelay(.6f);
        walls.DOMoveY(0f, .4f).SetEase(Ease.OutSine).SetDelay(.9f).OnComplete(() =>
        {
            raised = true;
            seq.Play();
        });
    }

    public IEnumerator Unlocking(float _sec)
    {
        if (cost < 0)
        {
            cost_text.gameObject.SetActive(false);
            name_text.gameObject.SetActive(false);
            GGUnlocked?.Invoke(this, EventArgs.Empty);
            unlocked = true;
        }
        yield return new WaitForSeconds(_sec);
        cost -= 10;
        fill_img.fillAmount -= .01f;
        player_inside = true;
    }

    
    public bool isRaised() { return raised; }

}
