using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_TurtleShell_Head : MonoBehaviour
{
    public Monster_TurtleShell _turtleShell = null;

    private void Awake()
    {
        _turtleShell = gameObject.GetComponentInParent<Monster_TurtleShell>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _turtleShell.AttackHead();
    }
}
