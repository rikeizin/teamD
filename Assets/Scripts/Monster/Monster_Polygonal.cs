using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_Polygonal : MonsterController
{
    public GameObject m_SpitPrefab;
    public Text m_MonsterName = null;
    public Text m_MonsterHp = null;
    private GameObject m_SpitPoint;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_status = new Status(100, 50f, 50f, 20f, 100f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)
        m_SpitPoint = GameObject.Find("SpitPoint").gameObject;

        m_MonsterName.GetComponent<Text>().text = "POLYGONAL";
        m_MonsterHp.GetComponent<Text>().text = (m_status.m_hp + "/" + m_status.m_hpMax);
    }

    #region Animation Event Methods
    protected void AnimEvent_AttackSpitFinish()
    {
        if (!isDead)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack_Spit", false);
            m_currentTime = 0.0f;
        }
    }

    protected void AnimEvent_AttackSpit()
    {
        if (!isDead)
        {
            Instantiate(m_SpitPrefab, m_SpitPoint.transform);
        }
    }
    #endregion

    protected float m_currentTime = 5.0f;
    protected float m_spitTime = 5.0f;
    
    public override void Attack()
    {
        m_currentTime += Time.deltaTime;
        if( m_currentTime > m_spitTime)
        {
            Attack_Spit();
        }
        else
        {
            base.Attack();
        }
    }

    public void Attack_Spit()
    {
        m_anim.SetBool("Attack_Spit", true);
        ResetMove();
    }
}
