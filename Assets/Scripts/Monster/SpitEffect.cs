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
        transform.forward = m_rigid.velocity;
        m_rigid.AddRelativeForce(Vector3.forward * 2.0f);

        if (m_currentTime > m_destoryTime)
        {
            Destroy(gameObject);
        }
    }
}
