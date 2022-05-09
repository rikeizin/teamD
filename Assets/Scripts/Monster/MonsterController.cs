using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    protected Slider m_hpBar;
    public float m_ciriDamage;
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
        if (m_dir < m_status.m_trackingRange && SearchTarget() == true)
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
        else if(m_dir > m_status.m_trackingRange && SearchTarget() == false)
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
        if( m_status.m_hp == 0)
        {
            SetState(eMonsterState.Dead);
        }
        m_anim.SetBool("Hit", true);
        //m_hpBar.value = (float)m_status.m_hp / (float)m_status.m_hpMax * 100;
    }

    public virtual void Dead()
    {
        if (!isDead)
        {
            m_anim.SetTrigger("doDead");
            isDead = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(gameObject.CompareTag("Weapons"))
        {
            SetState(eMonsterState.Hit);
            switch (collision.gameObject.name)
            {
                case "Sword":
                    m_status.m_hp -= 5;
                    break;
                case "Wand":
                    m_status.m_hp -= 5;
                    break;
                case "Mace":
                    m_status.m_hp -= 5;
                    break;
                case "Bow":
                    m_status.m_hp -= 5;
                    break;
                case "Arrow":
                    m_status.m_hp -= 5;
                    break;
            }
        }
    }

    // 플레이어와의 거리를 잴 때, Ray를 쏴서 방해물을 피해 감지하는 bool 함수
    public bool SearchTarget()
    {
        var direction = m_player.transform.position - this.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1f, direction.normalized, out hit, m_status.m_trackingRange))
        {
            if(hit.collider.CompareTag("Player"))
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

    // ATTACK일때 플레이어를 제자리회전으로 보게해주는 함수
    void FollowTarget()
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

    private void Update()
    {
        m_dir = (m_player.transform.position - this.transform.position).sqrMagnitude;
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
        m_navAgent = GetComponent<NavMeshAgent>();
        m_anim = GetComponent<Animator>();
        m_hpBar = GetComponent<Slider>();
        m_player = GameObject.FindGameObjectWithTag("Player");
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
