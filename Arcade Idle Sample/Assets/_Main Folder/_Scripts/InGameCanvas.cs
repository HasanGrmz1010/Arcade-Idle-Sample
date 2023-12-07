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
    [SerializeField] TextMeshProUGUI survivedWaveText;
    [SerializeField] TextMeshProUGUI waveNumberText;
    [SerializeField] Image treasure_health_img;

    [SerializeField] RectTransform tutorial;
    [SerializeField] RectTransform gameOverTab;
    [SerializeField] Button StartWaveButton;
    [SerializeField] GameObject GameOverFade;

    ManagerSO m_so;

    private void Start()
    {
        Player.instance.CanMove(false);
        StartWaveButton.interactable = false;
        if (tutorial != null)
        {
            tutorial.gameObject.SetActive(true);
            tutorial.DOScale(1f, .3f);
        }

        m_so = GameManager.instance._managerData;

        Treasure.instance.onGameOver_Treasure += HandleGameOver;

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
            moneyText.rectTransform.DOPunchScale(new Vector3(0.025f, 0.025f, 0.025f), 0.25f);
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

    public void CloseTutorialPopup()
    {
        tutorial.DOScale(0.01f, 0.2f).OnComplete(() =>
        {
            tutorial.gameObject.SetActive(false);
            Player.instance.CanMove(true);
            StartWaveButton.interactable = true;
        });
    }

    public void HandleGameOver(object sender, EventArgs _args)
    {
        survivedWaveText.text = "YOU SURVIVED " + waveNumberText.text + " WAVES";
        StartWaveButton.interactable = false;
        GameOverFade.SetActive(true);
        gameOverTab.gameObject.SetActive(true);
        gameOverTab.DOScale(1f, 0.2f);
    }

    #region Change Event Functions
    public void onMoneyChanged(object sender, EventArgs e) { ChangeMoneyText(); }
    public void onLevelChanged(object sender, EventArgs e) { ChangeLevelText(); }
    public void onTreasureHealthChanged(object sender, EventArgs e) { ChangeHealthImage(); }
    #endregion


}
