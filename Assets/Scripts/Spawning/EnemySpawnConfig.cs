using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spawning/Enemy Spawn Config")]
public class EnemySpawnConfig : ScriptableObject
{
    [Header("Chunk Spawn Count")]
    public int minEnemies;
    public int maxEnemies;

    [Header("Enemy Pool")]
    public EnemySpawnEntry[] entries;
}
