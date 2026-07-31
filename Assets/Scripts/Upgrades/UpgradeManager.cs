using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [SerializeField] private int baseUpgradeCost = 100;
    [SerializeField] private int costIncreasePerLevel = 50;

    [Header("Upgrade Levels")]
    [SerializeField] private UpgradeData[] upgrades;
    [SerializeField] private int slamForceLevel;
    [SerializeField] private int maxSlamsLevel;
    [SerializeField] private int coinDropChanceLevel;
    [SerializeField] private int coinValueLevel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public int GetLevel(UpgradeType type)
    {
        UpgradeData upgrade = GetUpgrade(type);

        if (upgrade == null)
        {
            return 0;
        }
        return upgrade.level;
    }
    public void LevelUp(UpgradeType type)
    {
        UpgradeData upgrade = GetUpgrade(type);

        if(upgrade == null) {  return; }

        if (upgrade.level >= upgrade.maxLevel) { return; }

        upgrade.level++;
    }
    public int GetUpgradeCost(UpgradeType type)
    {
        UpgradeData upgrade = GetUpgrade(type);

        if(upgrade == null) { return 0; }

        return upgrade.baseCost + (upgrade.level * upgrade.costIncrease);
    }
    public UpgradeData GetUpgrade(UpgradeType type) 
    {
        foreach (UpgradeData upgrade in upgrades)
        {
            if (upgrade.type == type)
            {
                return upgrade;
            }
        }
        return null;
    }
}
