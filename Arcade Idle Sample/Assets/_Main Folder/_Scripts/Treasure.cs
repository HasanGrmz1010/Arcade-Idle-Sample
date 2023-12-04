using UnityEngine;

public class Treasure : MonoBehaviour
{
    float treasure_health;

    private void Awake()
    {
        treasure_health = GameManager.instance._managerData.Treasure_Health;
    }

    public void TakeDamage_Treasure(int _val)
    {
        treasure_health -= _val;
    }
    public float GetHealth() { return treasure_health; }
}
