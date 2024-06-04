using System.Collections.Generic;
using Zenject;
using UnityEngine;
using _Inventory_System_.Code.Runtime.SlotManagment;

namespace _Inventory_System_.Code.Runtime.Common
{
    public sealed class InventoryInitializer : MonoBehaviour
    {
        [Header("Slot Settings")]
        [SerializeField] private List<Slot> _slots = new();
        [SerializeField] private GameObject _inventorySlotPrefab;
        [SerializeField] private GameObject _toolbeltSlotPrefab;
        [SerializeField] private SlotItem _slotItemPrefab;
        [SerializeField] private int _inventoryLockedSlotCount;

        [Space, Header("Inventory Settings")]
        [SerializeField] private Inventory _inventory;
        [SerializeField] private Transform _inventoryParent;
        [SerializeField] private int _inventorySize;
        [SerializeField] private int _inventoryMaxWeight;

        [Header("ToolBelt Settings")]
        [SerializeField] private Transform _toolBeltPlaceHolder;
        [SerializeField] private int _toolBeltSize;


        private SlotHandler _slotHandler;
        private InventoryWeight _weightHandler;

        [Inject]
        private void Constructor(SlotHandler slotHandler, InventoryWeight inventoryWeight)
        {
            _slotHandler = slotHandler;
            _weightHandler = inventoryWeight;

            _weightHandler.SetMaxWeight(_inventoryMaxWeight);

            _slotHandler.Init(_slots, _slotItemPrefab);
            _slotHandler.InitializeToolBelt(_toolbeltSlotPrefab, _toolBeltSize, 0, _toolBeltPlaceHolder);
            _slotHandler.InitializeInventory(_inventorySlotPrefab, _inventorySize, (_slots != null ? _slots.Count : _toolBeltSize) + 1, _inventoryParent);
        }
    }
}

