using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Billboard : MonoBehaviour
{
    public Transform m_cam;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Stage_OutOfTower")
        {
            m_cam = GameObject.Find("Camera").transform;
        }
        else
        {
            m_cam = GameObject.Find("Main Camera").transform;
        }
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "Stage_OutOfTower")
        {
            transform.LookAt(transform.position + m_cam.rotation * Vector3.forward, m_cam.rotation * Vector3.up);
        }
        else
        {
            transform.LookAt(transform.position + m_cam.rotation * Vector3.forward, m_cam.rotation * Vector3.up);
        }
    }
}
