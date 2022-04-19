using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementCharacterController : MonoBehaviour
{
    [HideInInspector]
    public float xInput;                     // 좌우 입력 받아오는 변수
    [HideInInspector]
    public float zInput;                     // 앞뒤 입력 받아오는 변수

    [SerializeField]
    private float runSpeed = 8;              // 뛰는 속도
    [SerializeField]
    private float walkSpeed = 4;             // 걷는 속도
    [SerializeField]
    private float applySpeed = 0;            // 적용되는 속도
    private float _applySpeed = 0;

    [SerializeField]
    private float jumpForce = 10;            // 점프력
    [SerializeField]
    private float gravity = -20;             // 중력

    private Vector3 moveForce;

    private Animator animator = null;                                // 애니메이션 파라미터 설정을 위해 Animator를 받아온다.
    private CharacterController characterController = null;          // 캐릭터 컨트롤러에 콜라이더와 리지드바디 정보가 담겨있으므로 불러온다.

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        applySpeed = runSpeed;
        _applySpeed = applySpeed;
    }

    void Update()
    {
        IsGravity();
        characterController.Move(moveForce * Time.deltaTime);       // 실제 Move 함수
    }

    public void Jump()
    {
        if (characterController.isGrounded&!animator.GetCurrentAnimatorStateInfo(0).IsName("Rolling")& !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
            &!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            moveForce.y = jumpForce;
        }
    }
    public void Jump(InputAction.CallbackContext context)   // 인풋 시스템 점프
    {
        if (context.started)
        {                                                   // isJumping이 false일 경우에만 && 현재 실행중인 애니메이션 State가 Jumping이 아닐때만
            if (!animator.GetBool("isJumping") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jumping"))
            {
                Jump();                                     // 점프
                animator.SetBool("isJumping", true);        // 점프할 때 isJumping = true
            }
        }
    }

    public void MoveTo(Vector3 direction)
    {
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);
        moveForce = new Vector3(direction.x * applySpeed, moveForce.y, direction.z * applySpeed);

        MoveAllow();
    }
    public void Move(InputAction.CallbackContext context)   // 인풋 시스템 무브
    {
        // 이동 입력값 받기
        Vector2 input = context.ReadValue<Vector2>();       // 입력값을 받아서 회전 정도랑 이동 정도를 받아옴
        xInput = input.x;
        animator.SetFloat("dirX", xInput);
        zInput = input.y;
        animator.SetFloat("dirZ", zInput);

        MoveTo(new Vector3(xInput, 0, zInput));

        // 애니메이션 설정
        if (context.started)
        {
            animator.SetBool("isRunning", true);            // 움직일 때 isRunning = true
        }
        else if (context.canceled)
        {
            animator.SetBool("isRunning", false);           // 멈췄을 때 isRunning = false
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger("onAttack");
        }

    }

    public void Attack2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger("onAttack2");
        }
    }

    public void Walk(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetBool("isWalking", true);
            applySpeed = walkSpeed;
        }

        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            applySpeed = runSpeed;
        }
    }

    public void Roll(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetBool("isRolling", true);
        }

        if (context.canceled)
        {
            animator.SetBool("isRolling", false);
        }
    }

    public void IsGravity() // 중력
    {
        if (!characterController.isGrounded)
        {
            moveForce.y += gravity * Time.deltaTime;
        }
    }

    public void MoveAllow()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Rolling"))
            applySpeed = 0;
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            applySpeed = 0;
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            applySpeed = 0;
        else if (!animator.GetBool("isWalking"))
            applySpeed = _applySpeed;
    }
}
