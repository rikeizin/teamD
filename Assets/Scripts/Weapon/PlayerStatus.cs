using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    /*
     hp,Maxhp,attackspeed,CriticalHit,Damage,Defence,Speed,Gold,Jump,Shield
    ü��, �ִ�ü��, ���ݼӵ�, ġ��Ÿ, ���ݷ�, ����, �̵��ӵ�, ���,������,��
     */
    public struct Status
    {
        public int p_hp; // ���� ü��
        public int p_Maxhp; // �ִ� ü��
        public float p_defence; // ����

        public float p_damage; // ���ݷ�
        public float p_attackSpeed; // ���ݼӵ�
        public float p_criticalhit; // ġ��Ÿ
        public float p_speed; // �̵��ӵ�

        public float p_jump; // ������
        
        public float p_gold; // ���ȹ��(���ȹ�淮)
        public Status(int hp, float defence, float damage, float attackspeed, float criticalhit, float speed, float jump, float gold)
        {
            p_hp = p_Maxhp = hp;
            p_defence = defence;
            p_damage = damage;
            p_attackSpeed = attackspeed;
            p_criticalhit = criticalhit;
            p_speed = speed;
            p_jump = jump;
            p_gold = gold;
        }
    }
   
}
