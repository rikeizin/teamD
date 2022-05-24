using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skeleton_warrior : MonsterController
{
    protected override void OnAwake()
    {
        base.OnAwake();

        m_status = new Status(100.0f, 10.0f, 1.0f, 5.0f, 5.0f);
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
    }

    public override void Hit()
    {
        base.Hit();
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
    }
}
