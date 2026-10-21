using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCardUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button upgradeButton;

    private UpgradeData upgrade;
    private UpgradeShopUI shop;

    public void Setup(UpgradeData upgradeData,UpgradeShopUI shopUI)
    {
        upgrade = upgradeData;
        shop = shopUI;

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(Purchase);

        Refresh();
    }

    public void Refresh()
    {
        if (upgrade == null || upgrade.definition == null)
            return;

        UpgradeDefinition definition = upgrade.definition;

        nameText.text = definition.displayName;
        descriptionText.text = definition.description;

        if (iconImage != null)
        {
            // Assign the icon once you add one to UpgradeDefinition.
            iconImage.enabled = false;
        }

        levelText.text =
            $"Level {upgrade.level}/{definition.maxLevel}";

        bool maxedOut = upgrade.level >= definition.maxLevel;

        if (maxedOut)
        {
            costText.text = "MAX LEVEL";
            buttonText.text = "Maxed";
            upgradeButton.interactable = false;
        }
        else
        {
            int cost = UpgradeManager.Instance.GetUpgradeCost(
                definition.upgradeType
            );

            costText.text = $"{cost} coins";
            buttonText.text = "Upgrade";

            upgradeButton.interactable =
                UpgradeManager.Instance.CanPurchase(
                    definition.upgradeType
                );
        }
    }

    private void Purchase()
    {
        if (upgrade == null || upgrade.definition == null)
            return;

        bool purchased = UpgradeManager.Instance.PurchaseUpgrade(
            upgrade.definition.upgradeType
        );

        if (purchased)
        {
            Refresh();
            //made reference in current script for testing perposes before connecting to coin manager/appropriate manager.
            shop.RefreshCoinBalance();
            // The shop controller will refresh the coin balance.
        }
        else
        {
            Refresh();
        }
    }
}