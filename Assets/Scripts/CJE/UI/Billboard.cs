using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Billboard : MonoBehaviour
{
    public Transform m_cam3D;
    public Transform m_cam2D;

    private void Start()
    {
        m_cam3D = Camera.main.transform;
        m_cam2D = GameObject.Find("Camera").transform;
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "Stage_OutOfTower")
        {
            transform.LookAt(transform.position + m_cam2D.rotation * Vector3.forward, m_cam2D.rotation * Vector3.up);
        }
        else
        {
            transform.LookAt(transform.position + m_cam3D.rotation * Vector3.forward, m_cam3D.rotation * Vector3.up);
        }
    }
}
