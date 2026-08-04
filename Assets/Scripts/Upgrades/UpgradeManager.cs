using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("Upgrades")]
    [SerializeField] private UpgradeData[] upgrades;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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

    public int GetLevel(UpgradeType type)
    {
        UpgradeData upgrade = GetUpgrade(type);

        return upgrade != null ? upgrade.level : 0;
    }

    public int GetUpgradeCost(UpgradeType type)
    {
        UpgradeData upgrade = GetUpgrade(type);

        if (upgrade == null)
            return 0;

        return upgrade.startingCost +
               (upgrade.level * upgrade.costIncreasePerLvl);
    }

    public bool CanPurchase(UpgradeType type)
    {
        UpgradeData upgrade = GetUpgrade(type);

        if (upgrade == null)
            return false;

        if (upgrade.level >= upgrade.maxLevel)
            return false;

        return CoinManager.Instance.GetCurrentCoins() >= GetUpgradeCost(type);
    }

    public bool PurchaseUpgrade(UpgradeType type)
    {
        UpgradeData upgrade = GetUpgrade(type);

        if (upgrade == null)
            return false;

        //If we are attempting to go past max level we return.
        if (upgrade.level >= upgrade.maxLevel)
            return false;

        int cost = GetUpgradeCost(type);

        if (!CoinManager.Instance.TrySpendCoins(cost))
            return false;

        upgrade.level++;

        return true;
    }
}
