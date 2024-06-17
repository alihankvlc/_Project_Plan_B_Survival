using Zenject;
using UnityEngine;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Item_System_.Runtime.Base;
using System.Collections.Generic;
using System.Linq;
using _Other_.Runtime.Code;
using System;
using _Inventory_System_.Code.Runtime.UI;

namespace _Inventory_System_.Code.Runtime.Common
{
    public interface IToolBeltHandler
    {
        public void SetSelectionSlot(int index);
        public void UnEquip(Action action = null);
        public bool IsEquipped { get; }
    }

    public sealed class ToolBelt : MonoBehaviour, IToolBeltHandler
    {
        private const int _toolbeltSize = 4;

        private ISlotManagement _slotManagement;
        private ISlotManager _slotInitializerHandler;
        private IWindowFromInventoryHandler _inventoryWindow;

        [SerializeField] private SlotItem _currentEquippedItem;


        [SerializeField] private int _selectedSlotIndex = -1;

        public bool IsEquipped => _currentEquippedItem != null;

        public static event Action<SlotItem> OnItemEquipped;
        public static event Action<SlotItem> OnItemUnequipped;

        [Inject]
        private void Constructor(ISlotManagement slotManagement,
            ISlotManager slotManager,
            IWindowFromInventoryHandler inventoryWindow)
        {
            _slotManagement = slotManagement;
            _slotInitializerHandler = slotManager;
            _inventoryWindow = inventoryWindow;
        }

        private void Update()
        {
            if (_inventoryWindow != null && _inventoryWindow.IsWindowEnable) return;

            for (int i = 0; i < _toolbeltSize; i++)
                SlotSelectionByKey(i);
        }

        public void SlotSelectionByKey(int index)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + index) && !_inventoryWindow.IsWindowEnable)
                SetSelectionSlot(index);
        }

        public void SetSelectionSlot(int index)
        {
            if (_slotManagement != null && _slotManagement.HasItemInSlotOfType(SlotType.ToolBelt, index, out SlotItem slotInItem))
            {
                if (_selectedSlotIndex == index)
                    return;

                if (_currentEquippedItem != null) UnEquip();

                _selectedSlotIndex = index;

                for (int i = 0; i < _slotInitializerHandler.ToolBeltSlots.Count; i++)
                {
                    if (i == index)
                        _slotInitializerHandler.ToolBeltSlots[i].Select();
                    else
                        _slotInitializerHandler.ToolBeltSlots[i].Deselect();
                }

                SetEquipmentItem(slotInItem);
            }
            else if (_currentEquippedItem != null)
                UnEquip();
        }

        public void UnEquip(Action action = null)
        {
            if (_currentEquippedItem == null) return;

            OnItemUnequipped?.Invoke(_currentEquippedItem);
            action?.Invoke();

            _currentEquippedItem = null;

            for (int i = 0; i < _slotInitializerHandler.ToolBeltSlots.Count; i++)
                _slotInitializerHandler.ToolBeltSlots[i].Deselect();

            _selectedSlotIndex = -1;
        }

        private void SetEquipmentItem(SlotItem slotItem)
        {
            if (_inventoryWindow != null && _inventoryWindow.IsWindowEnable) return;

            if (_currentEquippedItem == slotItem)
            {
                UnEquip();
                return;
            }

            Equip(slotItem);
        }

        private void Equip(SlotItem data)
        {
            _currentEquippedItem = data;
            OnItemEquipped?.Invoke(_currentEquippedItem);
        }
    }
}
