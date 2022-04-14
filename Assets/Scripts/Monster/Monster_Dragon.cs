using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Dragon : MonsterController
{
    public GameObject m_EffectFlame;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_status = new Status(100, 50f, 50f, 20f, 300f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)
        m_EffectFlame = GameObject.Find("FlamePoint").gameObject;
        m_EffectFlame.SetActive(false);
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
            m_currentTime = 0.0f;
        }
    }

    protected void AnimEvent_FlyAttackFlameFinish()
    {
        if (!isDead)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Fly_Attack_Flame", false);
            m_EffectFlame.SetActive(false);
            m_currentTime = 0.0f;
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
    protected float m_spitTime = 15.0f;

    public override void Attack()
    {
        m_currentTime += Time.deltaTime;
        if (m_currentTime > m_spitTime)
        {
            Attack_Flame();
        }
        else
        {
            base.Attack();
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
}
