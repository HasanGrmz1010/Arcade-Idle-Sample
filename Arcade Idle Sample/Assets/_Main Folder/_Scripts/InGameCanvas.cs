using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameCanvas : MonoBehaviour
{
    #region Singleton
    public static InGameCanvas instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    [SerializeField] Treasure _treasureScript;
    [SerializeField] Player _playerScript;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Image treasure_health_img;

    ManagerSO m_so;

    private void Start()
    {
        m_so = GameManager.instance._managerData;
        UpdateVariables();
    }

    void UpdateVariables()
    {
        ChangeMoneyText();
        ChangeLevelText();
        ChangeHealthImage();
    }

    void ChangeMoneyText()
    {
        if (!moneyText.text.Equals(_playerScript.GetMoney().ToString()))
        {
            moneyText.text = _playerScript.GetMoney().ToString();
            moneyText.rectTransform.DOPunchScale(new Vector3(0.05f, 0.05f, 0.05f), 0.5f);
        }
        else return;
    }

    void ChangeLevelText()
    {
        if (!levelText.text.Equals(GameManager.instance.GetLevel().ToString()))
        {
            levelText.text = GameManager.instance.GetLevel().ToString();
            levelText.rectTransform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f);
        }
        else return;
    }

    void ChangeHealthImage()
    {
        treasure_health_img.fillAmount = (_treasureScript.GetHealth() / m_so.Treasure_Health);
    }

    #region Change Event Functions
    public void onMoneyChanged(object sender, EventArgs e) { ChangeMoneyText(); }
    public void onLevelChanged(object sender, EventArgs e) { ChangeLevelText(); }
    public void onTreasureHealthChanged(object sender, EventArgs e) { ChangeHealthImage(); }
    #endregion


}
