using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spawning/Enemy Spawn Config")]
public class EnemySpawnConfig : ScriptableObject
{
    [Header("Difficulty")]
    [Tooltip("Maps chunk index -> difficulty value")]
    public AnimationCurve difficultyCurve;

    [Header("Ground Enemies")]
    public List<EnemySpawnEntry> groundEnemies;
    public AnimationCurve groundCountCurve;

    [Header("Air Enemies")]
    public List<EnemySpawnEntry> airEnemies;
    public AnimationCurve airCountCurve;

    public int GetDifficultyForChunk(int chunkIndex)
    {
        return Mathf.RoundToInt(difficultyCurve.Evaluate(chunkIndex));
    }
}
