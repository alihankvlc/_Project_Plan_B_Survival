using _Inventory_System_.Code.Runtime.SlotManagment;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Loot_System_.Runtime
{
    public class LootingSlot : Slot
    {
        public override SlotType Type => SlotType.Loot;
    }
}