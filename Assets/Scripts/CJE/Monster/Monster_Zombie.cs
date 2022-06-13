using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_Zombie : MonsterController
{
    // 2022.05.18 수정
    protected override void OnAwake()
    {        
        base.OnAwake();
<<<<<<< Updated upstream
        m_status = new Status(40f, 5f, 1f, 1f, 50f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)
=======
<<<<<<< HEAD
        m_status = new Status(100f, 5f, 0.7f, 1f, 50f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)
=======
        m_status = new Status(40f, 5f, 1f, 1f, 50f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)
>>>>>>> 91f5d3b0ebd7d2a7eed9307fbf9045051288bfdf
>>>>>>> Stashed changes
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
    }

    public override void Hit()
    {
        base.Hit();
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
    }
}
