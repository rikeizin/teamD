using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_warrior_sword : MonoBehaviour
{
    private Skeleton_warrior _mob = null;

    private void Awake()
    {
        _mob = gameObject.GetComponentInParent<Skeleton_warrior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _mob.AttackWarrior();
    }
}
