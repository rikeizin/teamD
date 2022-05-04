using RandomMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, ILift
{
    public float minLowerHeight = 0f;
    public float maxUpperHeight = 5.0f;
    public Direction currentDirection = Direction.Up;

    [SerializeField]
    private GameObject oneSizeLadder;
    private int LadderLength = 0;

    public Vector3 startPos;

    private void Start()
    {
        InitLift(currentDirection, minLowerHeight, maxUpperHeight);
    }

    public void InitLift(Direction dir, float _minLowerHeight, float _maxUpperHeight)
    {
        currentDirection = dir;
        maxUpperHeight = _maxUpperHeight;
        minLowerHeight = _minLowerHeight;

        startPos = this.transform.localPosition;
        switch (currentDirection)
        {
            case Direction.Down:
                this.transform.localPosition += new Vector3(0, minLowerHeight, 0);
                break;
            case Direction.Up:
                this.transform.localPosition += new Vector3(0, minLowerHeight, 0);
                break;
        }
        
        BuildLadder();
    }

    void BuildLadder()
    {
        LadderLength = (int)Mathf.Ceil(Mathf.Abs(maxUpperHeight - minLowerHeight));

        for (int i = 1; i < LadderLength; i++)
        {
            GameObject part = Instantiate(oneSizeLadder
                                            ,this.transform.position + new Vector3(0, i , 0)
                                            ,this.transform.rotation * Quaternion.Euler(0,0,0)
                                            ,this.transform);
        }
    }
}
