using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomMap
{
    public enum Direction : byte
    {
        None
        , N
        , E
        , S
        , W
        , Down
        , UP
    }

    public static class DirectionExt
    {
        public static Direction GetOpposite(this Direction direction)
        {
            switch (direction)
            {
                case Direction.N:
                    return Direction.S;
                case Direction.E:
                    return Direction.W;
                case Direction.S:
                    return Direction.N;
                case Direction.W:
                    return Direction.E;
                default:
                    return Direction.None;
            }
        }

        public static List<Direction> AddDirection(List<Direction> dirList, Direction checkDir)
        {
            if(checkDir != Direction.None)
            {
                bool duplicate = CheckdDirList(dirList, checkDir);
                if (duplicate == false)
                    dirList.Add(checkDir);
            }
            return dirList;
        }

        public static bool CheckdDirList(List<Direction> dirList, Direction checkDir)
        {
            bool check = false;
            if (dirList.Count > 0)
            {
                for (int i = 0; i < dirList.Count; i++)
                {
                    if (dirList[i] == checkDir)
                    {
                        check = true;
                    }
                }
            }
            return check;
        }

        public static void createDirectinalSurface(Direction direction, GameObject prefab)
        {
            switch (direction)
            {
                case Direction.W:
                    prefab.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case Direction.E:
                    prefab.transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                case Direction.S:
                    prefab.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case Direction.N:
                    prefab.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                default:
                    break;
            }
        }
    }

    
}
