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
        [SerializeField, ReadOnly] private ConsumableData _consumableData;

        private SlotItem _slotItem;
        private IItemManagement _itemManagement;

        [Inject]
        private void Constructor(IItemManagement itemManagment)
        {
            _itemManagement = itemManagment;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && IsEquipped && _slotItem != null)
            {
                _consumableData.Consume();
                _itemManagement.RemoveItemFromInventory(_slotItem.Slot.Index);
            }
        }

        public override void ItemEquipmentHandler(SlotItem slotItem)
        {
            if (slotItem.Data is ConsumableData consumableData && !IsEquipped)
            {
                IsEquipped = true;

                _slotItem = slotItem;
                _consumableData = consumableData;
            }
        }

        public override void ItemUnEquipmentHandler(SlotItem slotItem)
        {
            if (slotItem.Data is ConsumableData consumableData && IsEquipped && _consumableData != null)
            {
                IsEquipped = false;

                _consumableData = null;
                _slotItem = null;
            }
        }
    }
}

