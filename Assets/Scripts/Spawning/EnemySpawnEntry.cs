using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnEntry
{
    public EnemyType enemyType;

    public GameObject prefab;

    [Header("Weighted Spawn")]
    public int weight = 1;
}
