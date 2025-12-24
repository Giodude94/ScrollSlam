using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnEntry
{
    public GameObject prefab;

    [Range(0f, 10f)]
    public int weight = 1;

    [Header("Difficulty Gates")]
    public int minChunkIndex = 0;
    public int maxChunkIndex = 999;
}
