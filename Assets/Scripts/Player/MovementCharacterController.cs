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
    private Animator _animator = null;                                // 애니메이션 파라미터 설정을 위해 Animator를 받아온다.
    private CharacterController _characterController = null;          // 캐릭터 컨트롤러에 콜라이더와 리지드바디 정보가 담겨있으므로 불러온다.

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
    private readonly int hashAttackRight = Animator.StringToHash("Attack_Right");
    private readonly int hashAttackLeft0 = Animator.StringToHash("Attack_Left_0");
    private readonly int hashAttackLeft1 = Animator.StringToHash("Attack_Left_1");
    private readonly int hashAttackLeft2 = Animator.StringToHash("Attack_Left_2");

    // Values
    private readonly int hashAttackComboInteger = Animator.StringToHash("AttackCombo");

    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        _playerSpeed = RUN_SPEED;
    }

    void Update()
    {
        ApplyGravity();
        Move();
    }

    private void Move()
    {
        _characterController.Move(_moveForce * Time.deltaTime);
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

    public void MoveTo(Vector3 direction)
    {
        _animator.SetFloat("dirX", xInput, 0.1f, Time.deltaTime);   // 애니메이션 블렌드 부드럽게
        _animator.SetFloat("dirZ", zInput, 0.1f, Time.deltaTime);   // 애니메이션 블렌드 부드럽게2
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);
        _moveForce = new Vector3(direction.x * _playerSpeed, _moveForce.y, direction.z * _playerSpeed);

        MoveSpeed();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 이동 입력값 받기
        Vector2 input = context.ReadValue<Vector2>();       // 입력값을 받아서 회전 정도랑 이동 정도를 받아옴
        xInput = input.x;
        //animator.SetFloat("dirX", xInput);
        zInput = input.y;
        //animator.SetFloat("dirZ", zInput);

        MoveTo(new Vector3(xInput, 0, zInput));

        // 애니메이션 설정
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
        if (context.started) return;

        if (!IsAttackLeftAnimating())
        {
            _animator.SetTrigger(hashDoAttack);
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
        _animator.SetInteger(hashAttackComboInteger, 0);
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
    private bool IsAttackRightAnimating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackRight;
    private bool IsAttackLeftAnimating() => IsAttackLeft0Animating() || IsAttackLeft1Animating() || IsAttackLeft2Animating();
    private bool IsAttackLeft0Animating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackLeft0;
    private bool IsAttackLeft1Animating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackLeft1;
    private bool IsAttackLeft2Animating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackLeft2;
}
