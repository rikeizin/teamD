using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_Polygonal : MonsterController
{
    // 2022.05.20 수정
    public GameObject m_spit;
    public Transform m_spitPoint;
    public Text m_monsterName;
    public Text m_monsterHp;
    
    #region Animation Event Methods
    protected void AnimEvent_AttackSpitStart()
    {
        Instantiate(m_spit, m_spitPoint.position, m_spitPoint.rotation);
    }

    protected void AnimEvent_AttackSpitFinish()
    {
        if (!isDead)
        {
            m_anim.SetBool("Attack_Spit",false);
            SetState(eMonsterState.Idle);
        }
    }
    #endregion

    protected override void OnAwake()
    {
        base.OnAwake();
        m_status = new Status(100f, 50f, 50f, 20f, 100f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)

        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
        m_monsterName.text = "POLYGONAL";
        m_monsterHp.text = m_status.m_hp + "/" + m_status.m_hpMax;
    }

    public override void Hit()
    {
        base.Hit();
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
        m_monsterHp.text = m_status.m_hp + "/" + m_status.m_hpMax;
    }
    
    public override void Attack()
    {
        ResetMove();
        FollowTarget();
        m_anim.SetBool("Attack", true);
        StartCoroutine(SpitCouroutine());
        if (m_dir > m_status.m_attackRange)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack", false);
        }
    }


    IEnumerator SpitCouroutine()
    {
        yield return new WaitForSeconds(5.0f);
        if (m_anim.GetBool("Attack") == true)
        {
            Attack_Spit();
        }
    }

    public void Attack_Spit()
    {
        m_anim.SetBool("Attack", false);
        m_anim.SetBool("Attack_Spit",true);
        ResetMove();
    }
}
