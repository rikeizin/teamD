using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RandomMap {
    
    public class MapManager : MonoBehaviour
    {
        private MapManager instance = null;
        public MapManager Inst
        {
            get
            {
                if(instance == null)
                {
                    instance = new MapManager();
                }
                return instance;
            }
        }
        private Area[,] coordinate; // 좌표
        private Area[] areaArray;
        private const int TOTAL_DIR_CNT = 4;

        [Header("섹션 초기화")]
        [SerializeField]
        private Area thisArea;
        [SerializeField]
        private int MaxWidth = 5;
        [SerializeField]
        private int MaxDepth = 5;
        [SerializeField]
        private int creatCnt = 5;
        [SerializeField]
        private GameObject section;
        [Range(1, 3)]
        public int setSectionSzie = 1;
        public static float sectionDefaultWidth = 4.0f;
        public static float sectionDefaultHeight = 5.0f;
        public static int sectionSize;

        private static float sectionWidth;
        public static float SectionWidth
        {
            get
            {
                return sectionWidth;
            }
        }
        private static float sectionHeight;
        public static float SectionHeight
        {
            get
            {
                return sectionHeight;
            }
        }

        [Header("던전 출입구 오브젝트")]
        public GameObject entranceObj;

        [Header("플레이어 스타트 포인트")]
        public Transform playerSpawnPosition;

        enum PurposeOfGate
        {
            startPoint,
            endPoint
        }
        private void Awake()
        {
            MapBoundSet();
            Initalize();
        }

        #region 맵 공간 및 네브메쉬 베이크 공간 할당
        private void MapBoundSet()
        {
            sectionSize = setSectionSzie;
            sectionWidth = sectionDefaultWidth * sectionSize;
            sectionHeight = sectionDefaultHeight * sectionSize;
            LocalNavMeshBuildBoundSet();
        }

        private void LocalNavMeshBuildBoundSet()
        {
            Vector3 m_size = new Vector3(
                                (sectionWidth * MaxWidth),
                                (sectionHeight),
                                (sectionWidth * MaxDepth)
                            );
            Vector3 center = transform.position + (m_size / 2);
            Vector3 boundMargin = new Vector3(5, 5, 5);
            GameObject.Find("LocalNavMeshBuilder").transform.position = center;
            GameObject.Find("LocalNavMeshBuilder").GetComponent<LocalNavMeshBuilder>().m_Size = m_size + boundMargin;
        }
        #endregion

        // 입/출구 오브젝트 생성
        private void CreateGate(Area area, Direction direction, PurposeOfGate purpose )
        {
            GameObject gate = Instantiate(entranceObj, this.transform);
            Vector3 postion = new Vector3(area.x, 0, area.z) * sectionWidth;
            postion.x += sectionWidth / 2;
            postion.z += sectionWidth / 2;
            gate.transform.position = postion;
            DirectionExt.createDirectinalSurface(direction, gate);

            
            if(purpose == PurposeOfGate.startPoint)
            {
                gate.GetComponent<DungeonGate>().enableSpawn();
            } else if(purpose == PurposeOfGate.endPoint)
            {
                gate.GetComponent<DungeonGate>().enableExit();
            }
        }

        // 선형 길구조의 맵생성
        public void Initalize() {
            // 맵 초기화
            coordinate = new Area[MaxWidth, MaxDepth];
            areaArray = new Area[creatCnt];
            //Debug.Log($"생성 반복회수 : {areaArray.Length}");

            for (int i = 0; i < areaArray.Length; i++)
            {
                //Debug.Log($"{i}번째 Section");
                
                if (i > 0)
                {
                    thisArea = new Area();
                    thisArea.x = areaArray[i - 1].x;
                    thisArea.z = areaArray[i - 1].z;
                    thisArea.passedDir = areaArray[i - 1].nextDir;
                    thisArea = TransDir(thisArea); // 다음 방향위치로 생성
                }

                thisArea.gameObject = GameObject.Instantiate(section, this.transform);
                thisArea.script = thisArea.gameObject.GetComponent<Section>();
                thisArea.gameObject.transform.position = new Vector3(thisArea.x, 0, thisArea.z) * sectionWidth;
                // 던전에 할당된 좌표에 신규생성된 class정보 참조
                coordinate[thisArea.x, thisArea.z] = thisArea;

                // 1. 최대허용 범위 및 인접한 영역에 대한 벽생성
                thisArea.blockedDir = createBlockedDir(thisArea);
                // 2. 다음 진출 방향들에 이전 방향 추가하여 차단
                thisArea.blockedDir = DirectionExt.AddDirection(thisArea.blockedDir, thisArea.passedDir);
                
                // 3. 막힌 지역외 다음 진출 방향 결정
                if (thisArea.blockedDir.Count < TOTAL_DIR_CNT)
                {
                    thisArea.nextDir = randomNextDir(thisArea.blockedDir);
                }
                // 4. 지나온 방향, 다음 진출 방향 제외 벽 재생성
                thisArea.blockedDir = ReBlockDir(thisArea.passedDir, thisArea.nextDir);

                // 5. 던전 입/출구 방향 오브젝트 작업
                if (i == 0)
                {
                    // 0번째 구역인경우 던전 시작 스폰지점 생성
                    thisArea.blockedDir = DirectionExt.AddDirection(thisArea.blockedDir, thisArea.passedDir);
                    CreateGate(thisArea, DirectionExt.GetOpposite(thisArea.nextDir), PurposeOfGate.startPoint );// 던전입구 생성
                }
                else if (thisArea.blockedDir.Count == TOTAL_DIR_CNT)
                {
                    // 3-2 방향이 막힌경우 마지막 구역으로 처리
                    thisArea.blockedDir = DirectionExt.AddDirection(thisArea.blockedDir, thisArea.nextDir);
                    CreateGate(thisArea, DirectionExt.GetOpposite(areaArray[i - 1].passedDir), PurposeOfGate.endPoint);// 던전출구 생성
                    break;
                }
                else if(i == areaArray.Length - 1)
                {
                    // 마지막 구역 생성
                    thisArea.blockedDir = DirectionExt.AddDirection(thisArea.blockedDir, thisArea.nextDir);
                    CreateGate(thisArea, DirectionExt.GetOpposite(thisArea.passedDir), PurposeOfGate.endPoint);// 던전출구 생성
                }

                thisArea.script.area = thisArea;    // 생성된 구역오브젝트에 현재 생성으로 정의된 구역class 참조
                thisArea.script.OnInit(); // 구역 생성 초기화
                areaArray[i] = thisArea;  // 해당 생성순서의 index에 현재 생성 정보 저장
            }
        }

        List<Direction> createBlockedDir(Area area)
        {
            if (area.z == (MaxDepth - 1))
            {
                area.blockedDir.Add(Direction.N);
            }
            if (area.x == (MaxWidth - 1))
            {
                area.blockedDir.Add(Direction.E);
            }
            if (area.z == 0)
            {
                area.blockedDir.Add(Direction.S);
            }
            if (area.x == 0)
            {
                area.blockedDir.Add(Direction.W);
            }


            if (area.z > 0 || area.z < (MaxDepth - 1) || area.x > 0 || area.x < (MaxWidth - 1))
            {
                area.blockedDir = OtherDirBlock(area);
            }
            
            return area.blockedDir;
        }

        List<Direction> OtherDirBlock(Area area)
        {
            if (area.z < (MaxDepth - 1))
            {
                if (coordinate[area.x, area.z + 1] != null)
                    area.blockedDir = DirectionExt.AddDirection(area.blockedDir, Direction.N);
            }
            if (area.x < (MaxWidth - 1))
            {
                if (coordinate[area.x + 1, area.z] != null)
                    area.blockedDir = DirectionExt.AddDirection(area.blockedDir, Direction.E);
            }
            if (area.z > 0)
            {
                if (coordinate[area.x, area.z - 1] != null)
                    area.blockedDir = DirectionExt.AddDirection(area.blockedDir, Direction.S);
            }
            if (area.x > 0)
            {
                if (coordinate[area.x - 1, area.z] != null)
                    area.blockedDir = DirectionExt.AddDirection(area.blockedDir, Direction.W);
            }
           

            return area.blockedDir;
        }

        List<Direction> ReBlockDir(Direction passedDir, Direction nextDir)
        {
            List<Direction> dirList = new List<Direction>();
            dirList.Add(Direction.N);
            dirList.Add(Direction.E);
            dirList.Add(Direction.S);
            dirList.Add(Direction.W);

            dirList.Remove(passedDir);
            dirList.Remove(nextDir);
            return dirList;
        }

        Direction randomNextDir(List<Direction> blockedDir)
        {
            //Debug.Log($"중복 회수 : {blockedDir.Count}");
            Direction nextDir;
            bool isClash;

            while (true)
            {
                nextDir = (Direction)Random.Range(1, TOTAL_DIR_CNT + 1);
                isClash = DirectionExt.CheckdDirList(blockedDir, nextDir);

                if (isClash == false)
                {
                    break;
                }
            }
            return nextDir;
        }

        private Area TransDir(Area area)
        {
            switch (area.passedDir)
            {
                case Direction.N:
                    area.z += 1;
                    break;
                case Direction.E:
                    area.x += 1;
                    break;
                case Direction.S:
                    area.z -= 1;
                    break;
                case Direction.W:
                    area.x -= 1;
                    break;
                default:
                    Debug.Log("방향셋팅중 오류");
                    break;
            }
            area.passedDir = DirectionExt.GetOpposite(area.passedDir);

            return area;
        }
    }
}