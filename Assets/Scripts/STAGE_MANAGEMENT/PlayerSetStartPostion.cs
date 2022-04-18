using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STAGE_MANAGEMENT
{
    public class PlayerSetStartPostion : MonoBehaviour
    {
        //[Header("스폰시킬 플레이어")]
        public Transform player = null;
        //float time = 0;
        CharacterController ctrl = null;

        private void OnEnable()
        {
            Respawn();
        }

        //private void Update()
        //{
        //    time += Time.deltaTime;

        //    if (time>3)
        //    {
        //        //player.position = thisVector;
        //        //Debug.Log(player.position);
        //        time = 0;
        //    }
        //}

        public void Respawn()
        {
            // 현재 Scene에 플레이어가 없는지 확인
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                player = GameObject.FindObjectOfType<Player>().transform;
                ctrl = GameObject.FindObjectOfType<Player>().GetComponent<CharacterController>();

                ctrl.enabled = false;
                ctrl.transform.position = this.transform.position;
                ctrl.enabled = true;

                // 플레이어 시작 지점 스폰
                //player = GameObject.FindGameObjectWithTag("Player");

                //player.position = this.transform.position;
                //player.GetComponent<CharacterController>().Move(this.transform.position);
                Debug.Log($"플레이어 : {player.position} / 스폰 : {this.transform.position}");
            }
        }
    }
}