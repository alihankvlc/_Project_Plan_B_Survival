using System.Collections.Generic;
using System.Linq;
using _Item_System_.Runtime.Base;
using TMPro;
using UnityEngine;

namespace _Inventory_System_.Code.Runtime.SlotManagment
{
    [System.Serializable]
    public sealed class SlotHandler
    {
        private List<Slot> _slots;
        private SlotItem _slotItem;

        public List<Slot> Slots => _slots;
        public SlotItem SlotItem => _slotItem;

        public void Init(List<Slot> slots, SlotItem slotItem)
        {
            _slots = slots;
            _slotItem = slotItem;
        }

        public Slot FindAvailableSlot(ItemData data)
        {
            Slot availableSlot = _slots.FirstOrDefault(r => r.Type == SlotType.Inventory && r.Status == SlotStatus.Empty);
            availableSlot?.SetSlotStatus(SlotStatus.Occupied);
            return availableSlot;
        }

        public void InitializeInventory(GameObject prefab, int size, int initialIndex, Transform inventoryPlaceHolder) =>
            SpawnSlot(prefab, inventoryPlaceHolder, size, initialIndex, SlotType.Inventory);

        public void InitializeToolBelt(GameObject prefab, int size, int initialIndex, Transform placeHolder) =>
            SpawnSlot(prefab, placeHolder, size, initialIndex, SlotType.ToolBelt);


        private void SpawnSlot(GameObject prefab, Transform parent, int size, int initialIndex, SlotType slotType)
        {
            for (int i = 0; i < size; i++)
            {
                GameObject slotPrefab = Object.Instantiate(prefab, parent);

                Slot slot = GetSlotComponent(slotPrefab, slotType);
                slot.SetSlotIndex(i + initialIndex);

                string slotParentName = $"{slotType.ToString()}_SLOT = {i + initialIndex}";
                string slotName = $"{slotType.ToString()}_SLOT_INDEX = {i}";

                slotPrefab.transform.name = slotParentName;
                slot.transform.name = slotName;

                if (slotType == SlotType.ToolBelt)
                {
                    TextMeshProUGUI labelKeyIndex = slotPrefab.GetComponentInChildren<TextMeshProUGUI>();
                    labelKeyIndex?.SetText((i + +1).ToString());
                }

                _slots?.Add(slot);
            }
        }

        private Slot GetSlotComponent(GameObject slotObject, SlotType slotType)
        {
            Slot slot = slotType switch
            {
                SlotType.Inventory => slotObject.GetComponentInChildren<InventorySlot>(),
                SlotType.ToolBelt => slotObject.GetComponentInChildren<ToolBeltSlot>(),
                _ => null
            };

            return slot;
        }
    }
}

