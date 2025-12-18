using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;

    [SerializeField] private float spawnDistanceAhead = 20f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minX = 3f;
    [SerializeField] private float minY = -2f;
    [SerializeField] private float maxY = 10f;

    [SerializeField] private float cleanupDistance = 20f;
    [SerializeField] private List<Enemy> enemies;

    private float nextSpawnX;

    void Start()
    {
        nextSpawnX = player.position.x + RandomSpawnRangeX();
    }

    void Update()
    {

        if (player.position.x + spawnDistanceAhead >= nextSpawnX)
        {
            SpawnEnemy();
            nextSpawnX += RandomSpawnRangeX();
        }

        //Cleanup enemies that are further than a certain distance away from the player.
        var enemies = FindObjectsOfType<Enemy>().Where(go => Vector3.Distance(go.transform.position, player.position) >= cleanupDistance).ToList();
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }
    void SpawnEnemy()
    {
        float y = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(nextSpawnX, y, 0f);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
    
    float RandomSpawnRangeX()
    {
        float SpawDistanceX=  Random.Range(minX, maxX);
        Debug.Log(SpawDistanceX.ToString());
        return SpawDistanceX;
    }
}