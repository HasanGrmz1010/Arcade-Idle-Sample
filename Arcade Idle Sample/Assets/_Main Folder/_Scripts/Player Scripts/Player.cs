using UnityEngine;

public class Player : MonoBehaviour
{
    #region Singleton
    public static Player instance;

    private void Awake()
    {
        money = GameManager.instance._managerData.Start_Money;
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

    private int money;

    [SerializeField] Animator _animator;
    bool isTouchingScreen;
    public float moveSpeed;

    Vector3 moveVec, initMousePos, mousePos;

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

    #region Economy Scripts
    public void GiveMoney(int _val) { money += _val; }
    public void TakeMoney(int _val) {
        if (money <= 0){return;}
        else if ((money - _val) < 0) { money = 0; }
        else { money -= _val; }
    }
    public int GetMoney() { return money; }
    #endregion
}
