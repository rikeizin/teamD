using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private GameObject m_player;
    private Rigidbody m_rigid;
    private Golem _golem;

    [HideInInspector]
    public float damage = 0;
    private float _rockSpeed = 20;

    private void Awake()
    {
        m_player = GameObject.Find("Player");
        m_rigid = GetComponent<Rigidbody>();
        _golem = GameObject.Find("Golem").GetComponent<Golem>();
    }

    private void Start()
    {
        transform.LookAt(m_player.transform);
        m_rigid.velocity = transform.forward * _rockSpeed;
        Destroy(this.gameObject, 2.0f);
        damage = _golem.m_status.m_attack;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_player.GetComponent<PlayerController>().TakeDamage(damage);
            //gameObject.SetActive(false);
        }
    }
}
