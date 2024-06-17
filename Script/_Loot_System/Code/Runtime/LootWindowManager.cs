using System;
using _Inventory_System_.Code.Runtime.Common;
using _Inventory_System_.Code.Runtime.UI;
using UnityEngine;
using Zenject;

namespace _Loot_System_.Runtime
{
    public interface ILootWindowHandler
    {
        public bool LootWindowEnable { get; }
        public void OpenLootWindow();
        public void CloseLootWindow();
    }

    public class LootWindowManager : MonoBehaviour, ILootWindowHandler
    {
        [Header("Window Settings")] [SerializeField]
        private GameObject _lootWindowGameObject;

        [Header("Item Description Settings")] [SerializeField]
        private Transform _itemDescription;

        [SerializeField] private Transform _defaultDescriptionParent;
        [SerializeField] private Transform _lootwindowEnableParent;

        private IWindowFromInventoryHandler _inventoryWindow;
        private bool _lootWindowIsEnable;

        public bool LootWindowEnable => _lootWindowIsEnable;

        [Inject]
        private void Constructor(IWindowFromInventoryHandler inventoryWindow)
        {
            _inventoryWindow = inventoryWindow;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && _lootWindowIsEnable)
                CloseLootWindow();
        }

        public void OpenLootWindow()
        {
            if (_lootWindowIsEnable && _inventoryWindow.IsWindowEnable) return;

            _lootWindowIsEnable = true;
            _lootWindowGameObject.SetActive(true);
            _itemDescription.SetParent(_lootwindowEnableParent, false);
        }

        public void CloseLootWindow()
        {
            if (!_lootWindowIsEnable && _inventoryWindow.IsWindowEnable) return;

            _lootWindowIsEnable = false;
            _lootWindowGameObject.SetActive(false);
            _itemDescription.SetParent(_defaultDescriptionParent, false);
        }
    }
}