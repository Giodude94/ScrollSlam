using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnEntry
{
    public EnemyType enemyType;

    public GameObject prefab;

    [Header("Weighted Spawn")]
    [Range(0,100)]
    public int weight = 1;

    [Header("Spawn Location")]
    public SpawnAreaType preferredArea;
}
