using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

public class GuardTowerComplex : MonoBehaviour
{
    [Header("==== Tower Body Meshes ====")]
    [SerializeField] GameObject TOWER;
    [SerializeField] Transform ground;
    [SerializeField] Transform body_cube;
    [SerializeField] Transform upper_cube;
    [SerializeField] Transform turret;

    int cost;
    [Header("==== Other Variables ====")]
    [SerializeField] Image fill_img;
    [SerializeField] Image corner_img;
    [SerializeField] TextMeshProUGUI cost_text;
    [SerializeField] TextMeshProUGUI name_text;

    bool unlocked, player_inside, raised;

    private Coroutine unlocking_C;
    public event EventHandler TowerUnlocked;


    private void Start()
    {
        cost = GameManager.instance._managerData.GT_unlock_cost;
        TowerUnlocked += RaiseGuardTower;
        unlocked = false; player_inside = false; raised = false;
    }

    private void Update()
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
        if (cost < 0) {
            cost_text.gameObject.SetActive(false);
            name_text.gameObject.SetActive(false);
            TowerUnlocked?.Invoke(this, EventArgs.Empty);
            unlocked = true;
        }
        yield return new WaitForSeconds(_sec);
        cost -= 10;
        fill_img.fillAmount -= .025f;
        player_inside = true;
    }

    void RaiseGuardTower(object sender, EventArgs e)
    {
        TOWER.SetActive(true);
        ground.DOMoveY(0f, .4f).SetEase(Ease.OutSine);
        body_cube.DOMoveY(1.6f, .4f).SetEase(Ease.OutSine).SetDelay(0.2f);
        upper_cube.DOMoveY(4.2f, .4f).SetEase(Ease.OutSine).SetDelay(0.4f);
        turret.DOMoveY(5.2f, .4f).SetEase(Ease.OutSine).SetDelay(0.6f).OnComplete(() =>
        {
            raised = true;
        });
    }

    public bool isRaised() { return raised; }
}
