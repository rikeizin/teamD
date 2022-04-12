using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RandomMap {
    enum PurposeOfGate
    {
        startPoint,
        endPoint
    }

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
        private Area[,] coordinate; // ��ǥ
        private Area[] areaArray;
        private const int TOTAL_DIR_CNT = 4;

        [Header("���� �ʱ�ȭ")]
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

        [Header("���� ���Ա� ������Ʈ")]
        public GameObject entranceObj;

        [Header("�÷��̾� ��ŸƮ ����Ʈ")]
        public Transform playerSpawnPosition;


        private void Awake()
        {
            sectionSize = setSectionSzie;
            sectionWidth = sectionDefaultWidth * sectionSize;
            sectionHeight = sectionDefaultHeight * sectionSize;
            Initalize();
        }

        // ��/�ⱸ ������Ʈ ����
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

        // ���� �汸���� �ʻ���
        public void Initalize() {
            // �� �ʱ�ȭ
            coordinate = new Area[MaxWidth, MaxDepth];
            areaArray = new Area[creatCnt];
            //Debug.Log($"���� �ݺ�ȸ�� : {areaArray.Length}");

            for (int i = 0; i < areaArray.Length; i++)
            {
                //Debug.Log($"{i}��° Section");
                
                if (i > 0)
                {
                    thisArea = new Area();
                    thisArea.x = areaArray[i - 1].x;
                    thisArea.z = areaArray[i - 1].z;
                    thisArea.passedDir = areaArray[i - 1].nextDir;
                    thisArea = TransDir(thisArea); // ���� ������ġ�� ����
                }

                thisArea.gameObject = GameObject.Instantiate(section, this.transform);
                thisArea.script = thisArea.gameObject.GetComponent<Section>();
                thisArea.gameObject.transform.position = new Vector3(thisArea.x, 0, thisArea.z) * sectionWidth;
                // ������ �Ҵ�� ��ǥ�� �űԻ����� class���� ����
                coordinate[thisArea.x, thisArea.z] = thisArea;

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
                thisArea.blockedDir = ReBlockDir(thisArea.passedDir, thisArea.nextDir);

                // 5. ���� ��/�ⱸ ���� ������Ʈ �۾�
                if (i == 0)
                {
                    // 0��° �����ΰ�� ���� ���� �������� ����
                    thisArea.blockedDir = DirectionExt.AddDirection(thisArea.blockedDir, thisArea.passedDir);
                    CreateGate(thisArea, DirectionExt.GetOpposite(thisArea.nextDir), PurposeOfGate.startPoint );// �����Ա� ����
                }
                else if (thisArea.blockedDir.Count == TOTAL_DIR_CNT)
                {
                    // 3-2 ������ ������� ������ �������� ó��
                    thisArea.blockedDir = DirectionExt.AddDirection(thisArea.blockedDir, thisArea.nextDir);
                    CreateGate(thisArea, DirectionExt.GetOpposite(areaArray[i - 1].passedDir), PurposeOfGate.endPoint);// �����ⱸ ����
                    break;
                }
                else if(i == areaArray.Length - 1)
                {
                    // ������ ���� ����
                    thisArea.blockedDir = DirectionExt.AddDirection(thisArea.blockedDir, thisArea.nextDir);
                    CreateGate(thisArea, DirectionExt.GetOpposite(thisArea.passedDir), PurposeOfGate.endPoint);// �����ⱸ ����
                }

                thisArea.script.area = thisArea;    // ������ ����������Ʈ�� ���� �������� ���ǵ� ����class ����
                thisArea.script.OnInit(); // ���� ���� �ʱ�ȭ
                areaArray[i] = thisArea;  // �ش� ���������� index�� ���� ���� ���� ����
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
            //Debug.Log($"�ߺ� ȸ�� : {blockedDir.Count}");
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
                    Debug.Log("��������� ����");
                    break;
            }
            area.passedDir = DirectionExt.GetOpposite(area.passedDir);

            return area;
        }
    }
}