using _Input_System_.Code.Runtime;
using _UI_Managment_.Runtime.Common;
using UnityEngine;
using Zenject;

namespace _Inventory_System_.Code.Runtime.Common
{
    public sealed class InventoryManager : MonoBehaviour
    {
        [Header("Inventory Settings")]
        [SerializeField] private bool _inventoryEnable;

        private IPlayerInputHandler _input;
        private IMenuManager _menuHandler;
        private IVisualHandler _uiHandler;

        public bool InventoryEnable => _inventoryEnable;

        [Inject]
        private void Constructor(IPlayerInputHandler inputHandler, IMenuManager menuHandler, IVisualHandler uiHandler)
        {
            _input = inputHandler;
            _menuHandler = menuHandler;
            _uiHandler = uiHandler;
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
    }

}