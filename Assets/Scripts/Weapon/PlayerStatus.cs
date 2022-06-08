using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class PlayerStatus : MonoBehaviour
//{
    /*
     hp,Maxhp,attackspeed,CriticalHit,Damage,Defence,Speed,Gold,Jump,Shield
    체력, 최대체력, 공격속도, 치명타, 공격력, 방어력, 이동속도, 골드,점프력,방어막
     */
    public struct Status
    {
        public int p_hp; // 현재 체력
        public int p_Maxhp; // 최대 체력
        public float p_defence; // 방어력

        public float p_damage; // 공격력
        public float p_attackSpeed; // 공격속도
        public float p_criticalhit; // 치명타
        public float p_speed; // 이동속도

        public float p_jump; // 점프력
        
        public float p_gold; // 골드획득(골드획득량)
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
   
//}
