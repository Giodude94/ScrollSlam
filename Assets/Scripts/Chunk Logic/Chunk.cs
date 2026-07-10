using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Chunk : MonoBehaviour
{
    //Currently the Chunk moves background and spawns enemies.
    [Header("Chunk Data")]
    [SerializeField] private EnemySpawnConfig spawnConfig;
    public float length = 19.2f;

    [Header("Spawn Areas")]
    [SerializeField] private SpawnArea[] spawnAreas;

    private bool hasSpawned = false;
    private int chunkIndex;

    [Header("Continuous Spawning")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxEnemiesPerChunk = 10;

    private int spawnedEnemies;
    private Coroutine spawnRoutine;

    public void Initialize(int index)
    {
        chunkIndex = index;
        hasSpawned = false;
    }
    void SpawnEnemies()
    {
        Debug.Log("Calling SpawnEnemies script.");

        //Checking Edge Cases
        if (spawnConfig == null || spawnAreas.Length == 0) { Debug.Log("SpawnConfig is empty or length is 0"); return; }
        
        //For weighted spawning
        int enemyCount = UnityEngine.Random.Range(spawnConfig.minEnemies, spawnConfig.maxEnemies + 1);

            for (int i = 0; i < enemyCount; i++)
            {
                EnemySpawnEntry entry = GetWeightedEnemy();

                if (entry == null)
                {
                    continue;
                }   
                SpawnArea area = GetSpawnArea(entry.enemyType);

                if (area == null) { continue; }

                Vector3 spawnPos = area.GetRandomPoint();
                
                Instantiate(
                    entry.prefab,
                    spawnPos,
                    Quaternion.identity,
                    transform
                );
            
            }
    }
    void SpawnEnemy()
    {
        EnemySpawnEntry entry = GetWeightedEnemy();

        if (entry == null) { return; }

        SpawnArea area = GetSpawnArea(entry.enemyType );

        if (area == null) { return; }

        Vector3 spawnPos = area.GetRandomPoint();

        Instantiate(
            entry.prefab,
            spawnPos,
            Quaternion.identity,
            transform
            );
    }
    private EnemySpawnEntry GetWeightedEnemy() 
    {
        int totalWeight = 0;

        foreach (EnemySpawnEntry entry in spawnConfig.entries)
        {
            Debug.Log(entry.weight);
            totalWeight += entry.weight;
        }

        if (totalWeight <= 0) 
        {
            Debug.Log("Weight is zero");
            return null;
        }

        int randomValue = UnityEngine.Random.Range(0, totalWeight);

        int currentWeight = 0;

        foreach (EnemySpawnEntry entry in spawnConfig.entries) 
        {   
            currentWeight += entry.weight;

            if (randomValue < currentWeight)
            {
                return entry;
            }
        }
        Debug.Log("Returned Null in chunk.cs script.");
        return null;
    }
    private SpawnArea GetSpawnArea(EnemyType type) 
    {
        foreach (var area in spawnAreas) 
        {
            if (area.AllowedType == type) {  return area; }
        }
        return null;
    }

    public void OnPlayerEnteredChunk()
    {
        if (hasSpawned) { return; }

        hasSpawned = true;

        spawnRoutine = StartCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (spawnedEnemies < maxEnemiesPerChunk) {
            SpawnEnemy();
            spawnedEnemies++;

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}