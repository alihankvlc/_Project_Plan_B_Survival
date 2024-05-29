using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Slot_Settings
{
    public enum SlotType { Inventory, ToolBelt }
    public enum SlotStatus { Empty, Occupied, Locked }

    public class Slot : SerializedMonoBehaviour
    {
        [Header("Slot Settings")]
        [SerializeField] private SlotStatus _status;
        [SerializeField] private int _slotIndex;

        [SerializeField, ReadOnly]
        public virtual SlotType Type
        {
            get;
            protected set;
        }

        public int Index => _slotIndex;
        public SlotStatus Status => _status;
        public void SetSlotStatus(SlotStatus status) => _status = status;
    }
}


