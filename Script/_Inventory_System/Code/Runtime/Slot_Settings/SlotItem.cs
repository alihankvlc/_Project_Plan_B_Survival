using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using UnityEngine;
namespace _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Slot_Settings
{
    public enum StackType
    {
        Increse, Decrease
    }

    public class SlotItem : MonoBehaviour
    {
        [Header("Slot Settings")]
        [SerializeField] private Slot _activeSlot;
        [SerializeField] private int _slotIndex;

        [Header("Slot in Item Settings")]
        [SerializeField] private ItemData _data;
        [SerializeField] private int _slotInItemCount;

        [Header("UI Settings")]
        [SerializeField] private ItemDisplay _display;

        public ItemData Data => _data;
        public Slot Slot => _activeSlot;

        public int SlotInItemCount => _slotInItemCount;

        public void Init(ItemData data, Slot slot, int count = 1)
        {
            _data = data;
            _slotInItemCount = count;

            _activeSlot = slot;
            _slotIndex = slot.Index;

            _display.UpdateSlotDisplay(_data, count);
        }

        public void StackItem(StackType type, int stackChangeAmount = 1)
        {
            _slotInItemCount += type == StackType.Increse ? stackChangeAmount : -stackChangeAmount;
            _display.UpdateItemCount(_slotInItemCount);
        }

        public void SwapItem(Slot nextSlot)
        {
            _activeSlot = nextSlot;
            _slotIndex = nextSlot.Index;
        }
    }
}
