using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    [SerializeField]
    private float rotCamXAxisSpeed = 3; // 카메라 x축 회전속도
    [SerializeField]
    private float rotCamYAxisSpeed = 3; // 카메라 y축 회전속도

    private float limitMinX = -20; // 카메라 x축 회전 범위(최소)
    private float limitMaxX = 20;  // 카메라 x축 회전 범위(최대)
    private float eulerAngleX;
    private float eulerAngleY;

    public void UpdateRotate(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed; // 마우스 좌우이동으로 카메라 y축 회전
        eulerAngleX -= mouseY * rotCamXAxisSpeed; // 마우스 상하이동으로 카메라 x축 회전

        //카메라 x축 회전의 경우 회전 범위를 설정
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        //캐릭터를 마우스가 바라보는 곳으로 회전시킴. 
        transform.rotation = Quaternion.Euler(0, eulerAngleY, 0); //X축도 회전시키면 캐릭터가 앞뒤로 시소처럼 움직이므로 Y축만 회전

        //카메라 XY축 회전
        transform.Find("CameraZeroPivot").rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0); //카메라의 피벗이 캐릭터 피벗과 다르므로, 캐릭터와 같은 피벗으로 CameraZeroPivot(빈 오브젝트)을 만들어 주고 마우스 위치에 따라 XY축이 회전하게 한다.)
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) 
            angle += 360;
        if (angle > 360) 
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

}
