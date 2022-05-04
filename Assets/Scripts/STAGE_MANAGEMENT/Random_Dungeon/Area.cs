using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RandomMap
{
    [System.Serializable]
    public class Area
    {
        public bool Exist = false;
        
        public GameObject gameObject;
        public Section script;
        public int Index;
        public int x;
        public int y;
        public int z;
        public Direction passedDir;
        public Direction nextDir;
        public List<Direction> blockedDir = new List<Direction>();
        public List<Direction> openDir = new List<Direction>();

        // 섹션에 출/입구 존재여부 확인용도
        public bool IsExit = false;
        public bool IsSpawn = false;

        // 위/아래 층 이동용 구조물 생성용도
        public Area firstFloorArea = null;
        public Area lastFloorArea = null;
        public int floorCount = 0; // 누적층 정보
        public Direction upStairsNextDir = Direction.None; // 누적된 최상층에서 다음 경로
         
    }
}
