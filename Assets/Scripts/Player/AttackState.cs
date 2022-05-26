using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    Player player = null;

    public void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.IsAttack = true;
    }
 
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        player.IsAttack = false;
    }
}