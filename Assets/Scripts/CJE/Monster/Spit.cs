using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit : MonoBehaviour
{
    private GameObject m_player;
    private Rigidbody m_rigid;
    private Monster_Polygonal _polygonal;

    [HideInInspector]
    public float damage = 0;
    private float _speed = 5;
    private void Awake()
    {
        m_player = GameObject.Find("Player");
        m_rigid = GetComponent<Rigidbody>();
        _polygonal = FindObjectOfType<Monster_Polygonal>();
    }

    private void Start()
    {
        transform.LookAt(m_player.transform);
        m_rigid.velocity = transform.forward * _speed;
        Destroy(gameObject, 3.0f);
        damage = _polygonal.m_status.m_attack;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_player.GetComponent<PlayerController>().TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
