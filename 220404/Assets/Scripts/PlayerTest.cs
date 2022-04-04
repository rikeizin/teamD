using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(10 * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(-10 * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            gameObject.transform.Translate(0, 0, -10 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.Translate(0, 0, 10 * Time.deltaTime);
        }
    }
}
