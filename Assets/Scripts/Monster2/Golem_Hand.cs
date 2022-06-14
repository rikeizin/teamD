using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Hand : MonoBehaviour
{
    private Golem _golem = null;

    private void Awake()
    {
        _golem = gameObject.GetComponentInParent<Golem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _golem.AttackHand();
    }
}
