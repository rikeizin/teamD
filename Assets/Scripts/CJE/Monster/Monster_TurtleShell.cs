using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_TurtleShell : MonsterController
{
    // 2022.06.09 수정
    protected readonly int hashTurtleShell_Attack = Animator.StringToHash("TurtleShell_Attack");

    protected override void OnAwake()
    {
        base.OnAwake();
        m_status = new Status(40f, 5f, 1f, 1f, 50f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
    }

    public override void Hit()
    {
        base.Hit();
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
    }

    public void AttackHead()
    {
        if ((m_anim.GetCurrentAnimatorStateInfo(0).shortNameHash == hashTurtleShell_Attack)
            && (isAttackCool == false))
        {
            m_player.GetComponent<PlayerController>().TakeDamage(m_status.m_attack);
            StartCoroutine(IsAttackCoolTime());
        }
    }
}
