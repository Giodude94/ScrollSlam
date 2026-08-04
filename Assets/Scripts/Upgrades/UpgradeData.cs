using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeData
{
    [Header("Upgrade")]
    public UpgradeType type;

    [Header("Current Progress")]
    public int level = 0;
    public int maxLevel = 10;

    [Header("Cost")]
    public int startingCost = 100;
    public int costIncreasePerLvl = 50;

    [Header("Stat Increase")]
    public float statIncreasePerLvl = 1f;

}
