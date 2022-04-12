using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private MovementCharacterController movement = null;             // MovementCharacterController 스크립트의 moveTo 함수를 사용하기 위해 movement라는 이름으로 받아온다.
    private RotateToMouse rotateToMouse = null;                      // 캐릭터 시야 회전 스크립트를 받아온다. 
    private CharacterController characterController = null;          // 캐릭터 컨트롤러에 콜라이더와 리지드바디 정보가 담겨있으므로 불러온다.
    private Animator animator = null;                                // 애니메이션 파라미터 설정을 위해 Animator를 받아온다.


    private void Awake()
    {
        animator                     = GetComponent<Animator>();
        characterController          = GetComponent<CharacterController>();
        rotateToMouse                = GetComponent<RotateToMouse>();               //스크립트를 받아온다.
        movement                     = GetComponent<MovementCharacterController>();      //스크립트를 받아온다.
    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        IsGrounded();
    }

    private void UpdateRotate() // 캐릭터 시야 회전
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    private void UpdateMove() // 캐릭터 움직임
    {
        movement.MoveTo(new Vector3(movement.xInput, 0, movement.zInput));
    }

    public void IsGrounded() // 땅 밟고 있는지
    {
        if (characterController.isGrounded)
            animator.SetBool("isJumping", false);
        #region 캐릭터 컨트롤러 isGrounded 안쓸때
        //Vector3 origin = characterCtrl.bounds.center;       // ray 생성위치
        //Vector3 direction = Vector3.down;                   // ray 방향
        //Ray ray = new Ray(origin, direction);               // 새로운 ray 만들기

        //RaycastHit rayHit;                                  // hitinfo. 충돌체 정보를 받아올 변수 만들기
        //float radius = 0.15f;                               // ray를 따라서 발사할 Sphere(구)의 반지름. 캐릭터 콜라이더보다 조금 더 작게 했다.(캐릭터 : 0.2)
        //Physics.SphereCast(ray, radius, out rayHit);        // ray를 따라서, radius 크기의 Sphere를 발사하고, rayHit에 충돌체 정보를 전달한다.

        //if (characterCtrl.velocity.y < 0)                   // 캐릭터가 아래로 떨어지고 있을 때
        //{
        //    if (rayHit.collider != null)                    // ray에 뭔가 닿았을 경우 
        //    {
        //        if (rayHit.distance < 0.8f)                 // ray와 충돌체 사이의 거리가 0.8보다 짧을 경우 isJumping = false
        //            animator.SetBool("isJumping", false);
        //    }
        //}
        #endregion
    }

}