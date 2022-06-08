using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : Player_Swap
{
    public enum Type { AttackSpeed, CriticalHit, Damage, Defence, Evasion, Gold, Health, Jump, Speed };
    public Type type;
    public int value;

    private Animator _animator = null;
    private Player _player = null;

    private void Awake()
    {
        _animator = GameObject.Find("Player").GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Player>();
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

            case Type.Speed:
                Speed();
                break;
        }
    }

    public void HP()
    {
        _player.hp = _player.maxHp;
        _player.maxHp += 10;
        Debug.Log("피가 늘어남");
    }
    public void AttackSpeed()
    {
        _animator.SetFloat("AttackSpeed", _animator.GetFloat("AttackSpeed")+0.05f);
        Debug.Log("공격속도가 늘어남");
    }
    public void Defence()
    {
        _player.defence += 1.0f;
        Debug.Log("방어력이 늘어남");
    }
    public void Damage()
    {
        _player.attackPower += 5.0f;
        Debug.Log("공격력이 늘어남");
    }
    public void CriticalHit()
    {
        _player.critical += 0.1f;
        Debug.Log("치명타가 늘어남");
        return;
    }
    public void Speed()
    {
        _player.speed += 0.1f;
        Debug.Log("이동속도가 늘어남");
    }
    public void Jump()
    {
        _player.jump += + 0.1f;
        Debug.Log("점프력이 늘어남");
    }
    public void Gold()
    {
        _player.goldUp += 1;
        Debug.Log("골드획득량이 늘어남");
        return;
    }
    public void Evasion()
    {
        _player.evasion += 1f;
        Debug.Log("회피율이 늘어남");
        return;
    }
}
