using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
namespace _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Slot_Settings
{
    public enum StackType { Increase, Decrease }
    public class SlotItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Slot Settings")]
        [SerializeField, ReadOnly] private Slot _activeSlot;
        [SerializeField, ReadOnly] private int _activeSlotIndex;

        [Header("Slot in Item Settings")]
        [SerializeField] private ItemData _data;
        [SerializeField] private int _slotInItemCount;

        [Header("UI Settings")]
        [SerializeField] private ItemDisplay _display;

        private CanvasGroup _canvasGroup;
        private Slot _parentAfterSlot;

        public Transform ParentAfterDrag;
        public ItemData Data => _data;
        public Slot Slot => _activeSlot;

        public int SlotInItemCount
        {
            get => _slotInItemCount;
            private set
            {
                _slotInItemCount = value;
                _display.UpdateItemCount(_slotInItemCount);
            }
        }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Constructor(ItemData data, Slot slot, int count = 1)
        {
            _data = data;
            _slotInItemCount = count;

            _activeSlot = slot;

            _display.UpdateSlotDisplay(_data, count);
            slot.SetSlotItem(this);
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            _parentAfterSlot = _activeSlot;
            ParentAfterDrag = transform.parent;

            transform.SetParent(transform.root);
            transform.SetAsLastSibling();

            _display.ItemImage.raycastTarget = false;

        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
            _canvasGroup.alpha = 0.25f;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = 1;
            _display.ItemImage.raycastTarget = true;

            transform.SetParent(ParentAfterDrag);
        }
        public void StackItem(StackType type, int stackChangeAmount = 1)
        {
            SlotInItemCount += type == StackType.Increase ? stackChangeAmount : -stackChangeAmount;
        }

        public void MoveToSlot(Slot nextSlot)
        {
            _activeSlot = nextSlot;
            _activeSlotIndex = _activeSlot.Index;

            _parentAfterSlot = _activeSlot;
            ParentAfterDrag = _activeSlot.transform;

            _activeSlot.SetSlotItem(this);
            transform.SetParent(_activeSlot.transform, false);
        }
    }
}
