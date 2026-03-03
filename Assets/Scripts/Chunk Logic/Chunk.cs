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


    public void Initialize(int index)
    {
        chunkIndex = index;
        hasSpawned = false;
    }
    void SpawnEnemies()
    {
        if (spawnConfig == null || spawnAreas.Length == 0) { return; }

        foreach (var entry in spawnConfig.entries)
        {
            int count = UnityEngine.Random.Range(entry.minCount, entry.maxCount + 1);

            for (int i = 0; i < count; i++)
            {
                SpawnArea area = GetSpawnArea(entry.enemyType);
                if (area == null) { continue; }

                Vector3 spawnPos = area.GetRandomPoint();
                Instantiate(entry.prefab, spawnPos, Quaternion.identity, transform);
            }
        }
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

        SpawnEnemies();
        hasSpawned = true;
    }
}