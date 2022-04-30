using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomMap
{
    public class DungeonGate : MonoBehaviour
    {
        private Transform localPivot;
        private GameObject NoneReqiredObjs;

        private void Awake()
        {
            localPivot = transform.Find("LocalPivot");

            // 구역 사이즈가 1인경우 필요 없는 오브젝트 제거
            if (MapManager.sectionSize == 1)
            {
                localPivot.localPosition = new Vector3(0, 0, -3.5f);
                NoneReqiredObjs = localPivot.Find("NoneRequiredObject").gameObject;
                Destroy(NoneReqiredObjs);
            }

        }

        public void enableSpawn()
        {
            localPivot.Find("PlayerSpawnPosition").gameObject.SetActive(true);
        }

        public void enableExit()
        {
            localPivot.Find("Exit_Gate").gameObject.SetActive(true);
        }
    }
}

