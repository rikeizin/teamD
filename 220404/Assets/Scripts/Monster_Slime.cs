using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Slime : MonsterController
{
    protected override void OnAwake()
    {
        base.OnAwake();
        m_status = new Status(10, 10f, 50f, 1f, 500f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)
    }
}
