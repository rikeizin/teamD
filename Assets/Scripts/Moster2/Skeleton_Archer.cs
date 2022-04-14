using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Archer : MonsterController
{
    public bool particle = false; //파티클 제어 bool
    public ParticleSystem particleObject; //파티클시스템
    protected override void OnAwake()
    {
        base.OnAwake();
        m_status = new Status(100, 30.0f,30.0f, 10.0f, 50.0f);//(int hp, float attack, float attackRange, float hitRange, float trackingRange
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
            particle = true;
            particleObject.Play();
        }
    }

}

