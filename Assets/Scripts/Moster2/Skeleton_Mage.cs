using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Mage : MonsterController
{
    protected override void OnAwake()
    {
        base.OnAwake();
        m_status = new Status(100, 20.0f, 40.0f, 80.0f, 80.0f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange
    }
}