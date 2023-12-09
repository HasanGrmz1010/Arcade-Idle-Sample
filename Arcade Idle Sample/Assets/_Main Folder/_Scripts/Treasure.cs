using DG.Tweening;
using System;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    float treasure_health;
    [SerializeField] Transform body;

    #region Singleton
    public static Treasure instance;

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

        treasure_health = GameManager.instance._managerData.Treasure_Health;
    }
    #endregion

    public event EventHandler onGameOver_Treasure;

    public void TakeDamage_Treasure(int _val)
    {
        body.DOPunchScale(Vector3.one / 10, .2f, 1, .2f);
        treasure_health -= _val;
        if (treasure_health <= 0f)
        {
            onGameOver_Treasure?.Invoke(this, EventArgs.Empty);
        }
    }
    public float GetHealth() { return treasure_health; }
}
