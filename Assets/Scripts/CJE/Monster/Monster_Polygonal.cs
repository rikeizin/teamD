using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_Polygonal : MonsterController
{
    // 2022.06.02 수정
    public GameObject m_spit;
    public Text m_monsterName;
    public Text m_monsterHp;
    public GameObject[] m_weapons;  // 무기 배열 선언
    public AudioClip m_audioSpit;
    private GameObject m_spitPoint;
   

    #region Animation Event Methods
    protected void AnimEvent_AttackSpitStart()
    {
        Instantiate(m_spit, m_spitPoint.transform.position, m_spitPoint.transform.rotation);
        m_audio.clip = m_audioSpit;
        m_audio.Play();
    }

    protected void AnimEvent_AttackSpitFinish()
    {
        if (!isDead)
        {
            m_anim.SetBool("Attack_Spit", false);
        }
    }

    protected override void AnimEvent_DeadFinish()
    {
        base.AnimEvent_DeadFinish();
        Instantiate(m_weapons[Random.Range(0, 4)], transform.position + Vector3.forward * -1.5f, transform.rotation);
    }
    #endregion

    protected override void OnAwake()
    {
        base.OnAwake();
        m_status = new Status(300f, 50f, 10f, 20f, 100f); //(int hp, float attack, float attackRange, float hitRange, float trackingRange)

        m_rune = Resources.LoadAll<GameObject>("Prefab/Weapons"); // 리소스폴더의 웨폰 을 불러옴

        m_spitPoint = GameObject.Find("SpitPoint").gameObject;
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
        m_monsterName.text = "POLYGONAL";
        m_monsterHp.text = m_status.m_hp + "/" + m_status.m_hpMax;
    }

    public override void Hit()
    {
        base.Hit();
        m_Hpbar.value = m_status.m_hp / m_status.m_hpMax * 100;
        m_monsterHp.text = m_status.m_hp + "/" + m_status.m_hpMax;
    }

    public override void Attack()
    {
        ResetMove();
        FollowTarget();
        m_anim.SetBool("Attack", true);
        StartCoroutine(SpitCouroutine());
        if (m_dir > m_status.m_attackRange)
        {
            SetState(eMonsterState.Idle);
            m_anim.SetBool("Attack", false);
        }
    }


    IEnumerator SpitCouroutine()
    {
        yield return new WaitForSeconds(5.0f);
        if (m_anim.GetBool("Attack") == true)
        {
            Attack_Spit();
        }
    }

    public void Attack_Spit()
    {
        m_anim.SetBool("Attack_Spit", true);
        ResetMove();
    }
}
