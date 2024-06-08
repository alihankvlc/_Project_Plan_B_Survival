using _Inventory_System_.Code.Runtime.Common;
using _Item_System_.Runtime.Base;
using _Item_System_.Runtime.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Inventory_System_.Code.Runtime.SlotManagment
{
    public enum StackType { Increase, Decrease }
    public sealed class SlotItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler

    {
        [Header("Slot Settings")]
        [SerializeField, ReadOnly] private Slot _activeSlot;
        [SerializeField, ReadOnly] private int _activeSlotIndex;

        [Header("Slot in Item Settings")]
        [SerializeField] private ItemData _data;
        [SerializeField] private int _slotInItemCount;
        [SerializeField, ShowIf("@_data is WeaponData")] private int _durability;

        [Header("UI Settings")]
        [SerializeField] private ItemDisplay _display;

        private bool _isHoveringSlot;
        private CanvasGroup _canvasGroup;
        private Slot _parentAfterSlot;

        private RectTransform _reftTransform;

        public Transform ParentAfterDrag;
        public ItemData Data => _data;
        public Slot Slot => _activeSlot;
        public bool IsHoveringSlot => _isHoveringSlot;

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
            _reftTransform = GetComponent<RectTransform>();
        }

        public void Constructor(ItemData data, Slot slot, int count = 1)
        {
            _data = data;
            _slotInItemCount = count;
            _activeSlot = slot;

            _display.UpdateSlotDisplay(_data, count);
            _durability = (data is WeaponData weaponData) ? weaponData.Durability : 0;

            slot.SetSlotItem(this);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            _parentAfterSlot = _activeSlot;
            ParentAfterDrag = transform.parent;

            transform.SetParent(transform.root);
            transform.SetAsLastSibling();

            _display.DisableUIElements();
            _display.ItemImage.raycastTarget = false;

        }

        public void OnDrag(PointerEventData eventData)
        {
            _isHoveringSlot = true;

            _display.ItemImage.transform.position = Input.mousePosition;
            _canvasGroup.alpha = 0.25f;
        }

        public void OnEndDrag(PointerEventData eventData)
        {            
            _canvasGroup.alpha = 1;
            _display.EnableUIElements();

            _display.ItemImage.raycastTarget = true;
            transform.SetParent(ParentAfterDrag);

            _reftTransform.localPosition = new Vector3(_reftTransform.localPosition.x, _reftTransform.localPosition.y, 0);
            _isHoveringSlot = false;
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
            transform.SetParent(_activeSlot.transform, true);
        }

        public void UpdateDurability(int amount)
        {
            if (!(_data is WeaponData)) return;

            _durability += amount;
            _display.UpdateDurability(_durability);
        }
    }
}
