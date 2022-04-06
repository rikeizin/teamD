using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STAGE_MANAGEMENT
{
    public class Gate : MonoBehaviour
    {
        [Header ("출구 tranform")]
        public bool requestExit;
        private void Awake()
        {
            requestExit = false;
            //Debug.Log("Gate컴포넌트 추가");
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("나가겠습니까?");
            if (other.gameObject.CompareTag("Player"))
            {
                StageManager.Inst.NextStage();
            }
        }
    }
}

