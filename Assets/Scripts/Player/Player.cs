using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private MovementCharacterController movement = null;             // MovementCharacterController ��ũ��Ʈ�� moveTo �Լ��� ����ϱ� ���� movement��� �̸����� �޾ƿ´�.
    private RotateToMouse rotateToMouse = null;                      // ĳ���� �þ� ȸ�� ��ũ��Ʈ�� �޾ƿ´�. 
    private CharacterController characterController = null;          // ĳ���� ��Ʈ�ѷ��� �ݶ��̴��� ������ٵ� ������ ��������Ƿ� �ҷ��´�.
    private Animator animator = null;                                // �ִϸ��̼� �Ķ���� ������ ���� Animator�� �޾ƿ´�.


    private void Awake()
    {
        animator                     = GetComponent<Animator>();
        characterController          = GetComponent<CharacterController>();
        rotateToMouse                = GetComponent<RotateToMouse>();               //��ũ��Ʈ�� �޾ƿ´�.
        movement                     = GetComponent<MovementCharacterController>();      //��ũ��Ʈ�� �޾ƿ´�.
    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        IsGrounded();
    }

    private void UpdateRotate() // ĳ���� �þ� ȸ��
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    private void UpdateMove() // ĳ���� ������
    {
        movement.MoveTo(new Vector3(movement.xInput, 0, movement.zInput));
    }

    public void IsGrounded() // �� ��� �ִ���
    {
        if (characterController.isGrounded)
            animator.SetBool("isJumping", false);
        #region ĳ���� ��Ʈ�ѷ� isGrounded �Ⱦ���
        //Vector3 origin = characterCtrl.bounds.center;       // ray ������ġ
        //Vector3 direction = Vector3.down;                   // ray ����
        //Ray ray = new Ray(origin, direction);               // ���ο� ray �����

        //RaycastHit rayHit;                                  // hitinfo. �浹ü ������ �޾ƿ� ���� �����
        //float radius = 0.15f;                               // ray�� ���� �߻��� Sphere(��)�� ������. ĳ���� �ݶ��̴����� ���� �� �۰� �ߴ�.(ĳ���� : 0.2)
        //Physics.SphereCast(ray, radius, out rayHit);        // ray�� ����, radius ũ���� Sphere�� �߻��ϰ�, rayHit�� �浹ü ������ �����Ѵ�.

        //if (characterCtrl.velocity.y < 0)                   // ĳ���Ͱ� �Ʒ��� �������� ���� ��
        //{
        //    if (rayHit.collider != null)                    // ray�� ���� ����� ��� 
        //    {
        //        if (rayHit.distance < 0.8f)                 // ray�� �浹ü ������ �Ÿ��� 0.8���� ª�� ��� isJumping = false
        //            animator.SetBool("isJumping", false);
        //    }
        //}
        #endregion
    }

}