using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteBossRoomSpawner : MonoBehaviour
{
    public GameObject[] eliteBossPrefabs;

    private void Start()
    {
        if(eliteBossPrefabs != null)
        {
            Instantiate(eliteBossPrefabs[Random.Range(0, eliteBossPrefabs.Length)], transform.position, transform.rotation);
        }
    }
}
