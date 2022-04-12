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
            if (instance == null) // ���� ó�� ������� �ν��Ͻ��̴�.
            {
                instance = this;

                instance.Initalize();
                DontDestroyOnLoad(this.gameObject); // �ٸ� ���� �ε�Ǵ��� �������� �ʴ´�.
            }
            else
            {
                // �̹� �ν��Ͻ��� ��������� �ִ�.
                if (instance != this)    // �̹� ������� ���� ���� �ٸ���.
                {
                    Destroy(this.gameObject);   //���� �״´�.
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

            // ���� �������� Ȯ�� ����
            RandPercent percentage = new RandPercent();
            percentage.regist.Add("Stage_Dungeon", 70);
            percentage.regist.Add("Stage_EliteBossRoom", 30);

            int TotalStageCount = (TotalFloor * 2) - 1;
            for (int i = 0; i < TotalStageCount; i++)
            {
                if(i == 0) 
                {
                    // ù STAGE�� ���� ����
                    stageList.Add(new Stage(i, "Stage_Inn"));
                    CurrentStageIndex = 0;
                } 
                else if(i == TotalStageCount -1)
                {
                    // ������ STAGE�� ������������ ����
                    stageList.Add(new Stage(i, "Stage_LastBossRoom"));
                }
                else if(i%2 == 1)
                {
                    // STAGE ������ ¦�� �ΰ�� Ÿ�� �ܰ�
                    stageList.Add(new Stage(i, "Stage_OutOfTower"));
                }
                else
                {
                    // ȣ��� ���� �������� ���� ���� percentage.CallResult()
                    stageList.Add(new Stage(i, percentage.CallResult()));
                }
                Debug.Log($"{stageList[i].index} : {stageList[i].sceneName}");
            }
        }

        public void NextStage()
        {
            CurrentStageIndex++;

            // ������ �������� clear���� �̵��� �ʱ�ȭ
            if(CurrentStageIndex == stageList.Count)
            {
                Initalize();
            }

            string sceneName = stageList[CurrentStageIndex].sceneName;
            SceneManager.LoadScene(sceneName);
        }

    }
}