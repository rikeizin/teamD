using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beholder : MonsterController
{
    protected readonly int hashAttack = Animator.StringToHash("Attack03");

    protected override void OnAwake()
    {    
        base.OnAwake();

        m_status = new Status(100.0f, 20.0f, 2.0f, 10.0f, 10.0f);
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;

    }

    public override void Hit()
    {
        base.Hit();
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
    }
    public void AttackHead()
    {
        if ((m_anim.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttack)
            && (isAttackCool == false))
        {
            m_player.GetComponent<PlayerController>().TakeDamage(m_status.m_attack);
            StartCoroutine(IsAttackCoolTime());
        }
    }
}

