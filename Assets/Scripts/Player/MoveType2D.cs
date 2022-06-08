using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveType2D : MonoBehaviour
{
    private Transform[] _wayPoint = new Transform[25];

    public uint _wayNum = 0;
    public float moveSpeed2D = 4;
    private Vector3 _lookPoint = Vector3.zero;
    [HideInInspector]
    public Vector3 _moveForce = Vector3.zero;
    private float _moveX = 0;
    private float _moveZ = 0;
    private float _input = 0;
    private float direction = 0;

    PlayerController playerController = null;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        for (int n = 0; n < 25; n++)
        {
            _wayPoint[n] = GameObject.Find("WayPoint").transform.GetChild(n);
        }
    }

    private void OnDisable()
    {
        _wayNum = 0;
    }

    private void Update()
    {
        Move2DUpdate();
        playerController.MoveTo(_moveForce);
        playerController.Move(_moveForce);
        _moveForce.y += playerController.PLAYER_GRAVITY * Time.deltaTime;
    }
    public void Move2D(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<float>();

        if (context.started)
        {
            if (_input == 1)
            {
                //_wayNum++;
                direction = 1;
            }
            else if (_input == -1)
            {
                //_wayNum--;
                direction = -1;
            }
        }

        if (context.canceled)
        {
            if (direction == 1)
            {
                //_wayNum--;
                //_inputV = 0;
            }
            else if (direction == -1)
            {
                //_wayNum++;
                direction = -1;
            }
        }

        if (context.canceled)
        {
            _moveForce = new Vector3(0, _moveForce.y, 0);
        }
    }

    public void Move2DUpdate()
    {
        if (_input == 1 && _wayPoint[_wayNum] != null)
        {
            if (_wayNum != _wayPoint.Length - 1)
            {
                _moveX = _wayPoint[_wayNum].position.x - transform.position.x;
                _moveZ = _wayPoint[_wayNum].position.z - transform.position.z;
            }
            else
            {
                _moveX = _wayPoint[_wayPoint.Length - 1].position.x - transform.position.x;
                _moveZ = _wayPoint[_wayPoint.Length - 1].position.z - transform.position.z;
            }
            float moveY = _moveForce.y;
            _moveForce = new Vector3(_moveX, 0, _moveZ).normalized * moveSpeed2D;
            _moveForce.y = moveY;
            if (Mathf.Abs(_moveX) < 0.1f && Mathf.Abs(_moveZ) < 0.1f && _wayNum != 24)
            {
                _wayNum++;
            }
            LookPoint();

        }
        else if (_input == -1 && _wayPoint[_wayNum] != null)
        {
            if (_wayNum != 0)
            {
                _moveX = _wayPoint[_wayNum - 1].position.x - transform.position.x;
                _moveZ = _wayPoint[_wayNum - 1].position.z - transform.position.z;
            }
            else
            {
                _moveX = _wayPoint[0].position.x - transform.position.x;
                _moveZ = _wayPoint[0].position.z - transform.position.z;
            }
            float moveY = _moveForce.y;
            _moveForce = new Vector3(_moveX, 0, _moveZ).normalized * moveSpeed2D;
            _moveForce.y = moveY;
            if (Mathf.Abs(_moveX) < 0.1f && Mathf.Abs(_moveZ) < 0.1f && _wayNum != 0)
            {
                _wayNum--;
            }
            LookPoint();
        }
    }

    public void LookPoint()
    {
        if (direction == -1) // 뒤를 보고있는 상태에서 끝났으면 뒤쪽 목표 좌표 보기
        {
            if (_wayNum != 0)
                _lookPoint = new Vector3(_wayPoint[_wayNum - 1].position.x, transform.position.y, _wayPoint[_wayNum - 1].position.z);

            else
                _lookPoint = new Vector3(_wayPoint[24].position.x, transform.position.y, _wayPoint[24].position.z);
        }
        else // 그게 아니라면 앞쪽 목표 좌표 보기
        {
            if (_wayNum != _wayPoint.Length - 1)
                _lookPoint = new Vector3(_wayPoint[_wayNum].position.x, transform.position.y, _wayPoint[_wayNum].position.z);
            else
                _lookPoint = new Vector3(_wayPoint[0].position.x, transform.position.y, _wayPoint[0].position.z);

        }
        this.transform.LookAt(_lookPoint);
    }
}
