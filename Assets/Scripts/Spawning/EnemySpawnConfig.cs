using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spawning/Enemy Spawn Config")]
public class EnemySpawnConfig : ScriptableObject
{
    public EnemySpawnEntry[] entries;
}
