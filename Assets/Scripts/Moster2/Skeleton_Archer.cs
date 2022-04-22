using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Archer : MonsterController
{
    protected override void OnAwake()
    {
        base.OnAwake();
        m_status = new Status(100, 30.0f,30.0f, 10.0f, 50.0f);//(int hp, float attack, float attackRange, float hitRange, float trackingRange
    }
}

