using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* MonsterController
 * by 최지은
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
    protected GameObject m_player;
    protected NavMeshAgent m_navAgent;
    protected Animator m_anim;
    public float m_dir;
    public Status m_status;
    public bool isDead;

    #region Animation Event Methods
    protected virtual void AnimEvent_AttackFinish()
    {
        if(!isDead)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack", false);
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

    public virtual void Idle()
    {   
        ResetMove();
        // 인식 범위 안쪽일 때 Move
        if (m_dir < m_status.m_trackingRange)
        {
            SetState(eMonsterState.Move);
        }
    }

    public virtual void Move()
    {   
        ResetMove();
        // 플레이어 쫓아가기
        m_navAgent.SetDestination(m_player.transform.position);
        m_anim.SetBool("Move", true);

        // 공격사거리 안쪽일 때 Attack 
        if (m_dir < m_status.m_attackRange)
        {
            SetState(eMonsterState.Attack);
            m_anim.SetBool("Move", false);
        }// 인식 범위 바깥일 때 Idle
        else if(m_dir> m_status.m_trackingRange)
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
        // 공격사거리 바깥일 때 Move
        if (m_dir > m_status.m_attackRange)
        {
            SetState(eMonsterState.Move);
            m_anim.SetBool("Attack", false);
        }
    }

    public virtual void Hit()
    {
        m_anim.SetBool("Hit", true);
    }

    public virtual void Dead()
    {
        if (!isDead)
        {
            m_anim.SetTrigger("doDead");
            isDead = true;
        }
    }
                                   
    public float SearchTarget()
    {
        var direction = m_player.transform.position - this.transform.position;
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
        m_navAgent.isStopped = true;
        m_navAgent.ResetPath();
    }

    private void Update()
    {
        m_dir = SearchTarget();
        BehaviourProcess(); 
        //if(m_navAgent.velocity.sqrMagnitude >= 0.1f * 0.1f && m_navAgent.remainingDistance <= 0.1f)
        //{
        //    m_anim.SetBool("Walk", false);
        //}
        //else if(m_navAgent.desiredVelocity.sqrMagnitude >= 0.1f * 0.1f)
        //{
        //    Vector3 direction = m_navAgent.desiredVelocity;
        //    Quaternion targetAngle = Quaternion.LookRotation(direction);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, Time.deltaTime * 8.0f);
        //}
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
        m_navAgent = GetComponent<NavMeshAgent>();
        m_anim = GetComponent<Animator>();
        m_player = GameObject.FindGameObjectWithTag("Player");
        //m_navAgent.updateRotation = false;
    }

    private void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {        
        Initialize();        
    }

    // ATTACK일때 플레이어를 제자리회전으로 보게해주는 함수
    void FollowTarget()
    {
        if (m_dir < m_status.m_trackingRange)
        {
            Vector3 dir = m_player.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 1);
        }
    }
}
