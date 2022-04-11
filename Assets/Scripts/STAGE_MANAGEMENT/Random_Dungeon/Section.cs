using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RandomMap
{
    public class Section : MonoBehaviour
    {
        private GameObject[] walls;
        private GameObject[] UDSides;
        private GameObject[] enCounters;

        private GameObject floor;
        private GameObject ceiling;
        private GameObject eastWall;
        private GameObject westWall;
        private GameObject southWall;
        private GameObject nothWall;

        [SerializeField]
        public Area area;

        [SerializeField]
        private GameObject pillarPrefab;
        public Light mainLight;
        public int encounterCnt;
        public int maxEncounterCnt = 3;

        private void Awake()
        {
            walls = Resources.LoadAll<GameObject>("Prefab/MapPrefab/Stage/RandomMap/Wall");
            UDSides = Resources.LoadAll<GameObject>("Prefab/MapPrefab/Stage/RandomMap/floor&Ceiling");
            enCounters = Resources.LoadAll<GameObject>("Prefab/Enemy");
        }

        public void OnInit()
        {
            // �ٴ� �� õ�� ����
            CreatFloor();
            // �� ����
            OnCreateSection();
            // ���� ���� Ȱ��ȭ
            OnLightSettings();
            // ���� ��ī����
            OnCreateEncouter();
        }

        private void OnCreateSection()
        {
            foreach (Direction dir in area.blockedDir)
            {
                switch (dir)
                {
                    case Direction.W:
                        eastWall = new GameObject("eastWall");
                        IncreaseSize(eastWall, eastWall, dir);
                        
                        break;
                    case Direction.E:
                        westWall = new GameObject("westWall");
                        IncreaseSize(westWall, westWall, dir);
                        
                        break;
                    case Direction.S:
                        southWall = new GameObject("southWall");
                        IncreaseSize(southWall, southWall, dir);
                        
                        break;
                    case Direction.N:
                        nothWall = new GameObject("nothWall");
                        IncreaseSize(nothWall, nothWall, dir);
                        break;
                    default:
                        break;
                }
            }
        }

        private void CreatFloor()
        {
            int Index = Random.Range(0, UDSides.Length);
            floor = new GameObject("Floor");
            ceiling = new GameObject("Ceiling");
            IncreaseSize(ceiling, UDSides[Index], Direction.UP); // õ�����
            IncreaseSize(floor, UDSides[Index], Direction.Down); // �ٴڻ���
        }

        private GameObject IncreaseSize(GameObject side, GameObject _peace, Direction dir)
        {
            Vector3 sidePos = Vector3.zero;
            for (int i = 0; i < MapManager.sectionSize; i++)
            {
                for (int j = 0; j < MapManager.sectionSize; j++)
                {
                    GameObject Peace = null;
                    sidePos = side.transform.position;
                    sidePos.x += MapManager.sectionDefaultWidth / 2;
                    sidePos.z += MapManager.sectionDefaultWidth / 2;
                    switch (dir)
                    {
                        case Direction.W:
                            Peace = GetWallPeace(side.transform);
                            sidePos.z += MapManager.sectionDefaultWidth * i;
                            sidePos.y += MapManager.sectionDefaultHeight * j;
                            Peace.transform.rotation = Quaternion.Euler(0, 90, 0);
                            if (i == j)
                            {
                                GameObject pillar = Instantiate(pillarPrefab, side.transform);
                                pillar.transform.position = new Vector3(0, MapManager.sectionDefaultHeight * j, 0);
                            }
                            break;

                        case Direction.E:

                            Peace = GetWallPeace(side.transform);
                            sidePos.z += MapManager.sectionDefaultWidth * i;
                            sidePos.y += MapManager.sectionDefaultHeight * j;
                            sidePos.x += MapManager.SectionWidth - MapManager.sectionDefaultWidth;
                            Peace.transform.rotation = Quaternion.Euler(0, 270, 0);
                            if (i == j)
                            {
                                GameObject pillar = Instantiate(pillarPrefab, side.transform);
                                pillar.transform.position = new Vector3(0, MapManager.sectionDefaultHeight * j, 0);
                            }
                            break;
                        case Direction.S:

                            Peace = GetWallPeace(side.transform);
                            sidePos.x += MapManager.sectionDefaultWidth * i;
                            sidePos.y += MapManager.sectionDefaultHeight * j;
                            Peace.transform.rotation = Quaternion.Euler(0, 0, 0);
                            if (i == j)
                            {
                                GameObject pillar = Instantiate(pillarPrefab, side.transform);
                                pillar.transform.position = new Vector3(0, MapManager.sectionDefaultHeight * j, 0);
                            }
                            break;

                        case Direction.N:

                            Peace = GetWallPeace(side.transform);
                            sidePos.x += MapManager.sectionDefaultWidth * i;
                            sidePos.y += MapManager.sectionDefaultHeight * j;
                            sidePos.z += MapManager.SectionWidth - MapManager.sectionDefaultWidth;
                            Peace.transform.rotation = Quaternion.Euler(0, 180, 0);
                            if (i == j)
                            {
                                GameObject pillar = Instantiate(pillarPrefab, side.transform);
                                pillar.transform.position = new Vector3(0,MapManager.sectionDefaultHeight *j,0);
                            }
                            break;

                        case Direction.UP:
                            Peace = Instantiate(_peace, side.transform);

                            sidePos.x += MapManager.sectionDefaultWidth * i;
                            sidePos.z += MapManager.sectionDefaultWidth * j;
                            sidePos.y += MapManager.SectionHeight;
                            Peace.transform.rotation = Quaternion.Euler(180, 0, 0);
                            break;

                        case Direction.Down:
                            Peace = Instantiate(_peace, side.transform);

                            sidePos.x += MapManager.sectionDefaultWidth * i;
                            sidePos.z += MapManager.sectionDefaultWidth * j;
                            break;
                    }
                    Peace.transform.position = sidePos;
                }
            }
            
            sidePos = this.transform.position;
            side.transform.position = sidePos;
            side.transform.parent = this.transform;
            return side;
        }

        private GameObject GetWallPeace(Transform side)
        {
            int wallIndex = Random.Range(0, walls.Length);
            GameObject Peace = Instantiate(walls[wallIndex], side.transform);
            return Peace;
        }

        private void OnLightSettings()
        {
            Vector3 lightVector = mainLight.transform.position;
            lightVector.x += MapManager.SectionWidth / 2;
            lightVector.y += MapManager.SectionHeight / 2;
            lightVector.z += MapManager.SectionWidth / 2;
            mainLight.transform.position = lightVector;
        }

        private void OnCreateEncouter()
        {
            encounterCnt = Random.Range(0, maxEncounterCnt);

            for (int i = 0; i < encounterCnt; i++)
            {
                //Debug.Log("������");
                int index = Random.Range(0, enCounters.Length);
                GameObject encounter = Instantiate(enCounters[index]);
                Vector3 spawnPos = encounter.transform.localPosition;
                spawnPos.x = Random.Range(1, 6);
                spawnPos.z = Random.Range(1, 6);
                //encounter.transform.localPosition = spawnPos;
                //Debug.Log(floor.transform.position);
                Vector3 floorPivot = transform.position + new Vector3(MapManager.SectionWidth, 0, MapManager.SectionWidth)/2;
                Vector3 randomPoint = floorPivot + Random.insideUnitSphere * (MapManager.SectionWidth / 2 - 1);
                NavMeshHit hit;
                NavMesh.SamplePosition(randomPoint, out hit, 1, NavMesh.AllAreas);
                
                spawnPos.x = (int)Random.Range(floorPivot.x - MapManager.SectionWidth/2 +1, floorPivot.x + MapManager.SectionWidth/2 -1);
                spawnPos.z = (int)Random.Range(floorPivot.z - MapManager.SectionWidth/2 +1, floorPivot.z + MapManager.SectionWidth/2 -1);
                encounter.transform.localPosition = spawnPos;
            }
        }

        //private void OnDrawGizmos()
        //{
        //    Vector3 floorPivot = transform.position + new Vector3(MapManager.SectionWidth, 0, MapManager.SectionWidth) / 2;
        //    Vector3 randomPoint = floorPivot + Random.insideUnitSphere * (MapManager.SectionWidth / 2 - 1);

        //    NavMeshHit hit;
        //    if (NavMesh.SamplePosition(randomPoint, out hit, 1, NavMesh.AllAreas))
        //    {
        //        //encounter.transform.position = hit.position;
        //        vector = hit.position;
        //        Gizmos.color = Color.yellow;
        //        Gizmos.DrawSphere(hit.position, 1);
        //    }
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(floorPivot, 1);   
        //}

        //private void OnCreateRamp()
        //{
        //    // ���� ���� / ���� : ������ ���϶�
        //    if (area.passedDir != DirectionExt.GetOpposite(area.nextDir))
        //    {
        //        GameObject prefab = Instantiate(Chandelier, this.transform);
        //        Chandelier.transform.position = new Vector3(2, 4, 2);
        //    }
        //}
    }
}