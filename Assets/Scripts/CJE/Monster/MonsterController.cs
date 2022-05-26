using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/* 
 * MonsterController 
 * 2022.05.26 수정
 */
public class MonsterController : MonoBehaviour
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
    protected virtual void AnimEvent_AttackFinish()
    {
        if (!isDead)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack", false);
        }
    }

    protected virtual void AnimEvent_HitFinish()
    {
        if (!isDead)
        {
            SetState(eMonsterState.Idle);
            m_anim.ResetTrigger("Hit");
        }
    }

    protected virtual void AnimEvent_DeadFinish()
    {
        Destroy(gameObject);
    }
    #endregion

    public Status m_status;
    public Slider m_Hpbar;

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
        m_navAgent = GetComponent<NavMeshAgent>();
        m_anim = GetComponent<Animator>();
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
            case eMonsterState.Dead:
                Dead();
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

    public virtual void Hit()
    {
        m_anim.SetTrigger("Hit");
        if (m_status.m_hp <= 0)
        {
            m_status.m_hp = 0f;
            SetState(eMonsterState.Dead);
        }
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
    }

    public virtual void Dead()
    {
        if (!isDead)
        {
            isDead = true;
            m_anim.SetTrigger("Dead");
        }
    }
}
