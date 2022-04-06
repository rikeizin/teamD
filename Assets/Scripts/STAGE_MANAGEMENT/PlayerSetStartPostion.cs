using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace STAGE_MANAGEMENT
{
    public class PlayerSetStartPostion : MonoBehaviour
    {
        //[Header("스폰시킬 플레이어")]
        private GameObject Player;
        
        private void OnEnable()
        {
            // 현재 Scene에 플레이어가 없는지 확인
            if (GameObject.FindGameObjectWithTag("Player") == null) 
            {
                // 플레이어 생성
                Instantiate(Player, this.transform);
            }
            else
            {
                // 플레이어 시작 지점 스폰
                GameObject.FindGameObjectWithTag("Player").transform.position = this.transform.position;
            }
        }
    }
}

