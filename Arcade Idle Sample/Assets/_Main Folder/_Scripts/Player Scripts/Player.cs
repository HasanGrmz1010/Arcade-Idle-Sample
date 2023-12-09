using System;
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

    [SerializeField] ParticleSystem walk_particle;
    [SerializeField] Animator _animator;
    bool isTouchingScreen, canMove;
    public float moveSpeed;

    Vector3 moveVec, initMousePos, mousePos;

    private void Start()
    {
        Treasure.instance.onGameOver_Treasure += EnableDisableMovement;
    }

    void Update()
    {
        if (canMove) HandleMovement();
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
            mousePos = Input.mousePosition;
            moveVec = mousePos - initMousePos;
            if (!_animator.GetBool("running") && moveVec.magnitude > 0.1f)
            {
                walk_particle.Play();
                _animator.SetBool("running", true);
            }
            ModifyMoveVector();

            // Set player movement borders == 30&40
            float clampedX = Mathf.Clamp(transform.position.x + moveVec.x * moveSpeed * Time.deltaTime, -30f, 30f);
            float clampedZ = Mathf.Clamp(transform.position.z + moveVec.z * moveSpeed * Time.deltaTime, -40f, 40f);

            transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
            transform.forward = Vector3.Lerp(transform.forward, moveVec, 20f * Time.deltaTime);
        }

        else if (Input.GetMouseButtonUp(0))
        {
            _animator.SetBool("running", false);
            walk_particle.Stop();
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

    public void CanMove(bool _val) { canMove = _val; }

    void EnableDisableMovement(object sender, EventArgs e)
    {
        canMove = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 7:
                Debug.Log("AAAAAA");
                break;
            default:
                break;
        }
    }

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
