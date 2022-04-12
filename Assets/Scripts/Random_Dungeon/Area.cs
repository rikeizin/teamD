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
        public int z;
        public Direction passedDir;
        public Direction nextDir;
        public List<Direction> blockedDir = new List<Direction>();

        public bool IsExit = false;
        public bool IsSpawn = false;
    }
}
