using RandomMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftingFloor : MonoBehaviour, ILift
{
    private Vector3 currentLiftPos;
    public float minLowerHeight = 0.25f;
    public float maxUpperHeight = 5.0f;
    public Direction currentDirection = Direction.Up;
    public float liftSpeed = 2.0f;

    public void Start()
    {
        InitLift(currentDirection, minLowerHeight, maxUpperHeight);
    }

    void FixedUpdate()
    {
        currentLiftPos = transform.localPosition;
        if (currentDirection == Direction.Up)
        {
            currentLiftPos.y += liftSpeed * Time.deltaTime;
            transform.localPosition = currentLiftPos;
            if (transform.localPosition.y > maxUpperHeight)
            {
                currentDirection = Direction.Down;
            }
        }
        else
        {
            
            currentLiftPos.y -= liftSpeed * Time.deltaTime;
            //currentLiftPos.y = Mathf.Clamp(currentLiftPos.y, minPosition, maxPosition);

            transform.localPosition = currentLiftPos;
            if (transform.localPosition.y < minLowerHeight)
            {
                currentDirection = Direction.Up;
            }
        }
        
    }

    public void InitLift(Direction dir, float _minLowerHeight, float _maxUpperHeight)
    {
        maxUpperHeight = _maxUpperHeight;
        minLowerHeight = _minLowerHeight;
        currentDirection = dir;

        Vector3 startPos = transform.localPosition;
        switch (currentDirection)
        {
            case Direction.Up:
                startPos.y = _maxUpperHeight;
                break;
            case Direction.Down:
                startPos.y = _minLowerHeight;
                break;
        }
        transform.localPosition = startPos;
    }
}