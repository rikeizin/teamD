using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit : MonoBehaviour
{
    private GameObject m_player;
    private Rigidbody m_rigid;

    private void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        m_rigid.AddForce(m_player.transform.position);
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }


}
