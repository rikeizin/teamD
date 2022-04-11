using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace STAGE_MANAGEMENT
{
    public class PlayerSpawn : MonoBehaviour
    {
        //[Header("스폰시킬 플레이어")]
        private GameObject Player;

        private void OnEnable()
        {
            ReSpawn();
        }

        public void ReSpawn()
        {
            // 현재 Scene에 플레이어가 없는지 확인
            if (GameObject.FindGameObjectWithTag("Player") != null) 
            {
                Debug.Log("리스폰");
                Debug.Log(GameObject.FindGameObjectWithTag("Player").transform.position);
                Debug.Log(this.transform.position) ;
                // 플레이어 시작 지점 스폰
                //GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().Move(this.transform.position);
                GameObject.FindGameObjectWithTag("Player").transform.position = this.transform.position;
            }
        }
    }
}

