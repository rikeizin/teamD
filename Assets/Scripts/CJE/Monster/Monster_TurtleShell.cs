using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_TurtleShell : MonsterController
{
    // 2022.05.18 수정
    protected override void OnAwake()
    {
        base.OnAwake();
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
        m_status = new Status(30f, 10f, 10f, 1f, 50f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)
    }

    public override void Hit()
    {
        base.Hit();
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
    }
}
