using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonsterController
{
    private GameObject m_rockPoint;    
    public GameObject m_rock;

    #region Animation Event Methods
    protected void AnimEvent_AttackThrow()
    {
        Instantiate(m_rock, m_rockPoint.transform.position, m_rockPoint.transform.rotation);
        m_rock.SetActive(true);
    }

    protected void AnimEvent_AttackThrowFinish()
    {
        if (!isDead)
        {
            m_anim.SetBool("Attack_Throw", false);
        }
    }
    #endregion
    protected override void OnAwake()
    {
        base.OnAwake();
        //m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;

        m_status = new Status(100.0f, 10.0f, 10.0f, 15.0f, 15.0f); 
        m_rockPoint = GameObject.Find("RockPoint").gameObject;       

    }

    //public override void Hit()
    //{
    //    base.Hit();
    //    m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
    //}

    public override void Attack()
    {
        ResetMove();
        FollowTarget();
        if (m_dir <= 8.0f)
        {
            m_anim.SetBool("Attack_Throw", false);
            base.Attack(); //근거리 공격

        }
        else if (m_dir > 8.0f)
        {
            m_anim.SetBool("Attack", false);
            Attack_Throw();
        }
        if (m_dir > m_status.m_attackRange)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack", false);
            m_anim.SetBool("Attack_Throw", false);

        }
    }

    public void Attack_Throw()
    {
        m_anim.SetBool("Attack_Throw", true);
        ResetMove();
    }
}


