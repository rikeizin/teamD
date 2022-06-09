using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZone : MonoBehaviour
{
    public STAGE_MANAGEMENT.StageManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            manager.StageStart();
    }
}
