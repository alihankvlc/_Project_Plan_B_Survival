using System.Collections.Generic;
using System.Linq;
using _Crafting_System_.Runtime.UI;
using _Inventory_System_.Code.Runtime.Common;
using _Item_System_.Runtime.Base;
using _Item_System_.Runtime.Database;
using _Player_System_.Runtime.Common;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Crafting_System_.Runtime.Common
{
    public enum CraftingType
    {
        None,
        Building,
        Weapon,
        Clothes,
        Food,
        Health
    }

    public enum CraftingStatus
    {
        None,
        CanCraft,
        CanNotCraft
    }

    public class CraftingMenuInitializer : MonoBehaviour
    {
        [Header("Crafting Content Settings")] [SerializeField]
        private CraftingMenuItem _bluePrintTabPrefab;

        [SerializeField] private Transform _bluePrintTabContainer;

        [Header("Crafting Slot Settings")] [SerializeField]
        private GameObject _slotPageParent;

        [Header("Crafting Item Display Settings")] [SerializeField]
        private GameObject _craftingItemDisplay;

        [SerializeField, ReadOnly] private List<CraftingPanelSlotInItemDisplay> _craftingItemDisplays;
        [SerializeField, ReadOnly] private List<CraftingMenuItem> _craftingBlueprintTabs;

        private IItemManagement _itemManagement;
        private ItemDatabaseProvider _itemDatabaseProvider;
        private IPlayerExperienceHandler _playerStat;

        [Inject]
        private void Consturctor(IItemManagement itemManagment, ItemDatabaseProvider itemDatabaseProvider,
            IPlayerExperienceHandler experienceHandler)
        {
            _itemManagement = itemManagment;
            _itemDatabaseProvider = itemDatabaseProvider;
            _playerStat = experienceHandler;
        }

        private void Start()
        {
            ShowCraftingMenuForItems<WeaponData>();
            ShowCraftingMenuForItems<ConsumableData>();
        }

        private void ShowCraftingMenuForItems<T>() where T : ItemData
        {
            List<ItemData> items = GetCraftingItemDatas<T>();

            if (items == null) return;

            CraftingMenuItem weaponBluprintTab = SpawnCraftingBlueprintTab(GetCraftingType(items[0]));
            items.ForEach(weaponData =>
            {
                CraftingPanelSlotInItemDisplay craftingItemDisplay = SpawnCraftingItemSlot(
                    weaponBluprintTab.CraftingContent.SlotContainer
                    , CraftingType.Weapon, weaponData);

                //TODO:Lock icon şeysine ileride birşeyler düşünecem...
                craftingItemDisplay.UpdateDisplay(weaponData, CraftingStatus.CanCraft);

                craftingItemDisplay.SetItemLevel(1);

                _craftingItemDisplays.Add(craftingItemDisplay);
            });

            SpawnSlotPage(items.Count, weaponBluprintTab.CraftingContent.PageContainer);
        }

        private CraftingMenuItem SpawnCraftingBlueprintTab(CraftingType type)
        {
            GameObject blueprintTab = Instantiate(_bluePrintTabPrefab.gameObject, _bluePrintTabContainer);
            CraftingMenuItem blueprintTabScript = blueprintTab?.GetComponent<CraftingMenuItem>();

            blueprintTab.transform.name = $"{type.ToString()}_BlueprintTab";
            blueprintTabScript.SetCraftingType(type);

            return blueprintTabScript;
        }

        private CraftingPanelSlotInItemDisplay SpawnCraftingItemSlot(Transform parent, CraftingType type, ItemData data)
        {
            GameObject craftingItemDisplay = Instantiate(_craftingItemDisplay, parent);
            CraftingPanelSlotInItemDisplay
                display = craftingItemDisplay?.GetComponent<CraftingPanelSlotInItemDisplay>();

            return display;
        }

        private List<ItemData> GetCraftingItemDatas<T>() where T : ItemData
        {
            List<ItemData> items = _itemDatabaseProvider.GetItemsByType<T>();

            if (items == null || items.Count == 0)
                return null;

            List<ItemData> craftableItems = items.Where(r => r.CanCrafting()).ToList();

            return craftableItems.Count > 0 ? craftableItems : null;
        }

        private CraftingType GetCraftingType(ItemData data)
        {
            CraftingType craftingType = data.ItemType switch
            {
                ItemType.Weapon => CraftingType.Weapon,
                ItemType.Consumable => CraftingType.Health,
                _ => CraftingType.None
            };

            return craftingType;
        }

        private void SpawnSlotPage(int slotCount, Transform parent)
        {
            int pageCount = CalculatePageCount(slotCount);

            for (int i = 0; i < pageCount; i++)
            {
                GameObject page = Instantiate(_slotPageParent, parent);
                TextMeshProUGUI pageNumberTextMesh = page.GetComponentInChildren<TextMeshProUGUI>();
                CraftingPageButtonEventArgs pageButtonEventArgs = page.GetComponent<CraftingPageButtonEventArgs>();

                pageButtonEventArgs.SetInteraction(i > 1);
                pageNumberTextMesh.text = $"{i + 1}";
            }
        }

        private int CalculatePageCount(int checkSize)
        {
            int pageCount = checkSize switch
            {
                30 => 2,
                60 => 3,
                90 => 4,
                _ => 1
            };

            return pageCount;
        }
    }
}