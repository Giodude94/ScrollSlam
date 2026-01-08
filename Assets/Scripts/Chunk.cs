using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Chunk : MonoBehaviour
{
    //Currently the Chunk moves background and spawns enemies.
    [Header("Chunk Info")]
    public int chunkIndex;
    public float length = 19.2f;

    [Header("Spawn Areas")]
    public SpawnArea groundSpawnArea;
    public SpawnArea airSpawnArea;
    
    [Header("Enemy Spawning")]
    public EnemySpawnTable spawnTable;

    [Header("Spawn Tuning")]
    public int minEnemies = 1;
    public int maxEnemies = 7;
    [Range(0f, 1f)] public float maxAirEnemyChance = .4f;

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
        float difficulty = GetDifficulty();

        int enemyCount = Mathf.RoundToInt(Mathf.Lerp(minEnemies, maxEnemies, difficulty));

        float airChance = Mathf.Lerp(0f, maxAirEnemyChance, difficulty);
        //Debug.Log("Enemy Count: " + enemyCount);
        for (int i = 0; i < enemyCount; i++)
        {
            EnemyType type = Random.value < airChance ? EnemyType.Air : EnemyType.Ground;
            SpawnEnemy(type);
        }
    }

    void SpawnEnemy(EnemyType type)
    { 
        List<EnemySpawnEntry> list = type == EnemyType.Ground ? spawnTable.groundEnemies : spawnTable.airEnemies;

        //Obtain random prefab from specified type (Air/Ground)
        GameObject prefab = GetRandomEnemyPrefab(list);
        
        if (prefab == null) 
        {
            return; 
        }

        //Get Random point to spawn within the collider
        Vector3 spawnPos = type == EnemyType.Ground ? groundSpawnArea.GetRandomWorldPoint(transform) : airSpawnArea.GetRandomWorldPoint(transform);

        Instantiate(prefab,spawnPos, Quaternion.identity);
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


}