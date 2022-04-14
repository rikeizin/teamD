using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Mage : MonsterController
{
    public bool particle = false; //��ƼŬ ���� bool
    public ParticleSystem particleObject; //��ƼŬ�ý���
    protected override void OnAwake()
    {
        base.OnAwake();
        m_status = new Status(100, 20.0f,  40.0f, 40.0f, 60.0f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange
    }

    private void Awake()
    {
        particle = false;
        particleObject.Stop();
    }

    void Update()
    {
        if (isDead)
        {
            if(particle== false)
            {
                particleObject.Play();            

            }
            
            particle = true;
        }    
    }
}

