using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
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
            //SpawnEnemy();
            nextSpawnX += RandomSpawnRangeX();
        }

        
        //Finding enemies that are more than 20 units behind the player in the x direction and deleting them to free up space.
        var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Where(go => (Vector3.Distance(player.position, go.transform.position) > cleanupDistance) && go.transform.position.x < player.position.x).ToList();
        
        foreach (var enemy in enemies)
        {
            //Debug.Log("Enemy would be destroyed.");
            Destroy(enemy.gameObject);
        }
    }
    
    float RandomSpawnRangeX()
    {
        float SpawDistanceX =  Random.Range(minX, maxX);
        //Debug.Log(SpawDistanceX.ToString());
        return SpawDistanceX;
    }
}