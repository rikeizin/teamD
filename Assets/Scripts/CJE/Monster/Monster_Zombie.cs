using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_Zombie : MonsterController
{
    // 2022.05.18 수정
    protected readonly int hashAttack = Animator.StringToHash("Zombie_Attack");

    protected override void OnAwake()
    {        
        base.OnAwake();
        m_status = new Status(100f, 5f, 0.7f, 1f, 50f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
    }

    public override void Hit()
    {
        base.Hit();
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
    }

    public void AttackZombie()
    {
        if ((m_anim.GetCurrentAnimatorStateInfo(0).shortNameHash == hashAttack)
            && (isAttackCool == false))
        {
            m_player.GetComponent<PlayerController>().TakeDamage(m_status.m_attack);
            StartCoroutine(IsAttackCoolTime());
        }
    }
}
