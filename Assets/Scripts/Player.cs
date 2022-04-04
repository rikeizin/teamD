using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float xInput; // 좌우 입력 받아오는 변수
    private float zInput; // 앞뒤 입력 받아오는 변수

    private MovementCharacterController movement = null; // MovementCharacterController 스크립트의 moveTo 함수를 사용하기 위해 movement라는 이름으로 받아온다.
    private RotateToMouse rotateToMouse = null; // 캐릭터 시야 회전 스크립트를 받아온다. 
    private Animator animator = null; // 애니메이션 파라미터 설정을 위해 Animator를 받아온다.
    private CharacterController characterCtrl = null; // 캐릭터 컨트롤러에 콜라이더와 리지드바디 정보가 담겨있으므로 불러온다.


    private void Awake()
    {
        Cursor.visible = true; // 마우스 커서를 보이지 않게 한다. 추후 false로 바꿔서 안보이게 할 예정
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 현재 위치에 고정 시킨다.

        rotateToMouse = GetComponent<RotateToMouse>();               //스크립트를 받아온다.
        movement = GetComponent<MovementCharacterController>();      //스크립트를 받아온다.
        animator = GetComponent<Animator>();                         //스크립트를 받아온다.
        characterCtrl = GetComponent<CharacterController>();         //스크립트를 받아온다.
    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        isGrounded();
        #region
        //if (rigid.velocity.y < -0.05f)
        //{
        //    if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, 10f, LayerMask.GetMask("Ground")))
        //    {
        //        Debug.DrawRay(transform.position, Vector3.down * 0.8f, Color.blue, 0.3f);
        //        if (hit.distance < 0.2f)
        //            anim.SetBool(isJumpingHash, false);
        //    }
        //}애니메이션 isJumping false로 돌려주는 조건문, drawRay
        #endregion
    }

    void UpdateRotate() // 캐릭터 시야 회전
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    void UpdateMove() // 캐릭터 움직임
    {
        movement.MoveTo(new Vector3(xInput, 0, zInput));
    }

    public void Jump(InputAction.CallbackContext context)   // 인풋 시스템 점프
    {
        if (context.started)
        {
            if (!animator.GetBool("isJumping"))             // isJumping이 false일 경우에만 작동
            {
                animator.SetBool("isJumping", true);        // 점프할 때 isJumping = true
                movement.Jump();                            // Jump
            }
        }
    }

    public void Move(InputAction.CallbackContext context)   // 인풋 시스템 무브
    {
        // 이동 입력값 받기
        Vector2 input = context.ReadValue<Vector2>();       // 입력값을 받아서 회전 정도랑 이동 정도를 받아옴
        xInput = input.x;
        zInput = input.y;
        
        // 애니메이션 설정
        if (context.started)
            animator.SetBool("isWalking", true);            // 움직일 때 isWalking = true
        if (context.canceled)
            animator.SetBool("isWalking", false);           // 멈췄을 때 isWalking = false
    }

    public void isGrounded()
    {
        Vector3 origin = characterCtrl.bounds.center;       // ray 생성위치
        Vector3 direction = Vector3.down;                   // ray 방향
        Ray ray = new Ray(origin, direction);               // 새로운 ray 만들기

        RaycastHit rayHit;                                  // hitinfo. 충돌체 정보를 받아올 변수 만들기
        float radius = 0.15f;                               // ray를 따라서 발사할 Sphere(구)의 반지름. 캐릭터 콜라이더보다 조금 더 작게 했다.(캐릭터 : 0.2)
        Physics.SphereCast(ray, radius, out rayHit);        // ray를 따라서, radius 크기의 Sphere를 발사하고, rayHit에 충돌체 정보를 전달한다.

        if (characterCtrl.velocity.y < 0)                   // 캐릭터가 아래로 떨어지고 있을 때
        {
            if (rayHit.collider != null)                    // ray에 뭔가 닿았을 경우 
            {
                if (rayHit.distance < 0.8f)                 // ray와 충돌체 사이의 거리가 0.8보다 짧을 경우 isJumping = false
                    animator.SetBool("isJumping", false);
            }
        }
    }
}
