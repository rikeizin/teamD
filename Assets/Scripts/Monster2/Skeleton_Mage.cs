using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skeleton_Mage : MonsterController
{
    public GameObject m_magePoint;
    public GameObject m_mage;

    protected float magicSpeed = 5.0f;

    protected void AnimEvent_AttackStart()
    {
        GameObject magic = Instantiate(m_mage, m_magePoint.transform.position, m_magePoint.transform.rotation);
        magic.transform.LookAt(m_player.transform);
        magic.GetComponent<Rigidbody>().velocity = transform.forward * magicSpeed;
        Destroy(magic, 3f);
        m_mage.SetActive(true);
    }
    protected override void AnimEvent_AttackFinish()
    {        
        if(!isDead)
        {
            m_anim.SetBool("Attack", false);
        }
    }
    protected override void OnAwake()
    {    
        base.OnAwake();
        m_status = new Status(100.0f, 20.0f, 10.0f, 12.0f, 12.0f);
        m_magePoint = GameObject.Find("MagePoint").gameObject;
        
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;       
    }   
    
    public override void Hit()
    {
        base.Hit();
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
    }

    public override void Attack()
    {        
        ResetMove();
        FollowTarget();
        m_anim.SetBool("Attack", true);
        if(m_dir > m_status.m_attackRange)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack", false);
        }
    }

   

} 