using Zenject;
using UnityEngine;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Item_System_.Runtime.Base;
using System.Collections.Generic;
using System.Linq;
using _Other_.Runtime.Code;
using System;


namespace _Inventory_System_.Code.Runtime.Common
{
    public interface IToolBeltHandler
    {
        public void SetSelectionSlot(int index);
        public void UnEquip(Action action = null);

        public bool IsEquipped { get; }
    }

    public sealed class ToolBelt : Singleton<ToolBelt>, IToolBeltHandler
    {
        private const int _toolbeltSize = 4;

        private ISlotManagement _slotManagement;
        private IInventoryManagment _inventoryManagment;

        [SerializeField] private SlotItem _currentEquippedItem;

        [SerializeField] private List<ToolBeltSlot> _toolBeltSlots = new List<ToolBeltSlot>();

        private int _selectedSlotIndex = -1;

        public bool IsEquipped => _currentEquippedItem != null;

        public static event Action<SlotItem> OnItemEquipped;
        public static event Action<SlotItem> OnItemUnequipped;

        [Inject]
        private void Constructor(ISlotManagement slotManagement, IInventoryManagment inventoryManagment)
        {
            _slotManagement = slotManagement;
            _inventoryManagment = inventoryManagment;
        }

        private void Start()
        {
            _toolBeltSlots = _inventoryManagment.GetToolBeltSlots();
        }

        private void Update()
        {
            if (InventoryManager.Instance.InventoryEnable) return;

            for (int i = 0; i < _toolbeltSize; i++)
                SlotSelectionByKey(i);
        }

        public void SlotSelectionByKey(int index)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + index) && !InventoryManager.Instance.InventoryEnable)
                SetSelectionSlot(index);
        }

        public void SetSelectionSlot(int index)
        {
            if (_slotManagement.HasItemInSlotOfType(SlotType.ToolBelt, index, out SlotItem slotInItem))
            {
                if (_selectedSlotIndex == index)
                    return;

                if (_currentEquippedItem != null) UnEquip();

                _selectedSlotIndex = index;

                for (int i = 0; i < _toolBeltSlots.Count; i++)
                {
                    if (i == index)
                        _toolBeltSlots[i].Select();
                    else
                        _toolBeltSlots[i].Deselect();
                }

                SetEquipmentItem(slotInItem);
            }
            else if (_currentEquippedItem != null)
                UnEquip();
        }

        public void UnEquip(Action action = null)
        {
            OnItemUnequipped?.Invoke(_currentEquippedItem);
            action?.Invoke();

            _currentEquippedItem = null;

            for (int i = 0; i < _toolBeltSlots.Count; i++)
                _toolBeltSlots[i].Deselect();

            _selectedSlotIndex = -1;
        }

        private void SetEquipmentItem(SlotItem slotItem)
        {
            if (InventoryManager.Instance.InventoryEnable) return;

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
