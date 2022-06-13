using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Slime_Head : MonoBehaviour
{
    private Monster_Slime _slime = null;

    private void Awake()
    {
        _slime = gameObject.GetComponentInParent<Monster_Slime>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _slime.AttackHead();
    }
}
