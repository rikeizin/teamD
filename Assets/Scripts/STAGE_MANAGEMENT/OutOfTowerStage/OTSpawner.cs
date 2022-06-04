using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OTSpawner : MonoBehaviour
{
    public int minSpawnCount = 10;
    public int maxSpawnCount = 15;
    public GameObject[] EncounterPrefabs;

    void Start()
    {
        GameObject encounters = new GameObject("Encounters");
        if (EncounterPrefabs.Length > 0)
        {
            int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);
            
            for (int i = 0; i < spawnCount; i++)
            {
                GameObject spawnPos = Instantiate(new GameObject("Spanwer"), this.transform);

                spawnPos.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                spawnPos.transform.position = spawnPos.transform.right * 17;
                Vector3 VerticalPosOffset = spawnPos.transform.position;
                VerticalPosOffset.y = this.transform.position.y;
                spawnPos.transform.position = VerticalPosOffset;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(spawnPos.transform.position, out hit, 2.5f, NavMesh.AllAreas))
                {
                    spawnPos.transform.position = hit.position;
                    Instantiate(EncounterPrefabs[Random.Range(0, EncounterPrefabs.Length)]
                                ,spawnPos.transform.position
                                ,spawnPos.transform.rotation
                                ,encounters.transform);
                }
            }
        }
        
    }
}
