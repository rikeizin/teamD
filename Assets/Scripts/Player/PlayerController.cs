using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IBattle
{
    // Move 입력 받아오는 변수(RotateToMouse에서 접근)
    [HideInInspector]
    public float xInput;
    [HideInInspector]
    public float zInput;

    // 환경 변수
    public float PLAYER_GRAVITY = -20;
    [SerializeField]
    private float RUN_SPEED = 8;
    [SerializeField]
    private float WALK_SPEED = 4;
    [SerializeField]
    private float JUMP_FORCE = 10;

    private int _arrowCool = 1;
    private bool _isArrowCool = false;
    private Vector3 _moveForce;
    public bool isMove2D = false;
    private bool _isDead = false;

    public AudioSource p_AudioSource;
    public AudioClip p_Sword;
    public AudioClip p_wand;
    public AudioClip p_meteor;
    public AudioClip p_Mace;
    public AudioClip p_arrow;

    public GameObject camera2D;
    public GameObject camera3D;
    private Animator _animator = null;                                // 애니메이션 파라미터 설정을 위해 Animator를 받아온다.
    [HideInInspector]
    public CharacterController characterController = null;          // 캐릭터 컨트롤러에 콜라이더와 리지드바디 정보가 담겨있으므로 불러온다.
    [HideInInspector]
    private MoveType2D _move2D = null;
    private Player_Swap _swap = null;
    private Player _player = null;

    // 특수공격
    public Transform p_MagicTransform;
    public Transform p_MeteorTransform;
    public Rigidbody p_Arrow;
    public Rigidbody p_Wand;
    public Rigidbody p_Meteor;
    private float _lunchForce = 30f;

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
    private readonly int hashAttackLeftWand = Animator.StringToHash("Attack_Left_Wand");
    private readonly int hashAttackRightWand = Animator.StringToHash("Attack_Right_Wand");
    private readonly int hashAttackBow01 = Animator.StringToHash("Attack_Bow_Draw");
    private readonly int hashAttackBow02 = Animator.StringToHash("Attack_Bow_OverDraw");
    private readonly int hashAttackBow03 = Animator.StringToHash("Attack_Bow_Recoil");




    // Values
    private readonly int hashAttackComboInteger = Animator.StringToHash("AttackCombo");
    private readonly int hashEquipState = Animator.StringToHash("EquipState");

    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _move2D = GetComponent<MoveType2D>();
        _swap = GetComponent<Player_Swap>();
        _player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
        p_AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _player.applySpeed = RUN_SPEED;
    }

    void Update()
    {
        if (isMove2D == false)
        {
            Move(_moveForce);
        }
        else
        {
            Camera2DMove();
        }
    }

    void FixedUpdate()
    {
        ApplyGravity();
    }

    private void OnEnable()
    {
        Invoke("SceneCheck", 0.3f);
    }

    public void Jump()
    {
        if (isMove2D == false)
            _moveForce.y = JUMP_FORCE * _player.jump;
        else
            _move2D._moveForce.y = JUMP_FORCE * _player.jump;
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

    public void Move(Vector3 direction)
    {
        _animator.SetFloat("dirX", xInput, 0.1f, Time.deltaTime);   // 애니메이션 블렌드 부드럽게
        _animator.SetFloat("dirZ", zInput, 0.1f, Time.deltaTime);   // 애니메이션 블렌드 부드럽게2
        characterController.Move(direction * Time.deltaTime);
    }

    public void MoveTo(Vector3 direction)
    {
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);
        _moveForce = new Vector3(direction.x * _player.applySpeed, _moveForce.y, direction.z * _player.applySpeed);

        MoveSpeed();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isMove2D == false)
        {
            // 입력값을 받아서 회전 정도랑 이동 정도를 받아옴
            Vector2 input = context.ReadValue<Vector2>();
            xInput = input.x;
            zInput = input.y;

            MoveTo(new Vector3(xInput, 0, zInput));
        }
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
        if (Player.IsGamePaused == false)
        {
            if (context.started)
            {
                if ((_swap.equipWeaponIndex == 3) && !IsAttackAnimating() && !IsRollAnimating())
                {
                    p_AudioSource.clip = p_wand;
                    p_AudioSource.Play();
                    StartCoroutine(FireWand());
                }

                if (!(IsAttackLeftAnimating() || IsAttackRightAnimating() || IsMagicAttackAnimating()))
                {
                    if (IsAttackBow02Animating())
                    {
                        _animator.SetTrigger(hashDoAttack);
                        if ((_player.arrow > 0) && _isArrowCool == false)
                        {
                            p_AudioSource.clip = p_arrow;
                            p_AudioSource.Play();
                            StartCoroutine(FireArrow());
                            _isArrowCool = true;
                        }
                        else if (_player.arrow <= 0)
                        {
                            return;
                        }
                    }
                    else
                    {
                        _animator.SetTrigger(hashDoAttack);
                        _animator.SetInteger(hashAttackComboInteger, 0);
                    }

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

            if (context.canceled)
            {
                _animator.ResetTrigger(hashDoAttack);
            }
        }
    }

    public void OnAttackRight(InputAction.CallbackContext context)
    {
        if (Player.IsGamePaused == false)
        {
            if (!context.started)
            {
                if ((_swap.equipWeaponIndex == 3) && !IsAttackAnimating())
                {
                    p_AudioSource.clip = p_meteor;
                    p_AudioSource.Play();
                    StartCoroutine(FireMeteor());
                }

                if (!IsAttackAnimating())
                {
                    _animator.SetTrigger(hashDoAttack2);
                }
            }

            if (context.canceled)
            {
                _animator.ResetTrigger(hashDoAttack2);
            }
        }

    }

    public void Attack(IBattle target)
    {
        if (target != null)
        {
            float damage = _player.attackPower;
            if (Random.Range(0.0f, 1.0f) < _player.critical)
            {
                damage *= 2.0f;
            }
            target.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage - _player.defence;
        if (finalDamage < 1.0f)
        {
            finalDamage = 1.0f;
        }

        if (!(Random.Range(0, 100) < _player.evasion))
        {
            _player.hp -= finalDamage;
            Debug.Log($"{gameObject.name} : {finalDamage} 데미지 입음");
        }
        else
        {
            Debug.Log("회피");
        }

        if (_player.hp <= 0.0f)
        {
            if(_isDead == false)
            {
                _isDead = true;
                _animator.SetTrigger("Dead");
                StartCoroutine(Die());
            }
        }
        _player.hp = Mathf.Clamp(_player.hp, 0.0f, _player.maxHp);
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

    IEnumerator Die()
    {
        while (true)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.97)
            {
                gameObject.SetActive(false);
            }
            yield return null;
        }
    }

    public void ApplyGravity()
    {
        _moveForce.y += PLAYER_GRAVITY * Time.deltaTime;
    }

    public void MoveSpeed()
    {
        if (IsRollAnimating() || IsAttackAnimating())
        {
            _player.applySpeed = 0;
            _move2D.moveSpeed2D = 0;
        }
        else if (IsWalkAnimating())
        {
            _player.applySpeed = WALK_SPEED * _player.speed;
            _move2D.moveSpeed2D = WALK_SPEED / 2 * _player.speed;
        }
        else
        {
            _player.applySpeed = RUN_SPEED * _player.speed;
            _move2D.moveSpeed2D = RUN_SPEED / 2 * _player.speed;
        }
    }

    public void Camera2DMove()
    {
        if (camera2D != null)
        {
            camera2D?.transform.LookAt(this.transform.position);
            camera2D.transform.position = new Vector3(0, this.transform.position.y, 0);
        }

    }

    private void SceneCheck()
    {
        if (SceneManager.GetActiveScene().name == "Stage_OutOfTower")
        {
            camera3D.GetComponent<Camera>().enabled = false;
            camera2D = GameObject.Find("Camera");
            isMove2D = true;
            _move2D.enabled = true;
        }
        else
        {
            camera3D.GetComponent<Camera>().enabled = true;
            camera2D = null;
            isMove2D = false;
            _move2D.enabled = false;
        }
    }

    IEnumerator FireMeteor()
    {
        while (true)
        {
            if (IsAttackRightWandAnimating() && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6)
            {
                Rigidbody Meteor = Instantiate(p_Meteor, p_MeteorTransform.position, p_MeteorTransform.rotation) as Rigidbody;
                Meteor.velocity = 15 * p_MeteorTransform.forward;
                //Meteor.velocity = 2 * p_MeteorTransform.up * -1;
                Destroy(Meteor.gameObject, 3.0f);
                break;
            }
            yield return null;
        }
    }

    IEnumerator FireArrow()
    {
        Rigidbody Arrow = Instantiate(p_Arrow, p_MagicTransform.position, p_MagicTransform.rotation) as Rigidbody;
        Arrow.velocity = _lunchForce * p_MagicTransform.forward;
        _player.arrow--;
        Destroy(Arrow.gameObject, 4.0f);
        yield return new WaitForSeconds(_arrowCool);
        _isArrowCool = false;
    }

    IEnumerator FireWand()
    {
        while (true)
        {
            if (IsAttackLeftWandAnimating() && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.45)
            {
                Rigidbody Wand1 = Instantiate(p_Wand, p_MagicTransform.position, p_MagicTransform.rotation) as Rigidbody;
                //Rigidbody Wand2 = Instantiate(p_Wand, p_AttackTransform.position, p_AttackTransform.rotation) as Rigidbody;
                //Rigidbody Wand3 = Instantiate(p_Wand, p_AttackTransform.position, p_AttackTransform.rotation) as Rigidbody;

                Wand1.velocity = 14 * p_MagicTransform.forward;
                //yield return new WaitForSeconds(0.1f);
                //Wand2.velocity = 12 * p_AttackTransform.forward;
                //yield return new WaitForSeconds(0.1f);
                //Wand3.velocity = 10 * p_AttackTransform.forward;

                Destroy(Wand1.gameObject, 3.0f);
                //Destroy(Wand2.gameObject, 3.0f);
                //Destroy(Wand3.gameObject, 3.0f);
                break;
            }
            yield return null;
        }
    }
    #region
    private bool IsJumpAnimating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashJumping;
    private bool IsWalkAnimating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashWalking;
    private bool IsRollAnimating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashRolling;

    public bool IsAttackAnimating() => IsAttackLeftAnimating() || IsAttackRightAnimating() || IsMagicAttackAnimating() || IsAttackBowAnimating();
    private bool IsAttackLeftAnimating() => IsAttackLeft0Animating() || IsAttackLeft1Animating() || IsAttackLeft2Animating() || IsAttackLeftWandAnimating();
    private bool IsAttackRightAnimating() => IsAttackRight0Animating() || IsAttackRightWandAnimating();
    private bool IsAttackBowAnimating() => IsAttackBow01Animating() || IsAttackBow02Animating() || IsAttackBow03Animating();
    private bool IsMagicAttackAnimating() => IsAttackLeftWandAnimating() || IsAttackRightWandAnimating();


    private bool IsAttackBow01Animating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackBow01;
    private bool IsAttackBow02Animating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackBow02;
    private bool IsAttackBow03Animating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackBow03;
    private bool IsAttackRight0Animating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackRight00;
    private bool IsAttackLeft0Animating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackLeft00;
    private bool IsAttackLeft1Animating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackLeft01;
    private bool IsAttackLeft2Animating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackLeft02;
    private bool IsAttackLeftWandAnimating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackLeftWand;
    private bool IsAttackRightWandAnimating() => _animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttackRightWand;
    #endregion
}

