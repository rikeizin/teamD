using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private GameObject m_player;
    private Rigidbody m_rigid;
    


    public float speed = 20.0f;

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
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_rigid.constraints = RigidbodyConstraints.FreezeAll;            
        }
    }


}
