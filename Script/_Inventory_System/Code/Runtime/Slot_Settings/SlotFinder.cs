using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using System.Collections.Generic;
using System.Linq;

namespace _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Slot_Settings
{
    [System.Serializable]
    public class SlotFinder
    {
        private List<Slot> _slots;

        public SlotFinder(List<Slot> slots)
        {
            _slots = slots;
        }

        public Slot FindAvailableSlot(ItemData data)
        {
            Slot availableSlot = _slots.FirstOrDefault(r => r.Type == SlotType.ToolBelt && r.Status == SlotStatus.Empty);

            if (availableSlot == null)
                availableSlot = _slots.FirstOrDefault(r => r.Type == SlotType.Inventory && r.Status == SlotStatus.Empty);

            availableSlot?.SetSlotStatus(SlotStatus.Occupied);
            return availableSlot;
        }
    }
}

