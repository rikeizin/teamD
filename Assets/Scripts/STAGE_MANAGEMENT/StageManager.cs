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
        public int currentStageIndex;
        public int TotalFloor = 4;

        RandPercent percentage = null;

        // BGM����
        [SerializeField]
        private BGMExeManager bgm = null;

        [SerializeField]
        private GameObject Player;

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
            SceneManager.sceneLoaded += OnSceneLoaded;
            bgm = BGMExeManager.Inst;
            PlayerSpawn();
            //bgm.InitAudioToCAM();


            // ���� �������� Ȯ�� ����
            percentage  = new RandPercent();
            percentage.regist.Add("Stage_Dungeon", 70);
            percentage.regist.Add("Stage_EliteBossRoom", 30);
            
            StageInit();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (currentStageIndex < 1)
            {
                bgm.ChangeSoundTrack(SoundTrack.Inn);
            }
            else if (currentStageIndex == 1) // && currentStageIndex < (stageList.Count - 1)
            {
                bgm.ChangeSoundTrack(SoundTrack.NormalStage);
            }
            else if (currentStageIndex == (stageList.Count - 1))
            {
                bgm.ChangeSoundTrack(SoundTrack.BossStage);
            }
        }

        private void StageInit()
        {
            stageList = new List<Stage>();

            int TotalStageCount = (TotalFloor * 2) - 1;
            for (int i = 0; i < TotalStageCount; i++)
            {
                if (i == 0)
                {
                    // ù STAGE�� ���� ����
                    stageList.Add(new Stage(i, "Stage_Inn"));
                    currentStageIndex = 0;
                }
                else if (i == TotalStageCount - 1)
                {
                    // ������ STAGE�� ������������ ����
                    stageList.Add(new Stage(i, "Stage_LastBossRoom"));
                }
                else if (i % 2 == 1)
                {
                    // STAGE ������ ¦�� �ΰ�� Ÿ�� �ܰ�
                    stageList.Add(new Stage(i, "Stage_OutOfTower"));
                }
                else
                {
                    // ȣ��� ���� �������� ���� ���� percentage.CallResult()
                    stageList.Add(new Stage(i, percentage.CallResult()));
                }
                //Debug.Log($"{stageList[i].index} : {stageList[i].sceneName}");
            }
        }

        public void PlayerSpawn()
        {
            if (GameObject.FindGameObjectWithTag("Player") == null)
            {
                Player = Instantiate(Player);
                DontDestroyOnLoad(Player);
            }

            PlayerSetStartPostion spawn = FindObjectOfType<PlayerSetStartPostion>();

            if (spawn != null)
            {
                spawn.Respawn();
            }
        }

        public void NextStage()
        {
            currentStageIndex++;

            // ������ �������� clear���� �̵��� �ʱ�ȭ
            if(currentStageIndex == stageList.Count)
            {
                stageList.Clear();
                StageInit();
            } 
            

            string sceneName = stageList[currentStageIndex].sceneName;
            Player.SetActive(false);
            SceneManager.LoadScene(sceneName);
            

            Player.SetActive(true);
        }

    }
}