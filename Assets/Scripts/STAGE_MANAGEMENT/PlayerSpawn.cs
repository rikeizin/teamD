using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace STAGE_MANAGEMENT
{
    public class PlayerSpawn : MonoBehaviour
    {
        //[Header("������ų �÷��̾�")]
        private GameObject Player;

        private void OnEnable()
        {
            ReSpawn();
        }

        public void ReSpawn()
        {
            // ���� Scene�� �÷��̾ ������ Ȯ��
            if (GameObject.FindGameObjectWithTag("Player") != null) 
            {
                Debug.Log("������");
                Debug.Log(GameObject.FindGameObjectWithTag("Player").transform.position);
                Debug.Log(this.transform.position) ;
                // �÷��̾� ���� ���� ����
                //GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().Move(this.transform.position);
                GameObject.FindGameObjectWithTag("Player").transform.position = this.transform.position;
            }
        }
    }
}

