using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : Player_Swap
{
    public enum Type { AttackSpeed, CriticalHit, Damage, Defence, Evasion, Gold, Health, Jump, Shield };
    public Type type;
    public int value;

    public Status p_status;
    private Player_Swap PS;
    private void Awake()
    {
        PS = GetComponent<Player_Swap>();
        
    }

    public void GainRunes()
    {
        switch (type)
        {
            case Type.Health:
                HP();
                break;

            case Type.AttackSpeed:
                AttackSpeed();
                break;

            case Type.Damage:
                Damage();
                break;

            case Type.CriticalHit:
                CriticalHit();
                break;

            case Type.Defence:
                Defence();
                break;

            case Type.Evasion:
                Evasion();
                break;

            case Type.Jump:
                Jump();
                break;

            case Type.Gold:
                Gold();
                break;

            case Type.Shield:
                Shield();
                break;
        }
    }

    public void HP()
    {
        p_status.p_hp = p_status.p_Maxhp;
        p_status.p_Maxhp = p_status.p_Maxhp + 10;
        Debug.Log("�ǰ� �þ");
    }
    public void AttackSpeed()
    {
        p_status.p_attackSpeed = p_status.p_attackSpeed + 20.0f;
        Debug.Log("���ݼӵ��� �þ");
    }
    public void Defence()
    {
        p_status.p_defence = p_status.p_defence + 20.0f;
        Debug.Log("������ �þ");
    }
    public void Damage()
    {
        p_status.p_damage = p_status.p_damage + 15.0f;
        Debug.Log("���ݷ��� �þ");
    }
    public void CriticalHit()
    {
        Debug.Log("ġ��Ÿ�� �þ");
        return;
    }
    public void Evasion()
    {
        p_status.p_speed = p_status.p_speed + 10.0f;
        Debug.Log("�̵��ӵ��� �þ");
    }
    public void Jump()
    {
        p_status.p_jump = p_status.p_jump + 5.0f;
        Debug.Log("�������� �þ");
    }
    public void Gold()
    {
        Debug.Log("���ȹ�淮�� �þ");
        return;
    }
    public void Shield()
    {
        Debug.Log("��ȸ�� ���尡 ����");
        return;
    }
}
