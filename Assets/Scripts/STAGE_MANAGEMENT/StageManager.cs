using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace STAGE_MANAGEMENT
{
    public class Stage
    {
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
        [SerializeField]
        private int currentStageIndex;
        [SerializeField]
        private int TotalFloor = 4;
        [SerializeField]
        private int currentFloor = 0;
        public int CurrentFloor { get => currentFloor; } //StageManager.Inst.CurrentFloor

        RandPercent percentage = null;

        // BGM관리
        [SerializeField]
        private AudioManager bgm = null;

        public float playtime;
        public bool isBattle;
        public Text playtimeText;
        public Text stageText;
        public Text gold;
        public GameObject Player;

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
            SceneManager.sceneLoaded += OnSceneLoaded;
            bgm = AudioManager.Inst;
            PlayerSpawn();
            //bgm.InitAudioToCAM();

            // 랜덤 스테이지 확률 설정
            percentage = new RandPercent();
            percentage.regist.Add("Stage_Dungeon", 70);
            percentage.regist.Add("Stage_EliteBossRoom", 30);

            StageInit();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //if (GameObject.FindGameObjectWithTag("Player") != null)
            //{
            //    Transform player = GameObject.FindObjectOfType<Player>().transform;
            //    player.GetComponentInChildren<Camera>().enabled = true;
            //}

            if (currentStageIndex < 1)
            {
                bgm.PlaySoundTrack(SoundTrack.Inn);
            }
            else if (currentStageIndex == 1) // && currentStageIndex < (stageList.Count - 1)
            {
                bgm.PlaySoundTrack(SoundTrack.NormalStage);
            }
            else if (currentStageIndex == (stageList.Count - 1))
            {
                bgm.PlaySoundTrack(SoundTrack.BossStage);
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
                    // 첫 STAGE는 여관 고정
                    stageList.Add(new Stage(i, "Stage_Inn"));
                    currentStageIndex = 0;
                }
                else if (i == TotalStageCount - 1)
                {
                    // 마지막 STAGE는 최종보스던전 고정
                    stageList.Add(new Stage(i, "Stage_LastBossRoom"));
                }
                else if (i % 2 == 1)
                {
                    // STAGE 순서가 짝수 인경우 타워 외각
                    stageList.Add(new Stage(i, "Stage_OutOfTower"));
                }
                else
                {
                    // 호출된 랜덤 스테이지 정보 삽입 percentage.CallResult()
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
                int index = Player.name.IndexOf("(Clone)");
                if (index > 0)
                    Player.name = Player.name.Substring(0, index);
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

            // 마지막 스테이지 clear이후 이동시 초기화
            if (currentStageIndex == stageList.Count)
            {
                stageList.Clear();
                StageInit();
                currentFloor = 0;
            }
            else if (currentStageIndex % 2 == 1)
            {
                currentFloor++;
            }

            string sceneName = stageList[currentStageIndex].sceneName;
            Player.SetActive(false);
            SceneManager.LoadScene(sceneName);

            Player.SetActive(true);
        }

        public void StageStart()
        {
            isBattle = true;
        }


        private void Update()
        {
            if (isBattle)
                playtime += Time.deltaTime;

            gold.text = GameObject.FindObjectOfType<Player>().currentGold.ToString();
        }


        void LateUpdate()
        {
            stageText.text = " Stage " + CurrentFloor;
            int hour = (int)(playtime / 3600);
            int min = (int)((playtime - hour * 3600) / 60);
            int second = (int)(playtime % 60);
            playtimeText.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);
        }
    }
}