using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace STAGE_MANAGEMENT
{
    public class PlayerSetStartPostion : MonoBehaviour
    {
        //[Header("스폰시킬 플레이어")]
        private GameObject player;
        
        private void OnEnable()
        {
            Respawn();
        }

        public void Respawn()
        {
            // 현재 Scene에 플레이어가 없는지 확인
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                // 플레이어 시작 지점 스폰
                
                player.transform.position = this.transform.position;
                //player.GetComponent<CharacterController>().Move(this.transform.position);
                Debug.Log($"플레이어 : {player.transform.position} / 스폰 : {this.transform.position}");
            }
        }
    }
}

