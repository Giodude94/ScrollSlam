using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Upgrade")]
public class UpgradeDefinition : ScriptableObject
{
    public  UpgradeType upgradeType;

    public string displayName;

    [TextArea]
    public string description;

    public int maxLevel = 10;

    public int baseCost = 100;

    public int costIncreasePerLevel = 50;

    public int statIncreasePerLvl = 1;

    //public float valuePerLevel = 2f;
}
