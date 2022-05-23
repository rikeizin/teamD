using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_Dragon : MonsterController
{
    // 2022.05.18 ¼öÁ¤
    public Text m_monsterName;
    public Text m_monsterHp;
    private GameObject m_EffectFlame;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_status = new Status(300f, 50f, 50f, 20f, 300f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)
        m_EffectFlame = GameObject.Find("FlamePoint").gameObject;
        m_EffectFlame.SetActive(false);

        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
        m_monsterName.text = "DRAGON";
        m_monsterHp.text = m_status.m_hp + "/" + m_status.m_hpMax;   
    }

    #region Animation Event Methods
    protected void AnimEvent_AttackHandFinish()
    {
        if (!isDead)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack_Hand", false);
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
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack_FlyDown", false);
            m_anim.SetBool("OnFly", false);
            m_FlySwitch = false;
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

    protected float m_FlyCurrentTime = 0.0f;
    protected float m_FlyTime = 10.0f;
    protected float m_FlyFlameTime = 5.0f;

    protected bool m_Flame = false;
    protected bool m_Fly = false;
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
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
        m_monsterHp.text = m_status.m_hp + "/" + m_status.m_hpMax;
    }

    public override void Attack()
    {
        if (m_status.m_hp < 100 && m_Fly == false)
        {
            m_Fly = true;
            m_FlySwitch = true;
        }
        else if ( m_status.m_hp < 50)
        {
            m_Flame = false;
        }

        if ( m_FlySwitch == true )
        {
            m_anim.SetBool("OnFly",true);
            FlyAttack();
        }
        else if (m_status.m_hp < 200 && m_Flame == false)
        {
            Attack_Flame();
            if( m_status.m_hp > 50)
            {
                m_Flame = true;
            }
        }
        else
        {
            base.Attack();
            StartCoroutine(HandCoroutine());
        }
    }

    IEnumerator HandCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        Attack_Hand();
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
        m_FlyCurrentTime += Time.deltaTime;

        if( m_FlyCurrentTime > m_FlyTime)
        {
            Attack_FlyDown();
            ResetMove();
        }
        else if( m_FlyCurrentTime > m_FlyFlameTime && m_FlyFlame == false)
        {
            m_FlyFlame = true;
            Attack_FlyFlame();
            ResetMove();
        }
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
