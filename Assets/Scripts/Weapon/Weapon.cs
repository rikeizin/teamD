using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //public GameObject hitEffect = null;
    Queue<IBattle> hitTarget = new Queue<IBattle>(16);
    Player player = null;
    MonsterController mons = null;

    public float d_bsword = 5f;
    public float d_sword = 10f;
    public float d_mace = 30f;
    public float d_arrow = 10f;
    public float d_magic = 10;
    public float d_meteor = 40f;

    public enum P_WaeponName
    {
        BasicSword = 0,
        Sword_Sample = 1,
        Mace_Sample = 2,
        Arrow = 3,
        WandAttack = 4,
        Meteor2 = 5
    }

    public P_WaeponName PName;
    private void Awake()
    {
        player = FindObjectOfType<Player>();

    }

    private void OnTriggerEnter(Collider other)
    {
        mons = other.gameObject.GetComponent<MonsterController>();
        //Debug.Log($"target2 : {other.gameObject.name}");
        if (player.IsAttack && other.CompareTag("Enemy")
            && other.gameObject != player.gameObject)
        {
            switch (PName)
            {
                case P_WaeponName.BasicSword:
                    mons.m_status.m_hp -= player.attackPower + d_bsword;
                    Debug.Log("기본검으로..!");
                    break;
                case P_WaeponName.Sword_Sample:
                    mons.m_status.m_hp -= player.attackPower + d_sword;
                    Debug.Log("검으로..!");
                    break;
                case P_WaeponName.Mace_Sample:
                    mons.m_status.m_hp -= player.attackPower + d_mace;
                    Debug.Log("망치로..!");
                    break;
                case P_WaeponName.Arrow:
                    mons.m_status.m_hp -= player.attackPower + d_arrow;
                    Debug.Log("Arrow..!");
                    break;
                case P_WaeponName.WandAttack:
                    mons.m_status.m_hp -= player.attackPower + d_magic;
                    Debug.Log("WandAttack..!");
                    break;
                case P_WaeponName.Meteor2:
                    mons.m_status.m_hp -= player.attackPower + d_meteor;
                    Debug.Log("Meteor2..!");
                    break;

            }
            Debug.Log($"target : {other.gameObject.transform.name}");
            mons.Hit();
            IBattle battle = other.GetComponentInParent<IBattle>();
            if (battle != null)
            {
                Vector3 hitPoint = this.transform.position - this.transform.up * 0.8f;
                hitTarget.Enqueue(battle);
            }
        }
    }

    public IBattle GetHitTarget()
    {
        return hitTarget.Dequeue();
    }

    public int HitTargetCount()
    {
        return hitTarget.Count;
    }
}
