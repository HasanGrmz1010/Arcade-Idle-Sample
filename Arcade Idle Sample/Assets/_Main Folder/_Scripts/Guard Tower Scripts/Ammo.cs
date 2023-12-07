using System;
using System.Collections;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public enum AmmoType
    {
        tower,
        bot
    }
    public AmmoType type;

    private float bulletSpeed;
    private Vector3 moveVec;

    void Start()
    {
        StartCoroutine(KillAfterSeconds());
        if (type == AmmoType.tower) bulletSpeed = GameManager.instance._managerData.GT_bullet_speed;
        else if (type == AmmoType.bot) bulletSpeed = GameManager.instance._managerData.HB_bullet_speed;

    }

    void Update()
    {
        transform.position += moveVec * bulletSpeed * Time.deltaTime;
    }

    public void SetMoveVector(Vector3 _vec) { if (_vec != null) moveVec = _vec; else moveVec = Vector3.zero; }
    public Vector3 GetMoveVector() { return moveVec != null ? moveVec : Vector3.zero; }

    IEnumerator KillAfterSeconds()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Destroy(this.gameObject);
        }
    }

}
