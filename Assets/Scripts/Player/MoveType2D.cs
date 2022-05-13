using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveType2D : MonoBehaviour
{
    PlayerController playerController = null;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        playerController.characterController.enabled = false;
        playerController.navMeshAgent.enabled = true;
        playerController.navMeshCharacter.enabled = true;
    }

    private void OnDisable()
    {
        playerController.characterController.enabled = true;
        playerController.navMeshAgent.enabled = false;
        playerController.navMeshCharacter.enabled = false;
    }
}
