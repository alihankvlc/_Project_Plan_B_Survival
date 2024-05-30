using System.Collections.Generic;
using _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Slot_Settings;
using Zenject;
using UnityEngine;

namespace _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Common
{
    public class InventoryInitializer : MonoBehaviour
    {
        [Header("Slot Settings")]
        [SerializeField] private List<Slot> _slots = new();
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private SlotItem _slotItemPrefab;

        [Space, Header("Inventory Settings")]
        [SerializeField] private Inventory _inventory;
        [SerializeField] private Transform _inventoryParent;
        [SerializeField] private int _inventoryMaxWeight;
        [SerializeField] private int _inventorySize;

        [Space, Header("ToolBelt Settings")]
        [SerializeField] private ToolBelt _toolBelt;
        [SerializeField] private Transform _toolBeltParent;
        [SerializeField] private int _toolBeltSize;

        private SlotHandler _slotHandler;
        private InventoryWeight _weightHandler;

        [Inject]
        private void Constructor(SlotHandler slotHandler, InventoryWeight inventoryWeight)
        {
            _slotHandler = slotHandler;
            _weightHandler = inventoryWeight;

            _weightHandler.SetMaxWeight(_inventoryMaxWeight);

            _slotHandler.Init(_slots, _slotPrefab, _slotItemPrefab);
            _slotHandler.InitializeToolbelt(_toolBeltSize, 0, _toolBeltParent);
            _slotHandler.InitializeInventory(_inventorySize, _toolBeltSize, _inventoryParent);
        }
    }
}

