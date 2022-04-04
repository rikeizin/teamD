using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* MonsterController
 * by √÷¡ˆ¿∫
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
        public int m_hp;
        public int m_hpMax;
        public float m_attack;
        public float m_attackRange;
        public float m_hitRange;
        public float m_trackingRange;
        public Status(int hp, float attack, float attackRange, float hitRange, float trackingRange)
        {
            m_hp = m_hpMax = hp;
            m_attack = attack;
            m_attackRange = attackRange;
            m_hitRange = hitRange;
            m_trackingRange = trackingRange;
        }
    }

    [SerializeField]
    protected eMonsterState m_state;
    protected GameObject player;
    protected NavMeshAgent navAgent;
    protected Animator anim;
    public float dir;
    public Status m_status;
    public bool isDead;

    #region Animation Event Methods
    protected virtual void AnimEvent_AttackFinish()
    {
        if(!isDead)
        {
            SetState(eMonsterState.Idle);
            anim.SetBool("Attack", false);
        }
    }
    #endregion

    public void SetMonster()
    {
        gameObject.SetActive(true);
        SetState(eMonsterState.Idle);
        m_status.m_hp = m_status.m_hpMax;
    }

    public void SetState(eMonsterState state)
    {
        if (!isDead)
        {
            m_state = state;
        }
    }

    public void Idle()
    {
        dir = SearchTarget();
        
        if (dir < m_status.m_attackRange)
        {
            SetState(eMonsterState.Attack);
        }
        else if( dir < m_status.m_trackingRange)
        {
            SetState(eMonsterState.Move);
        }
    }

    public void Move()
    {
        dir = SearchTarget();
        navAgent.SetDestination(player.transform.position);
        anim.SetBool("Move", true);

        if (dir < m_status.m_attackRange)
        {
            ResetMove();
            SetState(eMonsterState.Attack);
            anim.SetBool("Move", false);
        }
    }

    public void Attack()
    {
        anim.SetBool("Attack", true);
        ResetMove();
    }

    public void Hit()
    {
        anim.SetBool("Hit", true);
    }

    public void Dead()
    {
        anim.SetBool("Dead", true);
        isDead = true;
    }
                                   
    public float SearchTarget()
    {
        var direction = player.transform.position - this.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1f, direction.normalized, out hit, m_status.m_trackingRange))
        {
            if(hit.collider.CompareTag("Player"))
            {
                return direction.sqrMagnitude;
            }
            else
            {
                return direction.sqrMagnitude;
            }
        }
        return direction.sqrMagnitude;
    }

    public void ResetMove()
    {
        navAgent.isStopped = true;
        navAgent.ResetPath();
    }

    private void Update()
    {
        BehaviourProcess();
    }

    public virtual void BehaviourProcess()
    {
        switch (m_state)
        {
            case eMonsterState.Idle :
                Idle();
                break;
            case eMonsterState.Move :
                Move();
                break;
            case eMonsterState.Attack :
                Attack();
                break;
            case eMonsterState.Hit :
                Hit();
                break;
            case eMonsterState.Dead :
                Dead();
                break;
        }
    }
  
    protected virtual void Initialize()
    {        
        SetMonster();
        isDead = false;
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {        
        Initialize();        
    }
}
