using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Rigidbody m_rigid;
    private GameObject m_rockPoint;
    private bool m_raise = true;
    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_rockPoint = GameObject.Find("RockPoint");
    }
    private void Update()
    {
        if(m_raise)
        {
            transform.position = m_rockPoint.transform.position;
        }
        StartCoroutine(RockCoroutine());
    }

    IEnumerator RockCoroutine()
    { 
        yield return new WaitForSeconds(0.45f);
        m_raise = false;
        m_rigid.velocity = Vector3.up * 1.0f;
        m_rigid.AddRelativeForce(Vector3.left * 5.0f);
        m_rigid.AddRelativeForce(Vector3.forward * 20.0f);

        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
