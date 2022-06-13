using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Zombie_Hand : MonoBehaviour
{
    private Monster_Zombie _mob = null;

    private void Awake()
    {
        _mob = gameObject.GetComponentInParent<Monster_Zombie>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _mob.AttackZombie();
    }
}
