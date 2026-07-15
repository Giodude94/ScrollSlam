using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEngine.EventSystems.EventTrigger;

public class Chunk : MonoBehaviour
{
    //Currently the Chunk moves background and spawns enemies.
    [Header("Chunk Data")]
    [SerializeField] private EncounterConfig encounterConfig;
    public float length = 19.2f;
    private float chunkStartX;

    [Header("Spawn Areas")]
    [SerializeField] private SpawnArea[] spawnAreas;

    [Header("Spawn Settings")]
    [SerializeField] private float minimumSpawnLookAhead = 10f;
    [SerializeField] private float velocityLookAheadMultiplier = .5f;

    private bool hasSpawned = false;
    private int chunkIndex;

    private bool[] spawnedEncounters;
    private Transform player;
    private Rigidbody2D playerRb;

    public void Initialize(int index)
    {
        chunkIndex = index;
        hasSpawned = false;

        chunkStartX = transform.position.x;

        player = null;
        playerRb = null;
        enabled = true;

        if (encounterConfig != null) 
        {
            spawnedEncounters = new bool[encounterConfig.encounters.Length];
        }
        else
        {
            spawnedEncounters = null;
        }
    }
    private void Update()
    {

        if (!hasSpawned)
        {
            return;
        }
        CheckEncounterSpawns();
    }
    private void CheckEncounterSpawns()
    {
        
        if (encounterConfig == null)
        {
            Debug.Log("Missing Encounter Config");
            return;
        }

        if (player == null)
        {
            Debug.Log("Missing Player Reference");
            return;
        }

        float distanceIntoChunk = player.position.x - chunkStartX;

        float spawnLookAhead = minimumSpawnLookAhead;

        if (playerRb != null)
        {
            spawnLookAhead = Mathf.Max(minimumSpawnLookAhead, Math.Abs(playerRb.velocity.x) * velocityLookAheadMultiplier);
        }

        for (int i = 0; i < encounterConfig.encounters.Length; i++)
        {
            // Already spawned?
            if (spawnedEncounters[i])
                continue;

            Encounter encounter = encounterConfig.encounters[i];

            if (distanceIntoChunk + spawnLookAhead >= encounter.SpawnDistance)
            {
                //Debug.Log($"Spawning Encounter {i}");

                SpawnEncounter(encounter);

                spawnedEncounters[i] = true;
            }
        }

        // Optional optimization:
        bool allSpawned = true;

        foreach (bool spawned in spawnedEncounters)
        {
            if (!spawned)
            {
                allSpawned = false;
                break;
            }
        }

        if (allSpawned)
        {
            enabled = false;
        }

    }
    private void SpawnEncounter(Encounter encounter)
    {
        int count = UnityEngine.Random.Range(encounter.minEnemies, encounter.maxEnemies + 1);

        for (int i = 0; i < count; i++) 
        {
            SpawnEncounterEnemy(encounter);
        }
    }

    private void  SpawnEncounterEnemy(Encounter encounter)
    {
        EnemySpawnEntry entry = GetWeightedEnemy(encounter.spawnConfig);

        if (entry == null) { return; }

        SpawnArea area = GetSpawnArea(entry.preferredArea);

        if ( area == null)
        {
            return;
        }

        Vector2 yPos = area.GetRandomYPos();
        
        float x = chunkStartX + encounter.SpawnDistance +  UnityEngine.Random.Range(0f, encounter.encounterWidth);

        Vector3 spawnPos = new Vector3(x, yPos.y, 0f);

        Instantiate(
            entry.prefab,
            spawnPos,
            Quaternion.identity
            );
    }
    
    private EnemySpawnEntry GetWeightedEnemy(EnemySpawnConfig config)
    {
        int totalWeight = 0;

        foreach(var entry in config.entries)
        {
            totalWeight += entry.weight;
        }

        int roll = UnityEngine.Random.Range(0,totalWeight);

        int current = 0;

        foreach(EnemySpawnEntry entry in config.entries)
        {
            current += entry.weight;

            if (roll < current) { return entry; }
        }
        return null;
    }
    private SpawnArea GetSpawnArea(SpawnAreaType areaType)
    {
        foreach (SpawnArea area in spawnAreas)
        {
            if (area.AreaType == areaType) { return area; }
        }
        return null;
    }

    public void OnPlayerEnteredChunk(Transform playerTransform)
    {
        Debug.Log("Player entered chunk: " + name);

        if (hasSpawned) { return; }

        player = playerTransform;
        playerRb = playerTransform.GetComponent<Rigidbody2D>();

        hasSpawned = true;
    }


}