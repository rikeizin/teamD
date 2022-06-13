using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beholder : MonsterController
{
    
    protected override void OnAwake()
    {    
        base.OnAwake();

        m_status = new Status(40f, 20.0f, 3.0f, 10.0f, 10.0f);
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;

    }

    public override void Hit()
    {
        base.Hit();
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
    }
}

