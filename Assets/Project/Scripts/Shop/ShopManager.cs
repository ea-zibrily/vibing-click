using System.Collections.Generic;
using UnityEngine;

namespace Vibing
{
    /// <summary>
    /// Coordinates shop items, upgrades, and synchronizes affordability with player score.
    /// </summary>
    public class ShopManager : MonoBehaviour
    {
        public static ShopManager Instance { get; private set; }

        [Header("Shop References")]
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private List<ShopItem> shopItems = new List<ShopItem>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            if (shopPanel == null)
            {
                shopPanel = gameObject;
            }

            if (shopItems.Count == 0)
            {
                shopItems.AddRange(GetComponentsInChildren<ShopItem>(true));
            }
        }

        private void Start()
        {
            if (ClickerManager.Instance != null)
            {
                ClickerManager.Instance.onScoreChanged.AddListener(OnScoreChanged);
            }

            UpdateAllItemsUI();
        }

        private void OnDestroy()
        {
            if (ClickerManager.Instance != null)
            {
                ClickerManager.Instance.onScoreChanged.RemoveListener(OnScoreChanged);
            }
        }

        private void OnScoreChanged(long newScore)
        {
            UpdateAllItemsUI();
        }

        /// <summary>
        /// Refreshes affordability and labels across all shop items.
        /// </summary>
        public void UpdateAllItemsUI()
        {
            foreach (var item in shopItems)
            {
                if (item != null)
                {
                    item.UpdateUI();
                }
            }
        }

        /// <summary>
        /// Handles the unlock or upgrade effect when an item is purchased.
        /// </summary>
        public void HandleItemPurchased(ShopItem item)
        {
            if (item == null || ClickerManager.Instance == null) return;

            switch (item.ItemKey.ToLower())
            {
                case "multiplier":
                    // Each level adds +1 to points per click
                    ClickerManager.Instance.PointsPerClick += 1;
                    Debug.Log($"[Shop] Upgraded Multiplier! New PointsPerClick: {ClickerManager.Instance.PointsPerClick}");
                    break;

                case "particles":
                    // Unlocks the particle burst on square click
                    ClickerManager.Instance.HasParticleEffect = true;
                    Debug.Log("[Shop] Unlocked Click Particle Effect!");
                    break;

                case "color":
                    // Unlocks the random color changer on square click
                    ClickerManager.Instance.HasRandomColor = true;
                    Debug.Log("[Shop] Unlocked Random Color Changer!");
                    break;

                default:
                    Debug.LogWarning($"[Shop] Unhandled shop item key: {item.ItemKey}");
                    break;
            }

            UpdateAllItemsUI();
        }

        /// <summary>
        /// Toggles the visibility of the shop panel.
        /// </summary>
        public void ToggleShopPanel()
        {
            if (shopPanel != null)
            {
                shopPanel.SetActive(!shopPanel.activeSelf);
                if (shopPanel.activeSelf)
                {
                    UpdateAllItemsUI();
                }
            }
        }
    }
}
