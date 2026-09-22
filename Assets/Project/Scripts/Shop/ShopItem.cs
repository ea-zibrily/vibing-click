using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Vibing
{
    /// <summary>
    /// Represents an individual purchasable item or upgrade in the shop.
    /// </summary>
    public class ShopItem : MonoBehaviour
    {
        [Header("Item Configuration")]
        [Tooltip("Unique ID/Key for this shop item (e.g. 'multiplier', 'particles', 'color')")]
        [SerializeField] private string itemKey;
        [SerializeField] private string itemName;
        [SerializeField] [TextArea(2, 3)] private string itemDescription;
        [SerializeField] private long baseCost = 10;
        [SerializeField] private float costMultiplier = 1.5f;
        [SerializeField] private bool isOneTimePurchase = false;
        [SerializeField] private int currentLevel = 0;
        [SerializeField] private int maxLevel = -1;

        [Header("UI Component References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descText;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Button buyButton;
        [SerializeField] private TextMeshProUGUI buyButtonText;

        [Header("Events")]
        public UnityEvent<ShopItem> onPurchased;

        public string ItemKey => itemKey;
        public string ItemName => itemName;
        public int CurrentLevel => currentLevel;
        public bool IsOneTimePurchase => isOneTimePurchase;

        public bool IsMaxLevel => isOneTimePurchase ? currentLevel >= 1 : (maxLevel > 0 && currentLevel >= maxLevel);
        public bool IsOwned => isOneTimePurchase && currentLevel >= 1;

        public long CurrentCost
        {
            get
            {
                if (isOneTimePurchase) return baseCost;
                return (long)(baseCost * Mathf.Pow(costMultiplier, currentLevel));
            }
        }

        private void Awake()
        {
            if (buyButton != null)
            {
                buyButton.onClick.AddListener(OnBuyClicked);
            }
        }

        private void Start()
        {
            UpdateUI();
        }

        public void SetupItem(string key, string name, string description, long cost, bool oneTime, int maxLvl = -1)
        {
            itemKey = key;
            itemName = name;
            itemDescription = description;
            baseCost = cost;
            isOneTimePurchase = oneTime;
            maxLevel = maxLvl;
            UpdateUI();
        }

        public void OnBuyClicked()
        {
            if (IsMaxLevel) return;
            if (ClickerManager.Instance == null) return;

            long cost = CurrentCost;
            if (ClickerManager.Instance.TrySpendScore(cost))
            {
                currentLevel++;
                onPurchased?.Invoke(this);
                if (ShopManager.Instance != null)
                {
                    ShopManager.Instance.HandleItemPurchased(this);
                }
                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            if (titleText != null) titleText.text = itemName;
            if (descText != null) descText.text = itemDescription;

            if (levelText != null)
            {
                if (isOneTimePurchase)
                {
                    levelText.text = IsOwned ? "<color=#55FF88>UNLOCKED</color>" : "<color=#FFBB55>LOCKED</color>";
                }
                else
                {
                    levelText.text = $"Level {currentLevel}";
                }
            }

            if (costText != null)
            {
                if (IsMaxLevel)
                {
                    costText.text = isOneTimePurchase ? "<color=#55FF88>OWNED</color>" : "<color=#AAAAAA>MAX</color>";
                }
                else
                {
                    costText.text = $"{CurrentCost:N0} Clicks";
                }
            }

            if (buyButton != null)
            {
                long currentClicks = ClickerManager.Instance != null ? ClickerManager.Instance.Score : 0;
                bool canAfford = !IsMaxLevel && currentClicks >= CurrentCost;

                buyButton.interactable = canAfford;

                if (buyButtonText != null)
                {
                    if (IsMaxLevel)
                    {
                        buyButtonText.text = isOneTimePurchase ? "OWNED" : "MAX";
                    }
                    else
                    {
                        buyButtonText.text = isOneTimePurchase ? "UNLOCK" : "UPGRADE";
                    }
                }
            }
        }
    }
}
