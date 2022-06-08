using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices;

public class RotateToMouse : MonoBehaviour
{
    private Transform _cameraPivot;
    private Transform _mainCamera;

    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out Vector2 pos);

    [SerializeField]
    private float _rotCamXAxisSpeed = 3;         // 카메라 x축 회전속도
    [SerializeField]
    private float _rotCamYAxisSpeed = 3;         // 카메라 y축 회전속도

    [SerializeField]
    private float _limitMinX = -45;              // 카메라 x축 회전 범위(최소)
    [SerializeField]
    private float _limitMaxX = 45;               // 카메라 x축 회전 범위(최대)
    private float _eulerAngleX;
    private float _eulerAngleY;
    private float _camera_dist = 0f; //리그로부터 카메라까지의 거리
    private float _camera_width = -3.5f; //가로거리
    private float _camera_height = 1.5f; //세로거리
    private float _camera_fix = 1f; //레이케스트 후 리그쪽으로 올 거리
    private int _layerMask = 0;

    private bool isCursorLock = false;
    private Vector3 _dir = Vector3.zero;

    private void Awake()
    {
        CursorLock();
    }

    private void Start()
    {
        //카메라리그에서 카메라까지의 길이
        _camera_dist = Mathf.Sqrt(_camera_width * _camera_width + _camera_height * _camera_height);

        //카메라리그에서 카메라위치까지의 방향벡터
        _dir = new Vector3(0, _camera_height, _camera_width).normalized;

        _cameraPivot = gameObject.transform.Find("CameraZeroPivot");
        _mainCamera = _cameraPivot.GetChild(0);

        _layerMask = 1 << LayerMask.NameToLayer("Map");
    }

    public void UpdateRotate(float mouseX, float mouseY)
    {
        _eulerAngleY += mouseX * _rotCamYAxisSpeed;                   // 마우스 좌우이동으로 카메라 y축 회전
        _eulerAngleX -= mouseY * _rotCamXAxisSpeed;                   // 마우스 상하이동으로 카메라 x축 회전

        //카메라 x축 회전의 경우 회전 범위를 설정
        _eulerAngleX = ClampAngle(_eulerAngleX, _limitMinX, _limitMaxX);

        if (Player.IsGamePaused == false)
        {
            //캐릭터를 마우스가 바라보는 곳으로 회전시킴. 
            transform.rotation = Quaternion.Euler(0, _eulerAngleY, 0);   // X축도 회전시키면 캐릭터가 앞뒤로 시소처럼 움직이므로 Y축만 회전
                                                                        //카메라 XY축 회전
            _cameraPivot.rotation = Quaternion.Euler(_eulerAngleX, _eulerAngleY, 0); //카메라의 피벗이 캐릭터 피벗과 다르므로, 캐릭터와 같은 피벗으로 CameraZeroPivot(빈 오브젝트)을 만들어 주고 마우스 위치에 따라 XY축이 회전하게 한다.)
        }

        //레이캐스트할 벡터값
        Vector3 ray_target = transform.up * _camera_height + transform.forward * _camera_width;

        RaycastHit hitinfo;
        Physics.Raycast(_cameraPivot.transform.position, ray_target, out hitinfo, _camera_dist, _layerMask);

        if (hitinfo.point != Vector3.zero)//레이케스트 성공시
        {
            //point로 옮긴다.
            _mainCamera.transform.position = new Vector3(hitinfo.point.x, hitinfo.point.y+_camera_height, hitinfo.point.z);
            //카메라 보정
            _mainCamera.transform.Translate(_dir * -1 * _camera_fix);
            
        }
        else
        {
            //로컬좌표를 0으로 맞춘다. (카메라리그로 옮긴다.)
            _mainCamera.transform.localPosition = Vector3.zero;
            //카메라위치까지의 방향벡터 * 카메라 최대거리 로 옮긴다.
            _mainCamera.transform.Translate(new Vector3(0, _camera_height, _camera_width));

            //카메라 보정
            //_mainCamera.transform.Translate(dir * -1 * camera_fix);
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    public void CursorLock()
    {
        if(isCursorLock == false)
        {
            Cursor.visible = false;                                     // 마우스 커서를 보이지 않게 한다.
            //Cursor.lockState = CursorLockMode.Locked;                   // 마우스 커서를 현재 위치에 고정 시킨다.
            isCursorLock = true;
        }
        else
        {
            Cursor.visible = true;                                     // 마우스 커서를 보이지 않게 한다.
            Cursor.lockState = CursorLockMode.None;                   // 마우스 커서를 현재 위치에 고정 시킨다.
            isCursorLock = false;
        }
        
    }
}
