using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeData
{
    [Header("Upgrade")]
    public UpgradeType type;

    [Header("Current Progress")]
    [HideInInspector]
    public int level;

    public int maxLevel = 10;

    [Header("Effect")]
    public float amountPerLevel = 1f;

    [Header("Economy")]
    public int startingCost = 100;
    public int costIncreasePerLevel = 50;

}
