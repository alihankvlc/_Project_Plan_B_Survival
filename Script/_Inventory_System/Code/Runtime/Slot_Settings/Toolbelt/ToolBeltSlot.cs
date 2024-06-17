using _Inventory_System_.Code.Runtime.Common;
using _Inventory_System_.Code.Runtime.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Inventory_System_.Code.Runtime.SlotManagment
{
    [RequireComponent(typeof(ToolBeltSlotNavigation))]
    public sealed class ToolBeltSlot : Slot, IPointerClickHandler
    {
        public override SlotType Type => SlotType.ToolBelt;
        private ToolBeltSlotNavigation _slotNavigation;
        private IWindowFromInventoryHandler _inventoryWindow;
        private IToolBeltHandler _toolBeltHandler;

        [SerializeField] private bool _isSelectedSlot = false;
        public bool IsSelectedSlot => _isSelectedSlot;

        private void Start()
        {
            _slotNavigation = GetComponent<ToolBeltSlotNavigation>();
        }

        [Inject]
        private void Constructor(IWindowFromInventoryHandler inventoryWindow, IToolBeltHandler toolBeltHandler)
        {
            _inventoryWindow = inventoryWindow;
            _toolBeltHandler = toolBeltHandler;
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
            if (_inventoryWindow.IsWindowEnable &&
                eventData.button != PointerEventData.InputButton.Left)
                return;

            _toolBeltHandler.SetSelectionSlot(Index);
        }
    }
}