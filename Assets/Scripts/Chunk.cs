using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Chunk : MonoBehaviour
{
    //Currently the Chunk moves background and spawns enemies.
    [Header("Chunk Indexing")]
    public int chunkIndex;
    public float length = 19.2f;

    [Header("Spawn Config")]
    [SerializeField] EnemySpawnConfig spawnConfig;
    
    [Header("Spawn Areas")]
    public SpawnArea groundSpawnArea;
    public SpawnArea airSpawnArea;

    [Header("Difficulty Ramp")]
    public AnimationCurve difficultyCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public int maxDifficultyChunk = 40;


    private bool hasSpawnedEnemies = false;


    private void Start()
    {
        //SpawnEnemies();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasSpawnedEnemies) { return; }

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has crossed the trigger.");
            hasSpawnedEnemies = true;
            SpawnEnemies();
        }
    }
    void SpawnEnemies()
    {
        if (spawnConfig == null){ return; }

        int difficulty = spawnConfig.GetDifficultyForChunk(chunkIndex);

        SpawnEnemyGroup(
            spawnConfig.groundEnemies,
            spawnConfig.groundCountCurve,
            groundSpawnArea,
            difficulty);

        SpawnEnemyGroup(
            spawnConfig.airEnemies,
            spawnConfig.airCountCurve,
            airSpawnArea,
            difficulty);
    }

    void SpawnEnemyGroup(List<EnemySpawnEntry> entries, AnimationCurve countCurve, SpawnArea area, int difficulty)
    { 
        //Checking cases where there are empty or null entries or spawn areas.
        if (entries == null || entries.Count == 0 || area == null) {  return; }

        int spawnCount = Mathf.RoundToInt(countCurve.Evaluate(difficulty));
        if (spawnCount <= 0) { return; }

        for (int i = 0; i < spawnCount; i++)
        {
            EnemySpawnEntry entry = GetWeightedRandom(entries);
            Vector2 spawnPos = area.GetRandomPoint();

            Instantiate(entry.prefab, spawnPos, Quaternion.identity, transform);
        }
    }

    EnemySpawnEntry GetWeightedRandom(List<EnemySpawnEntry> entries)
    {
        float totalWeight = 0f;
        foreach (var entry in entries)
        {
            totalWeight += entry.weight;
        }

        float roll = Random.Range(0f, totalWeight);

        float cumulative = 0f;
        foreach (var entry in entries)
        {
            cumulative += entry.weight;
            if (roll <= cumulative) { return entry; }
        }

        return entries[0];
    }
    GameObject GetRandomEnemyPrefab(List<EnemySpawnEntry> list){
        int totalWeight = 0;

        foreach (var enemy in list)
        {
            //If we are out of the bounds of the chunkIndex then we do nothing.
            if (chunkIndex < enemy.minChunkIndex || chunkIndex > enemy.maxChunkIndex) { continue; } 

            totalWeight += enemy.weight;
        }

        if (totalWeight == 0) {  return null; }

        float roll = Random.Range(0, totalWeight);

        foreach (var enemy in list) 
        {
            if (chunkIndex < enemy.minChunkIndex || chunkIndex > enemy.maxChunkIndex) { continue ; }

            roll -= enemy.weight;
            if (roll < 0) { return enemy.prefab; }

        }
        return null;
    }
    int GetEnemyCount()
    {
        float distance = FindObjectOfType<PlayerController>().transform.position.x;
        return Mathf.Clamp(2 + Mathf.FloorToInt(distance / 100f), 2, 10);
    }

    float GetDifficulty()
    {
        float t = Mathf.Clamp01((float)chunkIndex / maxDifficultyChunk);

        return difficultyCurve.Evaluate(t);
    }

    public void TriggerSpawn()
    {
        if (hasSpawnedEnemies) { return; }

        hasSpawnedEnemies = true;

        SpawnEnemies();

    }
}