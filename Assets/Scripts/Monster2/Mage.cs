using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    private GameObject m_player;
    private Rigidbody m_rigid;

    public float speed = 5.0f;

    private void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        transform.LookAt(m_player.transform);
        m_rigid.velocity = transform.forward * speed;
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
