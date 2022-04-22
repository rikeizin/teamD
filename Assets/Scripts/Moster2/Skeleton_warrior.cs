using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_warrior : MonsterController
{
    protected override void OnAwake()
    {
        base.OnAwake();
        m_status = new Status(100, 10.0f, 5.0f, 3.0f, 70.0f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange
    }
}
