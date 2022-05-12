using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Mage : MonsterController
{
    public GameObject m_MageParticle;
    public ParticleSystem part;
    public ParticleCollisionEvent collisionEvent;

    protected override void OnAwake()
    {    
        base.OnAwake();

        m_status = new Status(100, 20.0f, 40.0f, 80.0f, 80.0f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange
        m_MageParticle = GameObject.Find("ParticlePoint_Mage").gameObject;
        m_MageParticle.SetActive(false);
    }
    private void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvent = GetComponent<ParticleCollisionEvent>();
    }

    protected override void AnimEvent_AttackFinish()
    {
              
        //particleObject.Stop();
        if(!isDead)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack", false);
            m_MageParticle.SetActive(false);
        }
    }


    protected void AnimEvent_AttackParticle()
    {
        if(!isDead)
        {

            m_MageParticle.SetActive(true);
           
        }
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("맞았다");
    }

} 