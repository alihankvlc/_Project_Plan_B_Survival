using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Slot_Settings
{
    [System.Serializable]
    public class SlotHandler
    {
        private List<Slot> _slots;
        private GameObject _slotPrefab;
        private SlotItem _slotItem;

        public List<Slot> Slots => _slots;
        public SlotItem SlotItem => _slotItem;

        public void Init(List<Slot> slots, GameObject slotPrefab, SlotItem slotItem)
        {
            _slots = slots;
            _slotPrefab = slotPrefab;
            _slotItem = slotItem;
        }

        public Slot FindAvailableSlot(ItemData data)
        {
            Slot availableSlot = _slots.FirstOrDefault(r => r.Type == SlotType.ToolBelt && r.Status == SlotStatus.Empty);

            if (availableSlot == null)
                availableSlot = _slots.FirstOrDefault(r => r.Type == SlotType.Inventory && r.Status == SlotStatus.Empty);

            availableSlot?.SetSlotStatus(SlotStatus.Occupied);
            return availableSlot;
        }

        public void InitializeInventory(int size, int initialIndex, Transform inventoryPlaceHolder) =>
            SpawnSlot(inventoryPlaceHolder, size, initialIndex, SlotType.Inventory);

        public void InitializeToolbelt(int size, int initialIndex, Transform toolbeltPlaceHolder) =>
            SpawnSlot(toolbeltPlaceHolder, size, initialIndex, SlotType.ToolBelt);


        private void SpawnSlot(Transform parent, int size, int initialIndex, SlotType slotType)
        {
            for (int i = 0; i < size; i++)
            {
                GameObject slotPrefab = Object.Instantiate(_slotPrefab, parent);

                Slot slot = AddSlotComponent(slotPrefab, slotType);
                slot.SetSlotIndex(i + initialIndex);

                string slotName = $"{slotType.ToString()}_Slot_Index = {i + initialIndex}";
                slotPrefab.transform.name = slotName;

                _slots?.Add(slot);
            }
        }

        private Slot AddSlotComponent(GameObject slotObject, SlotType slotType)
        {
            Slot slot = slotType switch
            {
                SlotType.Inventory => slotObject.AddComponent<InventorySlot>(),
                SlotType.ToolBelt => slotObject.AddComponent<ToolBeltSlot>(),
                _ => null
            };

            return slot;
        }
    }
}

