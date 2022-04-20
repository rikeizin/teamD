using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_hpbar : MonoBehaviour
{
    public Transform player;
    private Camera cameraToLookAt;

    // Use this for initialization
    void Start()
    {
        cameraToLookAt = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CameraToLookAt();
        Enemyhp();
    }

    private void CameraToLookAt()
    {
        Vector3 v = cameraToLookAt.transform.position - transform.position;
        v.x = v.z = 0;
        transform.LookAt(cameraToLookAt.transform.position - v);
    }

    private void Enemyhp()
    {
        transform.position = player.position;
    }
}

