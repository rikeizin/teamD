using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitEffect : MonoBehaviour
{
    private Rigidbody m_rigid = null;
    private float m_currentTime = 0.0f;
    private float m_destoryTime = 5.0f;

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        m_currentTime += Time.deltaTime;
        m_rigid.AddForce (Vector3.forward * 100.0f * Time.deltaTime);

        if( m_currentTime > m_destoryTime)
        {
            Destroy(gameObject);
        }
    }
}
