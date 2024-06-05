using System.Collections.Generic;
using System.Linq;
using _Input_System_.Code.Runtime;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Other_.Runtime.Code;
using _UI_Managment_.Runtime.Common;
using UnityEngine;
using Zenject;

namespace _Inventory_System_.Code.Runtime.Common
{
    public interface IInventoryManagment
    {
        public List<InventorySlot> GetInventorySlots();
        public List<ToolBeltSlot> GetToolBeltSlots();

        public bool InventoryEnable { get; }
    }
    public sealed class InventoryManager : Singleton<InventoryManager>, IInventoryManagment
    {
        [Header("Slot Settings")]
        [SerializeField] private List<Slot> _slots = new();
        [SerializeField] private GameObject _inventorySlotPrefab;
        [SerializeField] private GameObject _toolbeltSlotPrefab;
        [SerializeField] private SlotItem _slotItemPrefab;
        [SerializeField] private int _inventoryLockedSlotCount;

        [Space, Header("Inventory Settings")]
        [SerializeField] private Inventory _inventory;
        [SerializeField] private Transform _inventoryParent;
        [SerializeField] private int _inventorySize;
        [SerializeField] private int _inventoryMaxWeight;
        [SerializeField] private bool _inventoryEnable;

        [Header("ToolBelt Settings")]
        [SerializeField] private Transform _toolBeltPlaceHolder;
        [SerializeField] private int _toolBeltSize;

        private IPlayerInputHandler _input;
        private IMenuManager _menuHandler;
        private IVisualHandler _uiHandler;
        private SlotHandler _slotHandler;
        private InventoryWeight _weightHandler;

        public bool InventoryEnable => _inventoryEnable;

        [Inject]
        private void Constructor(IPlayerInputHandler inputHandler, IMenuManager menuHandler,
        IVisualHandler uiHandler, SlotHandler slotHandler, InventoryWeight inventoryWeight)
        {
            _input = inputHandler;
            _menuHandler = menuHandler;
            _uiHandler = uiHandler;

            _slotHandler = slotHandler;
            _weightHandler = inventoryWeight;

            _weightHandler.SetMaxWeight(_inventoryMaxWeight);

            _slotHandler.Init(_slots, _slotItemPrefab);
            _slotHandler.InitializeToolBelt(_toolbeltSlotPrefab, _toolBeltSize, 0, _toolBeltPlaceHolder);
            _slotHandler.InitializeInventory(_inventorySlotPrefab, _inventorySize, (_slots != null ? _slots.Count : _toolBeltSize) + 1, _inventoryParent);
        }

        private void Update()
        {
            if (_input.Inventory)
            {
                _menuHandler.ToggleInventoryMenu();
                _uiHandler.MoveToToolBeltSlot(_menuHandler.ActiveMenu != MenuType.Inventory);
                _inventoryEnable = _menuHandler.ActiveMenu == MenuType.Inventory;
            }
        }

        public List<InventorySlot> GetInventorySlots()
        {
            return _slotHandler.Slots.OfType<InventorySlot>().Where(slot => slot.Type == SlotType.Inventory).ToList();
        }
        public List<ToolBeltSlot> GetToolBeltSlots()
        {
            return _slotHandler.Slots.OfType<ToolBeltSlot>().Where(slot => slot.Type == SlotType.ToolBelt).ToList();
        }
    }

}