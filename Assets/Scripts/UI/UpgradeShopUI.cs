using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeShopUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform upgradeContainer;
    [SerializeField] private UpgradeCardUI upgradeCardPrefab;
    [SerializeField] private TextMeshProUGUI coinBalanceText;

    private void OnEnable()
    {
        RefreshShop();
    }

    public void RefreshShop()
    {
        if (UpgradeManager.Instance == null)
            return;

        // Remove the previous cards.
        foreach (Transform child in upgradeContainer)
        {
            Destroy(child.gameObject);
        }

        // Generate a card for each registered upgrade.
        UpgradeData[] upgrades =
            UpgradeManager.Instance.GetAllUpgrades();

        foreach (UpgradeData upgrade in upgrades)
        {
            if (upgrade == null || upgrade.definition == null)
                continue;

            UpgradeCardUI card = Instantiate(
                upgradeCardPrefab,
                upgradeContainer
            );

            //Added parameter - original card.Setup(upgrade)
            card.Setup(upgrade,this);
        }

        RefreshCoinBalance();
    }

    public void RefreshCoinBalance()
    {
        if (CoinManager.Instance == null)
            return;

        coinBalanceText.text =
            $"Coins: {CoinManager.Instance.GetCurrentCoins()}";
    }
}

