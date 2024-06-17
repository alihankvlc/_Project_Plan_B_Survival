using System.Collections.Generic;
using System.Linq;
using _Item_System_.Runtime.Base;
using _Loot_System_.Runtime;
using TMPro;
using UnityEngine;

namespace _Inventory_System_.Code.Runtime.SlotManagment
{
    [System.Serializable]
    public sealed class SlotInitializer
    {
        public void InitializeInventorySlot(List<InventorySlot> inventorySlots, GameObject prefab, int size,
            int initialIndex,
            Transform placeHolder)
        {
            SpawnSlot<InventorySlot>(inventorySlots, prefab, placeHolder, size, initialIndex, SlotType.Inventory);
        }

        public void InitializeToolBeltSlot(List<ToolBeltSlot> toolbeltSlots, GameObject prefab, int size,
            int initialIndex, Transform placeHolder)
        {
            SpawnSlot<ToolBeltSlot>(toolbeltSlots, prefab, placeHolder, size, initialIndex, SlotType.ToolBelt);
        }

        public void InitializeLootSlot(List<LootingSlot> lootSlots, GameObject prefab, int size, int initialIndex,
            Transform placeHolder)
        {
            SpawnSlot<LootingSlot>(lootSlots, prefab, placeHolder, size, initialIndex, SlotType.Loot);
        }

        private void SpawnSlot<T>(List<T> slotList, GameObject prefab, Transform parent, int size, int initialIndex,
            SlotType slotType) where T : Slot
        {
            for (int i = 0; i < size; i++)
            {
                GameObject slotPrefab = Object.Instantiate(prefab, parent);

                T slot = GetSlotComponent<T>(slotPrefab);
                slot.SetSlotIndex(i + initialIndex);

                string slotParentName = $"{slotType.ToString()}_SLOT = {i + initialIndex}";
                string slotName = $"{slotType.ToString()}_SLOT_INDEX = {i + initialIndex}";

                slotPrefab.transform.name = slotParentName;
                slot.transform.name = slotName;

                if (slotType == SlotType.ToolBelt)
                {
                    TextMeshProUGUI labelKeyIndex = slotPrefab.GetComponentInChildren<TextMeshProUGUI>();
                    labelKeyIndex?.SetText((i + +1).ToString());
                }

                slotList?.Add(slot);
            }
        }

        private T GetSlotComponent<T>(GameObject slotObject) where T : Slot
        {
            return slotObject.GetComponentInChildren<T>();
        }
    }
}