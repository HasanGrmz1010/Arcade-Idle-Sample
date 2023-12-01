using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int money;

    [SerializeField] Animator _animator;
    Rigidbody rgb;
    bool isTouchingScreen;
    public float moveSpeed;

    Vector3 moveVec, initMousePos, mousePos;

    void Start()
    {
        rgb = GetComponent<Rigidbody>();
        GiveMoney(999999);
    }

    void Update()
    {
        HandleMovement();
    }

    #region MovementScripts
    void HandleMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isTouchingScreen = true;
            initMousePos = Input.mousePosition;
        }

        else if (Input.GetMouseButton(0) && isTouchingScreen)
        {
            if (!_animator.GetBool("running")) { _animator.SetBool("running", true); }
            mousePos = Input.mousePosition;
            moveVec = mousePos - initMousePos;
            ModifyMoveVector();
            transform.forward = Vector3.Lerp(transform.forward, moveVec, 20f * Time.deltaTime);
            transform.position += moveVec * moveSpeed * Time.deltaTime;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            _animator.SetBool("running", false);
            isTouchingScreen = false;
            initMousePos = Vector3.zero;
        }
    }

    void ModifyMoveVector()
    {
        moveVec.z = moveVec.y;
        moveVec.y = 0f;
        moveVec = moveVec.normalized;
    }
    #endregion

    #region Collision Scripts
    private void OnTriggerEnter(Collider col)
    {
        switch (col.gameObject.layer)
        {
            case 6:

                break;
            default:
                break;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        
    }
    #endregion

    #region Economy Scripts
    public void GiveMoney(int _val) { money = _val; }
    public int GetMoney() { return money; }
    #endregion
}
