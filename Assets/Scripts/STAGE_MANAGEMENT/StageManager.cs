using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace STAGE_MANAGEMENT
{
    public class Stage{
        public string sceneName;
        public int index;
        public Stage() { }
        public Stage(int _index, string _sceneName)
        {
            index = _index;
            sceneName = _sceneName;
        }
    }

    public class StageManager : MonoBehaviour
    {
        private static StageManager instance = null;
        public static StageManager Inst
        {
            get
            {
                return instance;
            }
        }

        private List<Stage> stageList;
        public int CurrentStageIndex;
        public int TotalFloor = 4;

        [SerializeField]
        private GameObject TestPlayer;

        private void Awake()
        {
            if (instance == null) // 제일 처음 만들어진 인스턴스이다.
            {
                instance = this;

                instance.Initalize();
                DontDestroyOnLoad(this.gameObject); // 다른 씬이 로드되더라도 삭제되지 않는다.
            }
            else
            {
                // 이미 인스턴스가 만들어진게 있다.
                if (instance != this)    // 이미 만들어진 것이 나와 다르다.
                {
                    Destroy(this.gameObject);   //나는 죽는다.
                }
            }
        }

        public void Initalize()
        {
            stageList = new List<Stage>();

            TestPlayer = Instantiate(TestPlayer);
            DontDestroyOnLoad(TestPlayer);

            PlayerSpawn spawn = FindObjectOfType<PlayerSpawn>();
            if(spawn != null)
            {
                spawn.ReSpawn();
            }

            // 랜덤 스테이지 확률 설정
            RandPercent percentage = new RandPercent();
            percentage.regist.Add("Stage_Dungeon", 70);
            percentage.regist.Add("Stage_EliteBossRoom", 30);

            int TotalStageCount = (TotalFloor * 2) - 1;
            for (int i = 0; i < TotalStageCount; i++)
            {
                if(i == 0) 
                {
                    // 첫 STAGE는 여관 고정
                    stageList.Add(new Stage(i, "Stage_Inn"));
                    CurrentStageIndex = 0;
                } 
                else if(i == TotalStageCount -1)
                {
                    // 마지막 STAGE는 최종보스던전 고정
                    stageList.Add(new Stage(i, "Stage_LastBossRoom"));
                }
                else if(i%2 == 1)
                {
                    // STAGE 순서가 짝수 인경우 타워 외각
                    stageList.Add(new Stage(i, "Stage_OutOfTower"));
                }
                else
                {
                    // 호출된 랜덤 스테이지 정보 삽입 percentage.CallResult()
                    stageList.Add(new Stage(i, percentage.CallResult()));
                }
                Debug.Log($"{stageList[i].index} : {stageList[i].sceneName}");
            }
        }

        public void NextStage()
        {
            CurrentStageIndex++;

            // 마지막 스테이지 clear이후 이동시 초기화
            if(CurrentStageIndex == stageList.Count)
            {
                Initalize();
            }

            string sceneName = stageList[CurrentStageIndex].sceneName;
            SceneManager.LoadScene(sceneName);
        }

    }
}