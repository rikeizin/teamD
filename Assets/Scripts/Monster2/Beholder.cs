using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beholder : MonsterController
{
    
    protected override void OnAwake()
    {    
        base.OnAwake();

        m_status = new Status(100, 10.0f, 5.0f, 80.0f,120.0f);//(int hp, float attack, float attackRange, float hitRange, float trackingRange
        
    }
    

}

