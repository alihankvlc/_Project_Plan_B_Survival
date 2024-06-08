using _Equipment_System_.Runtime.Base;
using _Inventory_System_.Code.Runtime.Common;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Item_System_.Runtime.Base;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Equipment_System_.Runtime.Sub
{
    public class ConsumableEquipmentController : EquipmentControll
    {
        private IItemManagement _itemManagement;

        [Inject]
        private void Constructor(IItemManagement itemManagment)
        {
            _itemManagement = itemManagment;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && IsEquipped && EquippedSlotItem != null)
            {
                if (EquippedSlotItem.IsHoveringSlot) return;

                ConsumableData consumableData = EquippedSlotItem.Data as ConsumableData;
                consumableData.Consume();

                _itemManagement.RemoveItemFromInventory(EquippedSlotItem.Slot.Index);
            }
        }

        public override void ItemEquipmentHandler(SlotItem slotItem)
        {
            if (slotItem.Data is ConsumableData consumableData && !IsEquipped)
            {
                IsEquipped = true;
                EquippedSlotItem = slotItem;
            }
        }

        public override void ItemUnEquipmentHandler(SlotItem slotItem)
        {
            if (slotItem.Data is ConsumableData consumableData && IsEquipped && EquippedSlotItem != null)
            {
                IsEquipped = false;
                EquippedSlotItem = null;
            }
        }
    }
}

