using UnityEngine;

public class Treasure : MonoBehaviour
{
    float treasure_health;

    private void Start()
    {
        treasure_health = GameManager.instance._managerData.Treasure_Health;
    }

    public void TakeDamage(int _val)
    {
        treasure_health -= _val;
    }
    public float GetHealth() { return treasure_health; }
}
