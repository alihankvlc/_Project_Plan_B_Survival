using _Inventory_System_.Code.Runtime.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Inventory_System_.Code.Runtime.SlotManagment
{
    [RequireComponent(typeof(ToolBeltSlotNavigation))]
    public sealed class ToolBeltSlot : Slot, IPointerClickHandler
    {
        public override SlotType Type => SlotType.ToolBelt;
        private ToolBeltSlotNavigation _slotNavigation;

        [SerializeField] private bool _isSelectedSlot = false;
        public bool IsSelectedSlot => _isSelectedSlot;

        private void Start()
        {
            _slotNavigation = GetComponent<ToolBeltSlotNavigation>();
        }

        public void Select()
        {
            _slotNavigation.Select();
            _isSelectedSlot = true;
        }

        public void Deselect()
        {
            _slotNavigation.Deselect();
            _isSelectedSlot = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (InventoryManager.Instance.InventoryEnable) return;
            ToolBelt.Instance.SetSelectionSlot(Index); // TODO: ToolBeltSlot içerisinde ToolBelt sınıfının örneğine zenject ile ulaşmam lazım.
        }
    }
}

