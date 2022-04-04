using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Zombie : MonsterController
{
    protected override void OnAwake()
    {        
        base.OnAwake();
        m_status = new Status(10, 10f, 1f, 1f, 10f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)
    }  
}
