using UnityEngine;

namespace _Inventory_System_.Code.Runtime.SlotManagment
{
    public sealed class InventorySlot : Slot
    {
        public override SlotType Type => SlotType.Inventory;
    }
}
