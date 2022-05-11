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

    public struct WeaponsDamage
    {
        public int m_sword;
        public int m_wand;
        public int m_mace;
        public int m_bow;
        public int m_arrow;
        public WeaponsDamage(int sword, int wand, int mace, int bow, int arrow)
        {
            m_sword = sword;
            m_wand = wand;
            m_mace = mace;
            m_bow = bow;
            m_arrow = arrow;
        }
    }

    [SerializeField]
    protected eMonsterState m_state;
    protected GameObject m_player;
    protected NavMeshAgent m_navAgent;
    protected Animator m_anim;
    protected Collider m_collider;
    public Slider m_hpBar;
    //public Text m_damage;
    public Status m_status;
    public WeaponsDamage m_weaponsDamage;
    public float m_ciriDamage;
    public float m_dir;
    public bool m_getHit = false;
    public bool isDead = false;

    #region Animation Event Methods
    protected virtual void AnimEvent_AttackFinish()
    {
        if(!isDead)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack", false);
        }
    }

    protected virtual void AnimEvent_HitFinish()
    {
        if (!isDead)
        {
            m_getHit = false;
            SetState(eMonsterState.Idle);
            m_anim.ResetTrigger("doHit");
        }
    }

    protected virtual void AnimEvent_DeadFinish()
    {
        Destroy(gameObject);
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
        if( m_status.m_hp <= 0)
        {
            SetState(eMonsterState.Dead);
        }
    }

    public virtual void Dead()
    {
        if (!isDead)
        {
            isDead = true;
            m_collider.enabled = false;
            m_anim.SetTrigger("doDead");
        }
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag("Weapons"))
        {
            SetState(eMonsterState.Hit);
            m_anim.ResetTrigger("doHit");
            switch (other.gameObject.name)
            {
                case "Sword_Sample":
                    m_status.m_hp -= m_weaponsDamage.m_sword;
                    m_anim.SetTrigger("doHit");
                    //m_damage.text = m_weaponsDamage.m_sword.ToString();
                    break;
                case "Wand_Sample":
                    m_status.m_hp -= m_weaponsDamage.m_wand;
                    m_anim.SetTrigger("doHit");
                    //m_damage.text = m_weaponsDamage.m_wand.ToString();
                    break;
                case "Mace_Sample":
                    m_status.m_hp -= m_weaponsDamage.m_mace;
                    m_anim.SetTrigger("doHit");
                    //m_damage.text = m_weaponsDamage.m_mace.ToString();
                    break;
                case "Bow_Sample":
                    m_status.m_hp -= m_weaponsDamage.m_bow;
                    m_anim.SetTrigger("doHit");
                    //m_damage.text = m_weaponsDamage.m_bow.ToString();
                    break;
                case "Arrow_Sample":
                    m_status.m_hp -= m_weaponsDamage.m_arrow;
                    m_anim.SetTrigger("doHit");
                    //m_damage.text = m_weaponsDamage.m_arrow.ToString();
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
        m_collider = GetComponent<Collider>();
        //m_damage = GetComponent<Text>();
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_weaponsDamage = new WeaponsDamage(5, 6, 7, 1, 2); // (int sword, int wand, int mace, int bow, int arrow)
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
