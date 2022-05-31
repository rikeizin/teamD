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
        public float d_meteor;
        public P_WeaponsDamage(float bsword, float sword, float wand, float mace, float arrow,float meteor)
        {
            d_bsword = bsword = 5.0f ;
            d_sword = sword = 15.0f;
            d_wand = wand = 4.0f;
            d_mace = mace = 30.0f;
            d_arrow = arrow = 8.0f;
            d_meteor = meteor = 45.0f;
        }
    }
    public enum P_WaeponName
    {
        BasicSword = 0 ,
        Sword_Sample = 1,
        Mace_Sample = 2,
        Arrow = 3,
        WandAttack = 4,
        Meteor2 = 5 
    }

    public P_WeaponsDamage PDamage;
    public  P_WaeponName PName;
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
            Debug.Log("왜안돼?");
            switch (PName)
            {
                case P_WaeponName.BasicSword:
                    mons.m_status.m_hp -= player.attackPower + PDamage.d_bsword;
                    Debug.Log("기본검으로..!");
                    break;
                case P_WaeponName.Sword_Sample:
                    mons.m_status.m_hp -= player.attackPower + PDamage.d_sword;
                    Debug.Log("검으로..!");
                    break;
                case P_WaeponName.Mace_Sample:
                    mons.m_status.m_hp -= player.attackPower + PDamage.d_mace;
                    Debug.Log("망치로..!");
                    break;
                case P_WaeponName.Arrow:
                    mons.m_status.m_hp -= player.attackPower + PDamage.d_arrow;
                    Debug.Log("Arrow..!");
                    break;
                case P_WaeponName.WandAttack:
                    mons.m_status.m_hp -= player.attackPower + PDamage.d_wand;
                    Debug.Log("WandAttack..!");
                    break;
                case P_WaeponName.Meteor2:
                    mons.m_status.m_hp -= player.attackPower + PDamage.d_meteor;
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
