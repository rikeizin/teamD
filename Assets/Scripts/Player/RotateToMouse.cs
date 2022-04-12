using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    [SerializeField]
    private float rotCamXAxisSpeed = 3;         // ī�޶� x�� ȸ���ӵ�
    [SerializeField]
    private float rotCamYAxisSpeed = 3;         // ī�޶� y�� ȸ���ӵ�

    private float limitMinX = -20;              // ī�޶� x�� ȸ�� ����(�ּ�)
    private float limitMaxX = 20;               // ī�޶� x�� ȸ�� ����(�ִ�)
    private float eulerAngleX;
    private float eulerAngleY;

    private void Awake()
    {
        //Cursor.visible = true;                                    // ���콺 Ŀ���� ������ �ʰ� �Ѵ�.
        Cursor.lockState = CursorLockMode.Locked;                   // ���콺 Ŀ���� ���� ��ġ�� ���� ��Ų��.
    }

    public void UpdateRotate(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed;                   // ���콺 �¿��̵����� ī�޶� y�� ȸ��
        eulerAngleX -= mouseY * rotCamXAxisSpeed;                   // ���콺 �����̵����� ī�޶� x�� ȸ��

        //ī�޶� x�� ȸ���� ��� ȸ�� ������ ����
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        //ĳ���͸� ���콺�� �ٶ󺸴� ������ ȸ����Ŵ. 
        transform.rotation = Quaternion.Euler(0, eulerAngleY, 0);   // X�൵ ȸ����Ű�� ĳ���Ͱ� �յڷ� �ü�ó�� �����̹Ƿ� Y�ุ ȸ��

        //ī�޶� XY�� ȸ��
        transform.Find("CameraZeroPivot").rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0); //ī�޶��� �ǹ��� ĳ���� �ǹ��� �ٸ��Ƿ�, ĳ���Ϳ� ���� �ǹ����� CameraZeroPivot(�� ������Ʈ)�� ����� �ְ� ���콺 ��ġ�� ���� XY���� ȸ���ϰ� �Ѵ�.)
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
