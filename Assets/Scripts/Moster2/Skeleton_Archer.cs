using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Archer : MonsterController
{
    public bool particle = false; //��ƼŬ ���� bool
    public ParticleSystem particleObject; //��ƼŬ�ý���
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

