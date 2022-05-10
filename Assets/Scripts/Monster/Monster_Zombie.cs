using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_Zombie : MonsterController
{
    protected override void OnAwake()
    {        
        base.OnAwake();
        m_status = new Status(25, 10f, 3f, 1f, 50f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)
    }

    public override void Hit()
    {
        base.Hit();
        m_hpBar.value = (float)m_status.m_hp / (float)m_status.m_hpMax * 100;
    }
}
