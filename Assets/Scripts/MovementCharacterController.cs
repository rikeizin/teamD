using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CharacterController))]
public class MovementCharacterController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7;
    private Vector3 moveForce;

    [SerializeField]
    private float jumpForce = 10;
    [SerializeField]
    private float gravity = -20;

    private CharacterController characterController;
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!characterController.isGrounded)
        {
            moveForce.y += gravity * Time.deltaTime;
        }
        characterController.Move(moveForce * Time.deltaTime);
    }

    public void MoveTo(Vector3 direction)
    {
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);

        moveForce = new Vector3(direction.x * moveSpeed, moveForce.y, direction.z * moveSpeed);
    }
    public void Jump()
    {
        if (characterController.isGrounded)
        {
            moveForce.y = jumpForce;
        }
    }
}
