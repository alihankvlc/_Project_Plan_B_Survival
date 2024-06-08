using System.Collections;
using System.Collections.Generic;
using _Crafting_System_.Runtime.UI;
using _Inventory_System_.Code.Runtime.Common;
using _Item_System_.Runtime.Base;
using _Item_System_.Runtime.Database;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace _Crafting_System_.Code.Runtime.Common
{
    public class CraftingManager : MonoBehaviour
    {
        [Header("Window Settings")]
        [SerializeField] private GameObject _craftingWindow;

        [Header("Crafting Item Settings")]
        [SerializeField] private ItemCraftingHandler _itemCraftingHandlerPrefab;
        [SerializeField] private Transform _itemCraftingHandlerContainer;

        private IItemManagement _itemManagment;

        [Inject]
        private void Consturctor(IItemManagement itemManagment)
        {
            _itemManagment = itemManagment;
        }
        private void Start()
        {
            CraftingPanelDisplay.OnCraftingItemRequested += OnCraftedItem;
        }

        private void OnCraftedItem(ItemData data, int count = 1)
        {
            data.CraftingRequirement.ForEach(r =>
            {
                _itemManagment.RemoveItemFromInventory(r.Data, r.Quantity * count);
            });

            GameObject craftingItem = Instantiate(_itemCraftingHandlerPrefab.gameObject, _itemCraftingHandlerContainer);
            ItemCraftingHandler itemCraftingHandler = craftingItem.GetComponent<ItemCraftingHandler>();
            itemCraftingHandler.CraftItem(data, count);
        }

        private void OnDestroy()
        {
            CraftingPanelDisplay.OnCraftingItemRequested -= OnCraftedItem;
        }
    }
}

