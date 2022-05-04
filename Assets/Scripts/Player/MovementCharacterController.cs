using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementCharacterController : MonoBehaviour
{
    // Move �Է� �޾ƿ��� ����(RotateToMouse���� ����)
    [HideInInspector]
    public float xInput;
    [HideInInspector]
    public float zInput;

    // ȯ�� ����
    [SerializeField]
    private float PLAYER_GRAVITY = -20;
    [SerializeField]
    private float RUN_SPEED = 8;
    [SerializeField]
    private float WALK_SPEED = 4;
    [SerializeField]
    private float JUMP_FORCE = 10;

    [SerializeField]
    private float _playerSpeed = 0;

    private Vector3 _moveForce;

    [SerializeField]
    private GameObject _attackCollision;
    private Animator _animator = null;                                // �ִϸ��̼� �Ķ���� ������ ���� Animator�� �޾ƿ´�.
    private CharacterController _characterController = null;          // ĳ���� ��Ʈ�ѷ��� �ݶ��̴��� ������ٵ� ������ ��������Ƿ� �ҷ��´�.
    private Player _player = null;
    #region hashes
    // State
    private readonly int hashIsRunning = Animator.StringToHash("isRunning");
    private readonly int hashIsJumping = Animator.StringToHash("isJumping");
    private readonly int hashIsWalking = Animator.StringToHash("isWalking");

    // Trigger
    private readonly int hashDoJumping = Animator.StringToHash("doJumping");
    private readonly int hashDoRolling = Animator.StringToHash("doRolling");
    private readonly int hashDoAttack = Animator.StringToHash("doAttack");
    private readonly int hashDoAttack2 = Animator.StringToHash("doAttack2");

    // Animation
    private readonly int hashWalking = Animator.StringToHash("Walking");
    private readonly int hashJumping = Animator.StringToHash("Jumping");
    private readonly int hashRolling = Animator.StringToHash("Rolling");
    private readonly int hashAttackRight00 = Animator.StringToHash("Attack_Right_00");
    private readonly int hashAttackLeft00 = Animator.StringToHash("Attack_Left_00");
    private readonly int hashAttackLeft01 = Animator.StringToHash("Attack_Left_01");
    private readonly int hashAttackLeft02 = Animator.StringToHash("Attack_Left_02");

    // Values
    private readonly int hashAttackComboInteger = Animator.StringToHash("AttackCombo");

    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _playerSpeed = RUN_SPEED;
    }

    void FixedUpdate()
    {
        ApplyGravity();
        Move();
    }

    public void Jump()
    {
        _moveForce.y = JUMP_FORCE;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (!_animator.GetBool(hashIsJumping)
            && !IsJumpAnimating() && !IsRollAnimating() && !IsAttackAnimating())
        {
            _animator.SetBool(hashIsJumping, true);
            _animator.SetTrigger(hashDoJumping);
            _animator.applyRootMotion = false;
            Jump();
        }
    }

    private void Move()
    {
        _characterController.Move(_moveForce * Time.deltaTime);
    }
    public void MoveTo(Vector3 direction)
    {
        _animator.SetFloat("dirX", xInput, 0.1f, Time.deltaTime);   // �ִϸ��̼� ���� �ε巴��
        _animator.SetFloat("dirZ", zInput, 0.1f, Time.deltaTime);   // �ִϸ��̼� ���� �ε巴��2
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);
        _moveForce = new Vector3(direction.x * _playerSpeed, _moveForce.y, direction.z * _playerSpeed);

        MoveSpeed();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // �Է°��� �޾Ƽ� ȸ�� ������ �̵� ������ �޾ƿ�
        Vector2 input = context.ReadValue<Vector2>();
        xInput = input.x;
        //animator.SetFloat("dirX", xInput);
        zInput = input.y;
        //animator.SetFloat("dirZ", zInput);

        MoveTo(new Vector3(xInput, 0, zInput));

        // �ִϸ��̼� ����
        if (context.started)
        {
            _animator.SetBool(hashIsRunning, true);
        }
        else if (context.canceled)
        {
            _animator.SetBool(hashIsRunning, false);
        }
    }

    public void OnAttackLeft(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (!IsAttackLeftAnimating())
        {
            _animator.SetTrigger(hashDoAttack);
            _animator.SetInteger(hashAttackComboInteger, 0);
        }
        else if (IsAttackLeft0Animating())
        {
            _animator.SetInteger(hashAttackComboInteger, 1);
        }
        else if (IsAttackLeft1Animating())
        {
            _animator.SetInteger(hashAttackComboInteger, 2);
        }
    }

    public void OnAttackRight(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (!IsAttackRightAnimating())
        {
            _animator.SetTrigger(hashDoAttack2);
        }
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _animator.SetBool(hashIsWalking, true);
        }

        if (context.canceled)
        {
            _animator.SetBool(hashIsWalking, false);
        }
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (!IsJumpAnimating() && !IsRollAnimating())
        {
            _animator.SetTrigger(hashDoRolling);
        }
    }

    public void ApplyGravity()
    {
        if (!_characterController.isGrounded)
        {
            _moveForce.y += PLAYER_GRAVITY * Time.deltaTime;
        }
    }

    public void OnAttackCollision()
    {
        _attackCollision.SetActive(true);
    }

    public void MoveSpeed()
    {
        if (IsRollAnimating() || IsAttackAnimating())
        {
            _playerSpeed = 0;
        }
        else if (IsWalkAnimating())
        {
            _playerSpeed = WALK_SPEED;
        }
        else
        {
            _playerSpeed = RUN_SPEED;
        }
    }

    private bool IsJumpAnimating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashJumping;
    private bool IsWalkAnimating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashWalking;
    private bool IsRollAnimating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashRolling;

    private bool IsAttackAnimating() => IsAttackLeftAnimating() || IsAttackRightAnimating();
    private bool IsAttackRightAnimating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackRight00;
    private bool IsAttackLeftAnimating() => IsAttackLeft0Animating() || IsAttackLeft1Animating() || IsAttackLeft2Animating();
    private bool IsAttackLeft0Animating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackLeft00;
    private bool IsAttackLeft1Animating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackLeft01;
    private bool IsAttackLeft2Animating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackLeft02;
}

