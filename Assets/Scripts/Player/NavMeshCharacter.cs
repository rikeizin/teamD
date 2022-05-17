using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class NavMeshCharacter : MonoBehaviour
{
    private NavMeshAgent _navAgent;
    [SerializeField]
    private GameObject _startingPoint;
    [SerializeField]
    private GameObject _goalPoint;
    private CharacterController _characterController = null;

    void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _characterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        _characterController.enabled = false;
    }

    public void Move2D(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();

        if (input == 1)
        {
            _navAgent?.SetDestination(_goalPoint.transform.position);
        }
        else if (input == -1)
        {
            _navAgent?.SetDestination(_startingPoint.transform.position);
        }
        else
            _navAgent?.SetDestination(this.transform.position);
    }
}
