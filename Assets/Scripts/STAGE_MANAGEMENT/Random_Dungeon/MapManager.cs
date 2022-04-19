using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RandomMap {
    
    public class MapManager : MonoBehaviour
    {
        #region �ɹ� ����
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
        private Area[,,] coordinate; // ��ǥ
        private Area[] areaArray;
        private const int TOTAL_DIR_CNT = 6;
        private Area thisArea;


        [Header("�� ��ü ��� ������ �Ҵ�")]
        [SerializeField]
        private int MaxWidth = 5;
        [SerializeField]
        private int MaxDepth = 5;
        [SerializeField]
        private int MaxHeight = 5;

        [Header("���� ����")]
        [SerializeField]
        private GameObject section;
        [Range(1, 3)]
        public int setSectionSzie = 1;

        [Header("���� ���� ����")]
        [SerializeField]
        private int creatCnt = 5;
        
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

        [Header("�� ���� ���� ��ġ")]
        [SerializeField]
        private int startX = 0;
        [SerializeField]
        private int startY = 0;
        [SerializeField]
        private int startZ = 0;

        [Header("���� ���Ա� ������Ʈ")]
        public GameObject entranceObj;

        enum PurposeOfGate
        {
            startPoint,
            endPoint
        }
        #endregion

        private void Awake()
        {
            MapBoundSet();
            LocalNavMeshBuildBoundSet();
            GenerateMap();
        }

        #region �� ���� �� �׺�޽� ����ũ ���� �Ҵ�
        private void MapBoundSet()
        {
            // ���ǻ����� ����
            sectionSize = setSectionSzie;
            sectionWidth = sectionDefaultWidth * sectionSize;
            sectionHeight = sectionDefaultHeight * sectionSize;

            // �� �ʱ�ȭ
            coordinate = new Area[MaxWidth, MaxHeight, MaxDepth];
            // ����Section�� ���� �Ҵ�� Section���� �ʰ��� ��
            if (creatCnt > MaxWidth * MaxHeight * MaxDepth)
            {
                creatCnt = MaxWidth * MaxHeight * MaxDepth;
            }
            if (startX > MaxWidth - 1)
            {
                startX = MaxWidth - 1;
            }
            if (startY > MaxHeight - 1)
            {
                startY = MaxHeight - 1;
            }
            if (startZ > MaxDepth - 1)
            {
                startZ = MaxDepth - 1;
            }
            areaArray = new Area[creatCnt];
        }

        private void LocalNavMeshBuildBoundSet()
        {
            Vector3 m_size = new Vector3(
                                (sectionWidth * MaxWidth),
                                (sectionHeight * MaxHeight),
                                (sectionWidth * MaxDepth)
                            );
            Vector3 center = transform.position + (m_size / 2);
            Vector3 boundMargin = new Vector3(5, 5, 5);
            GameObject.Find("LocalNavMeshBuilder").transform.position = center;
            GameObject.Find("LocalNavMeshBuilder").GetComponent<LocalNavMeshBuilder>().m_Size = m_size + boundMargin;
        }
        #endregion

        // ���� �汸���� �ʻ���
        public void GenerateMap()
        {
            for (int i = 0; i < areaArray.Length; i++)
            {
                thisArea = new Area();
                thisArea.Index = i;
                if (i > 0)
                {
                    thisArea.x = areaArray[i - 1].x;
                    thisArea.y = areaArray[i - 1].y;
                    thisArea.z = areaArray[i - 1].z;
                    thisArea.passedDir = areaArray[i - 1].nextDir;
                    thisArea = TransDir(thisArea); // ���� ������ġ�� ����
                }
                else if (i == 0)
                {
                    thisArea.x = startX;
                    thisArea.y = startY;
                    thisArea.z = startZ;
                }

                thisArea.gameObject = GameObject.Instantiate(section, this.transform);
                thisArea.gameObject.name += "(" + i + ")";
                thisArea.script = thisArea.gameObject.GetComponent<Section>();
                thisArea.gameObject.transform.position = new Vector3(thisArea.x * sectionWidth
                                                                       , thisArea.y * sectionHeight
                                                                       , thisArea.z * sectionWidth);

                // 1. �ִ���� ���� �� ������ ������ ���� ������
                thisArea.blockedDir = createBlockedDir(thisArea);
                // 2. ���� ���� ����鿡 ���� ���� �߰��Ͽ� ����
                thisArea.blockedDir = DirectionExt.AddDirection(thisArea.blockedDir, thisArea.passedDir);

                // 3. ���� ������ ���� ���� ���� ����
                if (thisArea.blockedDir.Count < TOTAL_DIR_CNT)
                {
                    thisArea.nextDir = randomNextDir(thisArea.blockedDir);

                }
                // 4. ������ ����, ���� ���� ���� ���� �� �����
                thisArea.openDir = DirectionExt.AddDirection(thisArea.openDir, thisArea.passedDir);
                thisArea.openDir = DirectionExt.AddDirection(thisArea.openDir, thisArea.nextDir);
                thisArea.blockedDir = ReBlockDir(thisArea.openDir);

                if (thisArea.blockedDir.Count >= TOTAL_DIR_CNT)
                {
                    ReGenerateSection(i);
                }


                // 5. ���� ��/�ⱸ ���� ������Ʈ �۾�
                if (i == 0)
                {
                    // 0��° �����ΰ�� ���� ���� �������� ����
                    thisArea.blockedDir = DirectionExt.AddDirection(thisArea.blockedDir, thisArea.passedDir);   // ù���� �Ա����� ���߰�
                    CreateGate(thisArea, DirectionExt.GetOpposite(thisArea.nextDir), PurposeOfGate.startPoint); // �����Ա� ����
                }
                else if (i == areaArray.Length - 1)
                {
                    // ������ ���� ����
                    thisArea.blockedDir = DirectionExt.AddDirection(thisArea.blockedDir, thisArea.nextDir);     // ���������� �ⱸ���� ���߰�
                    CreateGate(thisArea, DirectionExt.GetOpposite(thisArea.passedDir), PurposeOfGate.endPoint); // �����ⱸ ����
                }

                thisArea.script.area = thisArea;    // ������ ����������Ʈ�� ���� �������� ���ǵ� ����class ����
                coordinate[thisArea.x, thisArea.y, thisArea.z] = thisArea;// ������ �Ҵ�� ��ǥ�� �űԻ����� class���� ����
                areaArray[i] = thisArea;  // �ش� ���������� index�� ���� ���� ���� ����
            }

            for (int i = 0; i < areaArray.Length; i++)
            {
                //Debug.Log($"{i}��° �ε��� ���� ����");
                areaArray[i].script.OnInit();
            }
        }

        List<Direction> createBlockedDir(Area area)
        {
            area.blockedDir = new List<Direction>();

            if (area.x == 0)
            {
                area.blockedDir.Add(Direction.W);
            }
            if (area.y == 0)
            {
                area.blockedDir.Add(Direction.Down);
            }
            if (area.z == 0)
            {
                area.blockedDir.Add(Direction.S);
            }
            if (area.x == (MaxWidth - 1))
            {
                area.blockedDir.Add(Direction.E);
            }
            if (area.y == (MaxHeight - 1))
            {
                area.blockedDir.Add(Direction.Up);
            }
            if (area.z == (MaxDepth - 1))
            {
                area.blockedDir.Add(Direction.N);
            }
            
            if (area.x > 0 || area.x < (MaxWidth - 1) || area.y > 0 || area.y < (MaxHeight-1) || area.z > 0 || area.z < (MaxDepth - 1))
            {
                area.blockedDir = OtherDirBlock(area);
            }
            
            return area.blockedDir;
        }

        List<Direction> OtherDirBlock(Area area)
        {
            if (area.x > 0)
            {
                if (coordinate[area.x - 1, area.y, area.z] != null)
                    area.blockedDir = DirectionExt.AddDirection(area.blockedDir, Direction.W);
            }
            if (area.y > 0)
            {
                if (coordinate[area.x, area.y - 1, area.z] != null)
                    area.blockedDir = DirectionExt.AddDirection(area.blockedDir, Direction.Down);
            }
            if (area.z > 0)
            {
                if (coordinate[area.x, area.y, area.z - 1] != null)
                    area.blockedDir = DirectionExt.AddDirection(area.blockedDir, Direction.S);
            }
            if (area.x < (MaxWidth - 1))
            {
                if (coordinate[area.x + 1, area.y, area.z] != null)
                    area.blockedDir = DirectionExt.AddDirection(area.blockedDir, Direction.E);
            }
            if (area.y < (MaxHeight - 1))
            {
                if (coordinate[area.x, area.y + 1, area.z] != null)
                    area.blockedDir = DirectionExt.AddDirection(area.blockedDir, Direction.Up);
            }
            if (area.z < (MaxDepth - 1))
            {
                if (coordinate[area.x, area.y ,area.z + 1] != null)
                    area.blockedDir = DirectionExt.AddDirection(area.blockedDir, Direction.N);
            }
            
            return area.blockedDir;
        }

        List<Direction> ReBlockDir(List<Direction> openDir)
        {
            List<Direction> dirList = new List<Direction>();
            dirList.Add(Direction.N);
            dirList.Add(Direction.E);
            dirList.Add(Direction.S);
            dirList.Add(Direction.W);
            dirList.Add(Direction.Up);
            dirList.Add(Direction.Down);

            foreach(Direction dir in openDir)
            {
                dirList.Remove(dir);
            }
            //dirList.Remove(passedDir);
            //dirList.Remove(nextDir);
            return dirList;
        }

        Direction randomNextDir(List<Direction> blockedDir)
        {
            //Debug.Log($"�ߺ� ȸ�� : {blockedDir.Count}");
            Direction nextDir;
            bool isClash;
            int loopNum = 0;

            while (true)
            {
                nextDir = (Direction)Random.Range(1, TOTAL_DIR_CNT + 1);
                isClash = DirectionExt.CheckdDirList(blockedDir, nextDir);

                if (isClash == false)
                {
                    break;
                }
                if (loopNum++ > 50)
                {
                    Debug.Log($"Infinite Loop // {thisArea.Index}");
                    foreach(Direction data in blockedDir)
                    {
                        Debug.Log($"data : {data}");
                    }
                    ExistMap();
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
                case Direction.Up:
                    area.y += 1;
                    break;
                case Direction.Down:
                    area.y -= 1;
                    break;
                default:
                    Debug.Log("���� �̵��� ������ ����");
                    break;
            }
            area.passedDir = DirectionExt.GetOpposite(area.passedDir);

            return area;
        }

        private int GetReIndex(int i)
        {
            Debug.Log($"-------------���� ���� : {i}--------------");
            List<int> reindexList = new List<int>();
            Area tempArea = new Area();

            for (int preIndex = 0; preIndex < i; preIndex++)
            {
                tempArea.x = areaArray[preIndex].x;
                tempArea.y = areaArray[preIndex].y;
                tempArea.z = areaArray[preIndex].z;
                List<Direction> dirs = createBlockedDir(tempArea);

                if (dirs.Count < TOTAL_DIR_CNT && preIndex > 0)
                {
                    // ����� �ĺ����� �������� ���� = �̷θ� ������ �Ա� �ٷ� ���� �ⱸ���� ���¹�����
                    reindexList.Add(preIndex);
                }
            }
            
            StringBuilder temp = new StringBuilder();
            temp.Append("����� �ĺ� : ");
            foreach (int idx in reindexList)
            {
                temp.Append(idx+", ");
            }

            int reindex = reindexList[Random.Range(0, reindexList.Count)];
            Debug.Log($"{temp} / ���� ��ȣ : {reindex}");
            Debug.Log($"------------------------------------------");
            return reindex;
        }

        private void ReGenerateSection(int i)
        {
            #region �����
            // ����� ���� �ʱ�ȭ
            int reindex = GetReIndex(i);
            //Debug.Log($"���� ���� : {i} /  ����� �ε��� : {reindex}");
            thisArea.openDir = new List<Direction>();
            thisArea.x = areaArray[reindex].x;
            thisArea.y = areaArray[reindex].y;
            thisArea.z = areaArray[reindex].z;
            thisArea.blockedDir = createBlockedDir(thisArea);
            thisArea.passedDir = randomNextDir(thisArea.blockedDir);


            thisArea = TransDir(thisArea);
            // ���缽���� ��ġ�� ����� ������ġ���� ���� ���� ��ǥ �̵�

            // ����� ��ġ���� �̵��� �ش� ��ǥ���� �� �籸��
            thisArea.gameObject.transform.position =  new Vector3(thisArea.x * sectionWidth
                                                                 ,thisArea.y * sectionHeight
                                                                 ,thisArea.z * sectionWidth);
            thisArea.blockedDir = createBlockedDir(thisArea);
            if (thisArea.blockedDir.Count < TOTAL_DIR_CNT)
            {
                thisArea.nextDir = randomNextDir(thisArea.blockedDir);
                thisArea.openDir = DirectionExt.AddDirection(thisArea.openDir, thisArea.nextDir);
            }

            // ����� ���ǿ��� �߰� ������ ���� ������� ��θ� �籸��
            areaArray[reindex].openDir = DirectionExt.AddDirection(areaArray[reindex].openDir, DirectionExt.GetOpposite(thisArea.passedDir));
            areaArray[reindex].blockedDir = ReBlockDir(areaArray[reindex].openDir);
            areaArray[reindex].script.area = areaArray[reindex];

            thisArea.openDir = DirectionExt.AddDirection(thisArea.openDir, thisArea.passedDir);
            thisArea.blockedDir = ReBlockDir(thisArea.openDir);
            #endregion
        }

        // ��/�ⱸ ������Ʈ ����
        private void CreateGate(Area area, Direction direction, PurposeOfGate purpose)
        {
            GameObject gate = Instantiate(entranceObj, this.transform);
            Vector3 postion = new Vector3(  area.x * sectionWidth
                                           ,area.y * sectionHeight
                                           ,area.z * sectionWidth);
            postion.x += sectionWidth / 2;
            //postion.y += sectionHeight / 2;
            postion.z += sectionWidth / 2;
            gate.transform.position = postion;
            DirectionExt.createDirectinalSurface(direction, gate);


            if (purpose == PurposeOfGate.startPoint)
            {
                gate.GetComponent<DungeonGate>().enableSpawn();
            }
            else if (purpose == PurposeOfGate.endPoint)
            {
                gate.GetComponent<DungeonGate>().enableExit();
            }
        }

        #region ���� Ȯ�ο뵵
        private void ExistMap()
        {
            StringBuilder temp = new StringBuilder();
            for (int y = 0; y < MaxHeight; y++)
            {
                temp.Append(y + "�� : \n");
                for (int z = MaxDepth - 1; z > -1; z--)
                {
                    for (int x = 0; x < MaxWidth; x++)
                    {
                        if (coordinate[x, y, z] != null)
                        {
                            temp.Append(1);
                        }
                        else
                        {
                            temp.Append(0);
                        }
                        temp.Append(", ");
                    }
                    temp.Append("\n");
                }
                temp.Append("\n");
            }
            Debug.Log(temp);
        }
        #endregion
    }
}