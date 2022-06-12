using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beholder_Head : MonoBehaviour
{
    private Beholder _mob = null;

    private void Awake()
    {
        _mob = gameObject.GetComponentInParent<Beholder>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _mob.AttackHead();
    }
}
