using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_Dragon : MonsterController
{
    public GameObject m_EffectFlame;
    public GameObject m_EffectLanding;
    public Text m_MonsterName = null;
    public Text m_MonsterHp = null;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_status = new Status(300, 50f, 50f, 20f, 300f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)
        m_EffectFlame = GameObject.Find("FlamePoint").gameObject;
        m_EffectLanding = GameObject.Find("LandingPoint").gameObject;
        m_EffectFlame.SetActive(false);
        m_EffectLanding.SetActive(false);

        m_hpBar.value = (float)m_status.m_hp / (float)m_status.m_hpMax * 100;
        m_MonsterName.GetComponent<Text>().text = "DRAGON";
        m_MonsterHp.GetComponent<Text>().text = (m_status.m_hp + "/" + m_status.m_hpMax);
    }

    #region Animation Event Methods
    protected void AnimEvent_AttackHandFinish()
    {
        if (!isDead)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack_Hand", false);
            m_currentTime = 0.0f;
        }
    }

    protected void AnimEvent_AttackFlameFinish()
    {
        if (!isDead)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack_Flame", false);
            m_EffectFlame.SetActive(false);
        }
    }

    protected void AnimEvent_FlyAttackFlameFinish()
    {
        if (!isDead)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack_FlyFlame", false);
            m_EffectFlame.SetActive(false);
        }
    }

    protected void AnimEvent_FlyAttackDownFinish()
    {
        if (!isDead)
        {
            m_FlySwitch = false;
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack_FlyDown", false);
            m_anim.SetBool("OnFly", false);
            m_EffectLanding.SetActive(false);
            StopCoroutine(FlyAttackCoroutine());
        }
    }

    protected void AnimEvent_AttackFlame()
    {
        if (!isDead)
        {
            m_EffectFlame.SetActive(true);
        }
    }
    #endregion

    protected float m_currentTime = 0.0f;
    protected float m_HandTime = 5.0f;

    protected bool m_Flame = false;
    protected bool m_Fly = false;
    protected bool m_FlyAttack = false;
    protected bool m_FlySwitch = false;
    protected bool m_FlyFlame = false;

    public override void Idle()
    {
        if (m_FlySwitch == true)
        {
            FlyIdle();
        }
        else
        {
            base.Idle();
        }
    }

    public override void Move()
    {
        if (m_FlySwitch == true)
        {
            FlyMove();
        }
        else
        {
            base.Move();
        }
    }

    public override void Hit()
    {
        base.Hit();
        m_hpBar.value = (float)m_status.m_hp / (float)m_status.m_hpMax * 100;
        m_MonsterHp.GetComponent<Text>().text = (m_status.m_hp + "/" + m_status.m_hpMax);
    }

    public override void Attack()
    {
        m_currentTime += Time.deltaTime;

        if (m_status.m_hp < 100 && m_Fly == false)
        {
            m_Fly = true;
            m_FlySwitch = true;
            m_FlyAttack = true;
        }
        else if ( m_status.m_hp < 50)
        {
            m_Flame = false;
        }

        if ( m_FlySwitch == true )
        {
            m_anim.SetBool("OnFly",true);
            if(m_FlyAttack == true)
            {
                FlyAttack();
            }
        }
        else if (m_status.m_hp < 200 && m_Flame == false)
        {
            Attack_Flame();
            if( m_status.m_hp > 50)
            {
                m_Flame = true;
            }
        }
        else if ( m_currentTime > m_HandTime )
        {
            Attack_Hand();
        }
        else
        {
            base.Attack();
        }
    }

    public override void BehaviourProcess()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            m_status.m_hp -= 50;
            m_hpBar.value = (float)m_status.m_hp / (float)m_status.m_hpMax * 100;
            m_MonsterHp.GetComponent<Text>().text = (m_status.m_hp + "/" + m_status.m_hpMax);
        }
        base.BehaviourProcess();
    }

    public void FlyIdle()
    {
        if (m_dir < m_status.m_attackRange)
        {
            SetState(eMonsterState.Attack);
        }
        else if (m_dir < m_status.m_trackingRange)
        {
            SetState(eMonsterState.Move);
        }
    }

    public void FlyMove()
    {
        m_navAgent.SetDestination(m_player.transform.position);
        m_anim.SetBool("Move_Fly", true);

        if (m_dir < m_status.m_attackRange)
        {
            ResetMove();
            SetState(eMonsterState.Attack);
            m_anim.SetBool("Move_Fly", false);
        }
    }

    public void FlyAttack()
    {
        m_FlyAttack = false;
        StartCoroutine(FlyAttackCoroutine());
    }

    IEnumerator FlyAttackCoroutine()
    {
        yield return new WaitForSeconds(7.0f);
        m_FlyFlame = true;
        Attack_FlyFlame();
        ResetMove();

        yield return new WaitForSeconds(5.0f);
        m_FlyFlame = false;
        m_EffectLanding.SetActive(true);

        yield return new WaitForSeconds(2.0f);
        Attack_FlyDown();
        ResetMove();
    }

    public void Attack_Hand()
    {
        m_anim.SetBool("Attack_Hand", true);
        ResetMove();
    }

    public void Attack_Flame()
    {
        m_anim.SetBool("Attack_Flame", true);
        ResetMove();
    }

    public void Attack_FlyFlame()
    {
        m_anim.SetBool("Attack_FlyFlame", true);
        ResetMove();
    }

    public void Attack_FlyDown()
    {
        m_anim.SetBool("Attack_FlyDown",true);
        ResetMove();
    }
}
