using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Input KeyCodes")]

    private KeyCode keyCodeJump = KeyCode.Space;

    //Vector2 moveValue; // 키보드 입력 받아올 벡터공간
    //Animator anim;
    //int isWalkingHash; // 애니메이션 Walk bool
    //int isJumpingHash; // 애니메이션 Jump bool

    //[SerializeField] float jumpPower;
    //[SerializeField] float moveSpeed;
    //Rigidbody rigid;
    //private float xInput;
    //private float yInput;

    private MovementCharacterController movement;
    private RotateToMouse rotateToMouse; // 마우스 이동으로 카메라 회전 

    PlayerInput playerInput;

    private void Awake()
    {
        Cursor.visible = false; // 마우스 커서를 보이지 않게 한다.
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 현재 위치에 고정 시킨다.
        rotateToMouse = GetComponent<RotateToMouse>();
        movement = GetComponent<MovementCharacterController>();

        //playerInput = GetComponent<PlayerInput>();
        //playerCollider = GetComponent<CapsuleCollider>();

        #region
        //rigid = GetComponent<Rigidbody>();
        //anim = GetComponent<Animator>();
        //isWalkingHash = Animator.StringToHash("isWalking");
        //isJumpingHash = Animator.StringToHash("isJumping");
        //PlayerInputActions playerInputActions = new PlayerInputActions();
        //playerInputActions.Player.Jump.performed += Jump;
        //playerInputActions.Player.Move.performed += ctx => moveValue = ctx.ReadValue<Vector2>();
        #endregion
    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        //UpdateJump();

        #region
        //if (rigid.velocity.y < -0.05f)
        //{
        //    if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, 10f, LayerMask.GetMask("Ground")))
        //    {
        //        Debug.DrawRay(transform.position, Vector3.down * 0.8f, Color.blue, 0.3f);
        //        if (hit.distance < 0.2f)
        //            anim.SetBool(isJumpingHash, false);
        //    }
        //}애니메이션 isJumping false로 돌려주는 조건문, drawRay
        #endregion
    }

    private void FixedUpdate()
    {
        //rigid.AddForce(new Vector3(moveValue.x * moveSpeed, rigid.velocity.y, moveValue.y * moveSpeed));
        //rigid.velocity = new Vector3(moveValue.x * moveSpeed, rigid.velocity.y, moveValue.y * moveSpeed);
    }

    void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        movement.MoveTo(new Vector3(x, 0, z));
    }

    void UpdateJump()
    {
        if (Input.GetKeyDown(keyCodeJump))
        {
            movement.Jump();
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            //anim.SetBool(isJumpingHash, true);
            //rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            movement.Jump();
        }
    }

    //public void Move(InputAction.CallbackContext context)
    //{
    //    //anim.SetBool(isWalkingHash, true);
    //    //moveValue = context.ReadValue<Vector2>();
    //    Vector2 input = context.ReadValue<Vector2>(); // 입력값을 받아서 회전 정도랑 이동 정도를 받아옴
    //    xInput = input.x;    //A(1) D(-1)
    //    yInput = input.y;    //W(1) S(-1)
    //    //movement.MoveTo(new Vector3(xInput, 0, z));
    //    //if (context.canceled)
    //    //anim.SetBool(isWalkingHash, false);
    //}

    //void handleMovement()
    //{
    //    bool isRunning = animator.GetBool(isRunningHash);
    //    bool isWalking = animator.GetBool(isWalkingHash);

    //    if (movementPressed && !isWalking)
    //    {
    //        animator.SetBool(isWalkingHash, true);
    //    }

    //    if (!movementPressed && isWalking)
    //    {
    //        animator.SetBool(isWalkingHash, false);
    //    }

    //    if ((movementPressed && runPressed) && !isRunning)
    //    {
    //        animator.SetBool(isRunningHash, true);
    //    }

    //    if ((!movementPressed || !runPressed) && isRunning)
    //    {
    //        animator.SetBool(isRunningHash, false);
    //    }
    //}
}
