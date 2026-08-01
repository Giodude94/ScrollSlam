using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [Header("Base Stats")]
    [SerializeField] private float baseMaxSpeed = 20f;
    [SerializeField] private float baseBounceForce = 18f;
    [SerializeField] private float baseMaxHeight = 10f;
    [SerializeField] private float baseSlamForce = 40f;
    [SerializeField] private int baseMaxSlams = 3;

    private float CalculateStat(UpgradeType type, float baseValue)
    {
        UpgradeData upgrade = UpgradeManager.Instance.GetUpgrade(type);

        if (upgrade == null) { return baseValue; }

        return baseValue + upgrade.level * upgrade.amountPerLevel;
    }
    public float MaxSpeed => CalculateStat(UpgradeType.MaxSpeed, baseMaxSpeed);
    public float BounceForce => CalculateStat(UpgradeType.BounceForce, baseBounceForce);
    public float MaxHeight => CalculateStat(UpgradeType.MaxHeight, baseMaxHeight);
    public float SlamForce => CalculateStat(UpgradeType.SlamForce, baseSlamForce);
    public int MaxSlams => Mathf.RoundToInt(CalculateStat(UpgradeType.MaxSlams, baseMaxSlams));


}
