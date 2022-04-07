using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCharacterController : MonoBehaviour
{
    public float runSpeed = 8;             // 뛰는 속도
    public float walkSpeed = 4;            // 걷는 속도
    [HideInInspector]
    public float applySpeed = 0;           // 적용되는 속도
    [SerializeField]
    private float jumpForce = 10;           // 점프력
    [SerializeField]
    private float gravity = -20;            // 중력

    private Vector3 moveForce;

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        applySpeed = runSpeed;
    }

    void Update()
    {
        IsGravity();
        characterController.Move(moveForce * Time.deltaTime); // 실제 Move 함수
    }

    public void MoveTo(Vector3 direction)
    {
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);

        moveForce = new Vector3(direction.x * applySpeed, moveForce.y, direction.z * applySpeed);
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            moveForce.y = jumpForce;
        }
    }

    public void IsGravity()
    {
        if (!characterController.isGrounded)
        {
            moveForce.y += gravity * Time.deltaTime;
        }
    }
}
