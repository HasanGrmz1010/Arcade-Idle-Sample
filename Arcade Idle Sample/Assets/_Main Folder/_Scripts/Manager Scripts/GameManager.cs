using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

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

    int Level = 1;

    public ManagerSO _managerData;

    public enum State
    {
        wave_active,
        wave_inactive
    }
    public State _state = State.wave_inactive;

    public void IncreaseLevel() { Level++; }
    public int GetLevel() { return Level; }

    public void SpawnPlayDestroyParticle(GameObject _particle, Vector3 _pos, Quaternion _rot)
    {
        GameObject _particleObj = Instantiate(_particle, _pos, _rot);
        ParticleSystem effect = _particleObj.GetComponent<ParticleSystem>();
        effect.Play();
        float duration = effect.main.duration + .5f;
        Destroy(_particleObj, duration);
    }
}
