using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeData
{
    public UpgradeType type;

    [Header("Progression")]
    public int level;
    public int maxLevel = 10;

    [Header("Cost")]
    public int baseCost = 100;
    public int costIncrease = 50;

    [Header("Stat Increase")]
    public float valueIncrPerLevel = 1f;

}
