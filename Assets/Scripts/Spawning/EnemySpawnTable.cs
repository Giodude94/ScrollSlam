using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemySpawnTable",
    menuName = "Spawning/Enemy Spawn Table"
    )]
public class EnemySpawnTable : ScriptableObject
{
    public List<EnemySpawnEntry> groundEnemies;
    public List<EnemySpawnEntry> airEnemies;
}
