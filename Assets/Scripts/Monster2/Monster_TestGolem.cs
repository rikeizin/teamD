using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_TestGolem : MonsterController
{
    private GameObject m_rockPoint;
    private GameObject m_rockObject;
    public GameObject m_rock;
    public Text m_MonsterName = null;
    public Text m_MonsterHp = null;

    #region Animation Event Methods
    protected void AnimEvent_AttackThrow()
    {
        GameObject rock = Instantiate(m_rock, m_rockObject.transform);
        rock.SetActive(true);
    }

    protected void AnimEvent_AttackThrowFinish()
    {
        if (!isDead)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack_Throw", false);
        }
    }
    #endregion

    protected override void OnAwake()
    {
        base.OnAwake();
        m_status = new Status(100, 10.0f, 50.0f, 1.0f, 100.0f);
        m_rockPoint = GameObject.Find("RockPoint").gameObject;
        m_rockObject = GameObject.Find("RockObject").gameObject;

        m_MonsterName.GetComponent<Text>().text = "GOLEM";
        m_MonsterHp.GetComponent<Text>().text = (m_status.m_hp + "/" + m_status.m_hpMax);
    }

    public override void Hit()
    {
        base.Hit();
        //m_hpBar.value = (float)m_status.m_hp / (float)m_status.m_hpMax * 100;
        m_MonsterHp.GetComponent<Text>().text = (m_status.m_hp + "/" + m_status.m_hpMax);
    }

    public override void Attack()
    {
        if (m_dir <= 3.0f)
        {
            base.Attack();
        }
        else if (m_dir > 3.0f)
        {
            Attack_Throw();
        }
    }

    public void Attack_Throw()
    {
        m_anim.SetBool("Attack_Throw", true);
        ResetMove();
    }
}
