using System;
using _Input_System_.Code.Runtime;
using _Inventory_System_.Code.Runtime.Common;
using _Loot_System_.Runtime;
using _UI_Managment_.Runtime.Common;
using _UI_Managment_.Runtime.Menu.Common;
using UnityEngine;
using Zenject;

namespace _Inventory_System_.Code.Runtime.UI
{
    public interface IWindowFromInventoryHandler
    {
        public bool IsWindowEnable { get; }
    }

    public class InventoryWindowManager : MonoBehaviour, IWindowFromInventoryHandler
    {
        [Header("Inventory Window Settings")] [SerializeField]
        private GameObject _inventorySlotContainer;

        [SerializeField] private bool _inventoryWindowIsEnable;

        private IPlayerInputHandler _input;
        private IMenuManager _menuHandler;
        private IVisualHandler _uiHandler;
        private ILootWindowHandler _lootWindowHandler;

        public bool IsWindowEnable => _inventoryWindowIsEnable;

        [Inject]
        private void Constructor(IPlayerInputHandler inputHandler, IMenuManager menuHandler,
            IVisualHandler uiHandler, ILootWindowHandler lootWindowHandler)
        {
            _input = inputHandler;
            _menuHandler = menuHandler;
            _uiHandler = uiHandler;

            _lootWindowHandler = lootWindowHandler;
        }

        private void Update()
        {
            if ((_input.Inventory || Input.GetKeyDown(KeyCode.Escape) && _inventoryWindowIsEnable) &&
                !_lootWindowHandler.LootWindowEnable)
            {
                ToggleInventory();
            }

            bool isLootWindowEnable = _lootWindowHandler.LootWindowEnable;
        }

        private void ToggleInventory()
        {
            _menuHandler.ToggleInventoryMenu();
            _uiHandler.MoveToToolBeltSlot(_menuHandler.ActiveMenu != MenuType.Inventory);
            _inventoryWindowIsEnable = _menuHandler.ActiveMenu == MenuType.Inventory;
        }
    }
}