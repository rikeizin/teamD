using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Dragon_Hand : MonoBehaviour
{
    private Monster_Dragon _dragon = null;

    private void Awake()
    {
        _dragon = gameObject.GetComponentInParent<Monster_Dragon>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _dragon.AttackHead();
    }
}
