using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Encounter
{
    [Header("Spawn Timing")]
    public float SpawnDistance;

    [Header("Enemy Count")]
    public int minEnemies;
    public int maxEnemies;

    [Header("Enemy Selection")]
    public EnemySpawnConfig spawnConfig;

    [Header("Placement")]
    public float heightOffset;

    [Header("Encounter Shape")]
    public float encounterWidth = 8f;
}
