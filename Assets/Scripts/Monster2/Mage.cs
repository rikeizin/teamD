using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    private GameObject m_player;
    private Rigidbody m_rigid;
    private Skeleton_Mage mage;

    [HideInInspector]
    public float damage = 0;
    public float speed = 5.0f;

    private void Awake()
    {
        m_player = GameObject.Find("Player");
        mage = FindObjectOfType<Skeleton_Mage>();
    }

    private void Start()
    {
        damage = mage.m_status.m_attack;
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
