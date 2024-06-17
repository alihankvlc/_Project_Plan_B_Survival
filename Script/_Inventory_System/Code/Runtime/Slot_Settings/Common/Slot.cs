using _Inventory_System_.Code.Runtime.Common;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Inventory_System_.Code.Runtime.SlotManagment
{
    public enum SlotType
    {
        Inventory,
        ToolBelt,
        Loot
    }

    public enum SlotStatus
    {
        Empty,
        Occupied,
        Locked
    }

    public class Slot : SerializedMonoBehaviour, IDropHandler
    {
        [Header("Slot Settings")] [SerializeField]
        private SlotStatus _status;

        [SerializeField] private int _slotIndex;
        [SerializeField] private SlotItem _slotItem;

        protected ISlotManagement _slotManagment;

        [SerializeField, ReadOnly] public virtual SlotType Type { get; protected set; }

        private void Awake() => _slotManagment = Inventory.Instance;
        public SlotItem SlotInItem => _slotItem;
        public int Index => _slotIndex;
        public SlotStatus Status => _status;
        public void SetSlotStatus(SlotStatus status) => _status = status;
        public void SetSlotIndex(int index) => _slotIndex = index;
        public void SetSlotItem(SlotItem slotItem) => _slotItem = slotItem;
        public void SetSlotType(SlotType slotType) => Type = slotType;

        public virtual void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            GameObject droppedObject = eventData.pointerDrag;
            SlotItem draggingSlot = droppedObject.GetComponent<SlotItem>();

            _slotManagment.SwapItem(draggingSlot.Slot.Index, _slotIndex);
        }
    }
}