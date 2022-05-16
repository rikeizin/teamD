using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //public GameObject hitEffect = null;
    Queue<IBattle> hitTarget = new Queue<IBattle>(16);
    Player player = null;
    MonsterController mons = null;
    public struct P_WeaponsDamage
    {
        public float d_sword;
        public float d_bsword;
        public float d_wand;
        public float d_mace;
        public float d_arrow;
        public P_WeaponsDamage(float bsword, float sword, float wand, float mace, float arrow)
        {
            d_bsword = bsword = 5.0f ;
            d_sword = sword = 15.0f;
            d_wand = wand = 4.0f;
            d_mace = mace = 30.0f;
            d_arrow = arrow = 8.0f;
        }
    }
    public enum P_WaeponName
    {
        BasicSword = 0 ,
        Sword_Sample = 1,
        Mace_Sample = 2,
    }

    public P_WeaponsDamage PDamage;
    public  P_WaeponName PName;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        mons = other.GetComponent<MonsterController>();
        //Debug.Log($"target2 : {other.gameObject.name}");
        if (player.IsAttack && other.CompareTag("Enemy")
            && other.gameObject != player.gameObject)
        {
            switch (PName)
            {
                case P_WaeponName.BasicSword:
                    mons.m_status.m_hp -= player.attackPower + PDamage.d_bsword;
                    Debug.Log("�⺻������..!");
                    break;
                case P_WaeponName.Sword_Sample:
                    mons.m_status.m_hp -= player.attackPower + PDamage.d_sword;
                    Debug.Log("������..!");
                    break;
                case P_WaeponName.Mace_Sample:
                    mons.m_status.m_hp -= player.attackPower + PDamage.d_mace;
                    Debug.Log("��ġ��..!");
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
