using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeData
{

    public UpgradeDefinition definition;

    [Header("Current Progress")]
    public int level = 0;

}
