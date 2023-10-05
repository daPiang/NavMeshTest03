using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private GameObject ground;
    [Range(1, 100)]
    [SerializeField] private int spawnCount = 10;
    private Vector3 spawnRange1;
    private Vector3 spawnRange2;

    private void Start()
    {
        float rangeX = Mathf.Pow(ground.transform.localScale.x, 2);
        float rangeZ = Mathf.Pow(ground.transform.localScale.z, 2);
        
        spawnRange1 = new(rangeX, 1, rangeZ);
        spawnRange2 = new(-rangeX, 1, -rangeZ);

        for (int i = 0; i < spawnCount; i++)
        {
            // Generate a random position within the specified range
            Vector3 randomPosition = new(
                UnityEngine.Random.Range(spawnRange1.x, spawnRange2.x),
                UnityEngine.Random.Range(spawnRange1.y, spawnRange2.y),
                UnityEngine.Random.Range(spawnRange1.z, spawnRange2.z)
            );

            // Instantiate the object at the random position
            Instantiate(npcPrefab, randomPosition, Quaternion.identity);
        }
    }
}
