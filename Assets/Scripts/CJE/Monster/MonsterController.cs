using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/* 
 * MonsterController 
 * 2022.06.08 수정
 */
public class MonsterController : MonoBehaviour, IBattle
{
    public enum eMonsterState
    {
        Idle,
        Move,
        Attack,
        Hit,
        Dead
    };

    public struct Status
    {
        public float m_hp;
        public float m_hpMax;
        public float m_attack;
        public float m_attackRange;
        public float m_hitRange;
        public float m_trackingRange;
        public Status(float hp, float attack, float attackRange, float hitRange, float trackingRange)
        {
            m_hp = m_hpMax = hp;
            m_attack = attack;
            m_attackRange = attackRange;
            m_hitRange = hitRange;
            m_trackingRange = trackingRange;
        }
    }

    #region Animation Event Methods

    protected virtual void AnimEvent_Attack()
    {
        if (!isDead)
        {
            m_audio.clip = m_audioAttack;
            m_audio.Play();
        }
    }
    protected virtual void AnimEvent_AttackFinish()
    {
        m_anim.SetBool("Attack", false);
        if (!isDead)
        {
            SetState(eMonsterState.Idle);
        }
    }

    protected virtual void AnimEvent_HitFinish()
    {
        m_anim.ResetTrigger("Hit");
        if (!isDead)
        {
            SetState(eMonsterState.Idle);
        }
    }

    protected virtual void AnimEvent_Dead()
    {
        m_audio.clip = m_audioDead;
        m_audio.Play();
    }

    protected virtual void AnimEvent_DeadFinish()
    {
        Destroy(gameObject);
        DropItem();
    }
    #endregion

    public Status m_status;
    public Slider m_Hpbar;
    public GameObject[] m_rune;
    public GameObject m_gold;
    public AudioSource m_audio;
    public AudioClip m_audioAttack;
    public AudioClip m_audioDead;

    [SerializeField] protected eMonsterState m_state;
    protected GameObject m_player;
    protected NavMeshAgent m_navAgent;
    protected Animator m_anim;
    protected Vector3 m_dirPos;
    protected float m_dir;
    protected bool isDead;


    public void Awake()
    {
        OnAwake();
    }


    protected virtual void OnAwake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        SetMonster();
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_rune = Resources.LoadAll<GameObject>("Prefab/Rune");
        m_navAgent = GetComponent<NavMeshAgent>();
        m_anim = GetComponent<Animator>();
        m_audio = GetComponent<AudioSource>();
        gameObject.SetActive(true);
        isDead = false;
    }

    public void SetState(eMonsterState state)
    {
        if (!isDead)
        {
            m_state = state;
        }
    }

    public void SetMonster()
    {
        SetState(eMonsterState.Idle);
        m_status.m_hp = m_status.m_hpMax;
    }

    public void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate()
    {
        BehaviourProcess();
        Direction();
    }

    protected void Direction()
    {
        m_dirPos = m_player.transform.position - this.transform.position;
        m_dir = m_dirPos.magnitude;
    }

    protected virtual void BehaviourProcess()
    {
        switch (m_state)
        {
            case eMonsterState.Idle:
                Idle();
                break;
            case eMonsterState.Move:
                Move();
                break;
            case eMonsterState.Attack:
                Attack();
                break;
            case eMonsterState.Hit:
                Hit();
                break;
        }
    }

    public bool SearchTarget()
    {
        var direction = m_player.transform.position - this.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1f, direction.normalized, out hit, m_status.m_trackingRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public void FollowTarget()
    {
        if (m_dir < m_status.m_trackingRange)
        {
            Vector3 dir = m_player.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 1);
        }
    }

    public void ResetMove()
    {
        m_navAgent.isStopped = true;
        m_navAgent.ResetPath();
    }

    public virtual void Idle()
    {
        ResetMove();
        if (m_dir < m_status.m_trackingRange && SearchTarget() == true)
        {
            SetState(eMonsterState.Move);
        }
    }

    public virtual void Move()
    {
        ResetMove();
        m_navAgent.SetDestination(m_player.transform.position);
        m_anim.SetBool("Move", true);


        if (m_dir < m_status.m_attackRange)
        {
            SetState(eMonsterState.Attack);
            m_anim.SetBool("Move", false);
        }
        else if (m_dir > m_status.m_trackingRange)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Move", false);
        }
    }

    public virtual void Attack()
    {
        ResetMove();
        FollowTarget();
        m_anim.SetBool("Attack", true);
        if (m_dir > m_status.m_attackRange)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack", false);
        }
    }

    public void Attack(IBattle target)
    {

    }
    public virtual void Hit()
    {
        if (!isDead)
        {
            m_anim.SetTrigger("Hit");
        }
        if (m_status.m_hp <= 0)
        {
            m_status.m_hp = 0f;
            SetState(eMonsterState.Dead);
            isDead = true;
            m_anim.SetTrigger("Dead");
        }
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
    }
    public void TakeDamage(float damage)
    {

    }
    public virtual void DropItem()
    {
        int runePer = Random.Range(0, 4);
        if (runePer == 1)
        {
            Instantiate(m_rune[Random.Range(0, 9)], transform.position, transform.rotation);
        }
        Instantiate(m_gold, transform.position + Vector3.forward * 1.5f, transform.rotation);
    }

}
