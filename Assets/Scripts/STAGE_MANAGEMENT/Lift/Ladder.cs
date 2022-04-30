using RandomMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, ILift
{
    public float minLowerHeight = 0.25f;
    public float maxUpperHeight = 5.0f;
    public Direction currentDirection = Direction.Up;

    [SerializeField]
    private GameObject oneSizeLadder;
    private int LadderLength = 0;

    private void Start()
    {
        InitLift(currentDirection, minLowerHeight, maxUpperHeight);
    }

    public void InitLift(Direction dir, float _minLowerHeight, float _maxUpperHeight)
    {
        maxUpperHeight = _maxUpperHeight;
        minLowerHeight = _minLowerHeight;

        Vector3 startPos = transform.position;
        switch (currentDirection)
        {
            case Direction.Up:
                startPos.y = _maxUpperHeight;
                break;
            case Direction.Down:
                startPos.y = _minLowerHeight;
                break;
        }
        transform.position = startPos;
        BuildLadder();
    }

    void BuildLadder()
    {
        LadderLength = (int)Mathf.Ceil(Mathf.Abs(maxUpperHeight - minLowerHeight));

        for (int i = 1; i < LadderLength; i++)
        {
            GameObject part = Instantiate(oneSizeLadder
                                            ,this.transform.position + new Vector3(0, i ,0)
                                            ,this.transform.rotation * Quaternion.Euler(0,90,0)
                                            ,this.transform);
        }
    }
}
