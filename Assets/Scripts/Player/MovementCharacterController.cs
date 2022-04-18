using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementCharacterController : MonoBehaviour
{
    [HideInInspector]
    public float xInput;                     // �¿� �Է� �޾ƿ��� ����
    [HideInInspector]
    public float zInput;                     // �յ� �Է� �޾ƿ��� ����

    [SerializeField]
    private float runSpeed = 8;              // �ٴ� �ӵ�
    [SerializeField]
    private float walkSpeed = 4;             // �ȴ� �ӵ�
    [SerializeField]
    private float applySpeed = 0;            // ����Ǵ� �ӵ�
    private float _applySpeed = 0;

    [SerializeField]
    private float jumpForce = 10;            // ������
    [SerializeField]
    private float gravity = -20;             // �߷�

    private Vector3 moveForce;

    private Animator animator = null;                                // �ִϸ��̼� �Ķ���� ������ ���� Animator�� �޾ƿ´�.
    private CharacterController characterController = null;          // ĳ���� ��Ʈ�ѷ��� �ݶ��̴��� ������ٵ� ������ ��������Ƿ� �ҷ��´�.

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
        characterController.Move(moveForce * Time.deltaTime);       // ���� Move �Լ�
    }

    public void Jump()
    {
        if (characterController.isGrounded&!animator.GetCurrentAnimatorStateInfo(0).IsName("Rolling")& !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
            &!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            moveForce.y = jumpForce;
        }
    }
    public void Jump(InputAction.CallbackContext context)   // ��ǲ �ý��� ����
    {
        if (context.started)
        {                                                   // isJumping�� false�� ��쿡�� && ���� �������� �ִϸ��̼� State�� Jumping�� �ƴҶ���
            if (!animator.GetBool("isJumping") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jumping"))
            {
                Jump();                                     // ����
                animator.SetBool("isJumping", true);        // ������ �� isJumping = true
            }
        }
    }

    public void MoveTo(Vector3 direction)
    {
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);
        moveForce = new Vector3(direction.x * applySpeed, moveForce.y, direction.z * applySpeed);

        MoveAllow();
    }
    public void Move(InputAction.CallbackContext context)   // ��ǲ �ý��� ����
    {
        // �̵� �Է°� �ޱ�
        Vector2 input = context.ReadValue<Vector2>();       // �Է°��� �޾Ƽ� ȸ�� ������ �̵� ������ �޾ƿ�
        xInput = input.x;
        animator.SetFloat("dirX", xInput);
        zInput = input.y;
        animator.SetFloat("dirZ", zInput);

        MoveTo(new Vector3(xInput, 0, zInput));

        // �ִϸ��̼� ����
        if (context.started)
        {
            animator.SetBool("isRunning", true);            // ������ �� isRunning = true
        }
        else if (context.canceled)
        {
            animator.SetBool("isRunning", false);           // ������ �� isRunning = false
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

    public void IsGravity() // �߷�
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
