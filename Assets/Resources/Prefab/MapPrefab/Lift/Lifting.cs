using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifting : MonoBehaviour
{
    private Vector3 currentLiftPos;
    public float minPosition = 0.25f;
    public float maxPosition = 5.0f;
    public bool LiftUP = true;
    public float liftSpeed = 2.0f;

    void Update()
    {
        currentLiftPos = transform.position;
        if (LiftUP)
        {
            currentLiftPos.y += liftSpeed * Time.deltaTime;
            transform.position = currentLiftPos;
            if (transform.position.y > maxPosition)
            {
                LiftUP = false;
            }
        }
        else
        {
            
            currentLiftPos.y -= liftSpeed * Time.deltaTime;
            //currentLiftPos.y = Mathf.Clamp(currentLiftPos.y, minPosition, maxPosition);

            transform.position = currentLiftPos;
            if (transform.position.y < minPosition)
            {
                LiftUP = true;
            }
        }
        
    }
}
