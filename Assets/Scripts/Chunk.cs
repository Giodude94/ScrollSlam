using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public float length = 30f;
    // Add enemies, obstacles as children of this chunk

    [Header("Enemy Spawning")]
    public GameObject enemyPrefab;
    [SerializeField] public int minEnemies = 2;
    [SerializeField] public int maxEnemies = 5;
    [SerializeField] private float minY = -3.025f;
    [SerializeField] private float maxY = 10f;

    private void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        //Edge case check to make sure enemyPrefab is set before logic executes.
        if (!enemyPrefab) return;

        Debug.Log(enemyPrefab.GetComponent<Enemy>().getEnemyType());

        float y;
        if (enemyPrefab.GetComponent<Enemy>().getEnemyType() == EnemyType.Ground)
        {
            y = minY;
        }
        else
        {
            y = Random.Range(minY, maxY);
        }

        //count used for linear spawning where GetEnemyCount function scales with distance
        int count = Random.Range(minEnemies, maxEnemies + 1);

        for (int i = 0; i < GetEnemyCount(); i++)
        {
            Debug.Log("Enemy has been spawned.");
            float x = Random.Range(2f, length - 2f);
            Vector3 pos = transform.position + new Vector3(x, y, 0);
            Instantiate(enemyPrefab, pos, Quaternion.identity, transform);
        }
    }

    int GetEnemyCount()
    {
        float distance = FindObjectOfType<PlayerController>().transform.position.x;
        return Mathf.Clamp(2 + Mathf.FloorToInt(distance / 100f), 2, 10);
    }
}