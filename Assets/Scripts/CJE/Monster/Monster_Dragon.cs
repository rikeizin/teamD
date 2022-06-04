using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_Dragon : MonsterController
{
    // 2022.06.02 수정
    public Text m_monsterName;
    public Text m_monsterHp;
    public GameObject m_effectFlame;
    public GameObject m_effectScream;

    protected Collider m_collider;
    protected bool m_intro = true;
    protected bool m_flame = false;
    protected bool m_scream = false;

    #region Animation Event Methods

    protected void AnimEvent_IntroFlyUpFinish()
    {
        if (!isDead)
        {
            m_anim.SetBool("Intro_FlyUp", false);
            m_anim.SetBool("Intro_FlyMove", true);
        }
    }

    protected void AnimEvent_IntroMove()
    {
        if (!isDead)
        {
            transform.Translate(Vector3.forward * 1.0f);
        }
    }

    protected void AnimEvent_IntroMoveFinish()
    {
        if (!isDead)
        {
            m_anim.SetBool("Intro_FlyMove", false);
            m_anim.SetBool("Intro_FlyDown", true);
        }
    }

    protected void AnimEvent_IntroFlyDownFinish()
    {
        if (!isDead)
        {
            m_anim.SetBool("Intro_FlyDown", false);
            m_anim.SetBool("Intro_Scream", true);
        }
    }

    protected void AnimEvent_IntroScreamFinish()
    {
        if (!isDead)
        {
            m_anim.SetBool("Intro_Scream", false);
            m_collider.isTrigger = false;
            m_intro = false;
        }
    }
    protected void AnimEvent_AttackHandFinish()
    {
        if (!isDead)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack_Hand", false);
        }
    }
    protected void AnimEvent_AttackFlame()
    {
        if (!isDead)
        {
            m_effectFlame.SetActive(true);
        }
    }

    protected void AnimEvent_AttackFlameFinish()
    {
        if (!isDead)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack_Flame", false);
            m_effectFlame.SetActive(false);
            m_flame = true;
        }
    }

    protected void AnimEvent_AttackScream()
    {
        if (!isDead)
        {
            m_effectScream.SetActive(true);
        }
    }

    protected void AnimEvent_AttackScreamFinish()
    {
        if (!isDead)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack_Scream", false);
            m_effectScream.SetActive(false);
            m_scream = true;
        }
    }

    #endregion
    protected override void OnAwake()
    {
        base.OnAwake();
        m_status = new Status(300f, 25f, 3f, 20f, 300f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)
        m_collider = GetComponent<Collider>();
        m_effectFlame.SetActive(false);
        m_effectScream.SetActive(false);

        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
        m_monsterName.text = "DRAGON";
        m_monsterHp.text = m_status.m_hp + "/" + m_status.m_hpMax;   
    }


    public override void Idle()
    {
        if(m_intro == true)
        {
            m_intro = false;
            m_collider.isTrigger = true;
            m_anim.SetBool("Intro_FlyUp", true);
        }
        base.Idle();
    }

    public override void Hit()
    {
        base.Hit();
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
        m_monsterHp.text = m_status.m_hp + "/" + m_status.m_hpMax;
    }

    public override void Attack()
    {
        if(m_status.m_hp <= 200 && m_flame == false)
        {
            Attack_Flame();
        }

        if(m_status.m_hp <= 100 && m_scream == false)
        {
            Attack_Scream();
        }

        base.Attack();
        StartCoroutine(HandCoroutine());
    }

    IEnumerator HandCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        Attack_Hand();
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

    public void Attack_Scream()
    {
        m_anim.SetBool("Attack_Scream", true);
        ResetMove();
    }

}
