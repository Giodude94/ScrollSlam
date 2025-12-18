using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;

    [SerializeField] private float spawnDistanceAhead = 20f;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private float minY = -2f;
    [SerializeField] private float maxY = 4f;

    private float nextSpawnX;

    void Start()
    {
        nextSpawnX = player.position.x + spawnInterval;
    }

    void Update()
    {
        if (player.position.x + spawnDistanceAhead >= nextSpawnX)
        {
            SpawnEnemy();
            nextSpawnX += spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        float y = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(nextSpawnX, y, 0f);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}