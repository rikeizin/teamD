using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpControll1 : MonoBehaviour
{
    public GameObject EnemycanvasGo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( collision.transform.CompareTag ( "플레이어공격태그이름넣기" ))
        {
            EnemycanvasGo.GetComponent<Enemy_hpbar>().currentHp -= 10f; //데미지 기입하기
        }
    }
}
