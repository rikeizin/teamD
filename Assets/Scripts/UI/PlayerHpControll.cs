using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpControll : MonoBehaviour
{
    public GameObject PlayercanvasGo;
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
        if ( collision.transform.CompareTag ( "Enemy" ))
        {
            PlayercanvasGo.GetComponent<Player_hpbar>().currentHp -= 10f; //데미지 기입하기
        }
    }
}
    