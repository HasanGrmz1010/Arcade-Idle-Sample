using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GuardTowerAmmo : MonoBehaviour
{
    private float bulletSpeed;
    private Vector3 moveVec;

    void Start()
    {
        StartCoroutine(KillAfterSeconds());
        bulletSpeed = GameManager.instance._managerData.GT_bullet_speed;
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
