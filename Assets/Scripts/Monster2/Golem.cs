using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonsterController
{
    public GameObject m_rockPoint;
    //public GameObject m_rockPrefeb;
    public GameObject rock;
    public bool isAttack = false;
         

    public float rockSpeed = 1.0f;


    private Rigidbody rigid;
    protected override void OnAwake()
    {
        base.OnAwake();

        m_status = new Status(100, 10.0f, 50.0f, 1.0f, 100.0f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)

    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    //protected void FixedUpdate()
    //{
    //    if(isAttack)
    //    {
    //        ThrowRock();
    //    }
    //}
    //기본 장거리 발사체 공격
    //특정 범위에 들어왔을때 근접공격으로 변경   

    public override void Attack()
    {
        if (Vector3.Distance(this.transform.position, m_player.transform.position) <= 3.0f)
        {
            m_anim.SetBool("Attack", false);
            Attack2(); //근거리 공격          
           
        }
        else if (Vector3.Distance(this.transform.position, m_player.transform.position) > 3.0f)
        {
            m_anim.SetBool("Attack2", false);
            isAttack = true;
            base.Attack();  //던지는 공격             
        }
    }
    
    public void Attack2()
    {
        m_anim.SetBool("Attack2", true);
        ResetMove();
    }

    protected private void ThrowRock()
    {
        Instantiate(rock, m_rockPoint.transform.position, m_rockPoint.transform.rotation); 

        if (rockSpeed != 0)  //날라가는 부분인데 안날라간다!
        {
            if (!m_player)
            {
                rigid.velocity = transform.TransformDirection(Vector3.forward * rockSpeed);
                //rigid.velocity = transform.forward * rockSpeed;

                Vector3 targetPos = m_player.transform.position - this.transform.position;
                transform.rotation = Quaternion.LookRotation(targetPos);
            }


        }
    }
       

}


