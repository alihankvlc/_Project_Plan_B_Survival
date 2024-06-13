using System;
using System.Collections.Generic;
using System.Linq;
using _Other_.Runtime.Code;
using Sirenix.Utilities;
using UnityEngine;

namespace _UI_Managment_.Runtime.Menu.Common
{
    public interface IMenuManager
    {
        public void SetMenuState(MenuType menuType);
        public void ToggleInventoryMenu();

        public MenuType ActiveMenu { get; }
    }

    public sealed class MenuManager : Singleton<MenuManager>, IMenuManager
    {
        [Header("Menu Settings")] [SerializeField]
        private List<MenuButtonEventArgs> _menuButtonEventArgs = new();

        [SerializeField] private Menu[] _menus;
        [SerializeField] private MenuType _activeMenu = MenuType.None;
        [SerializeField] private GameObject _activeMenuSelectionLine;
        [SerializeField] private GameObject _menuWindowParent;

        [SerializeField] private int _activeIndex = 1;

        public MenuType ActiveMenu
        {
            get => _activeMenu;
            private set
            {
                _activeMenu = value;
                _activeIndex = (int)_activeMenu;
            }
        }

        private void Start() => _menuButtonEventArgs.ForEach(r => r.OnButtonPressed += OnPressButtonEvent);

        private void Update()
        {
            if (_activeMenu != MenuType.None)
                NavigateMenu();
        }

        private void NavigateMenu()
        {
            int menuCount = _menuButtonEventArgs.Count;

            if (Input.GetKeyDown(KeyCode.Q))
                _activeIndex = (_activeIndex - 1 + menuCount - 1) % menuCount + 1;
            else if (Input.GetKeyDown(KeyCode.E))
                _activeIndex = (_activeIndex % menuCount) + 1;

            SetSelectionLine(_activeIndex);
            SetMenuState((MenuType)_activeIndex);
        }

        public void ToggleInventoryMenu()
        {
            ActiveMenu = _activeMenu == MenuType.None ? MenuType.Inventory : MenuType.None;
            SetMenuState(ActiveMenu);
        }

        public void ToogleCraftingMenu()
        {
        }

        public void SetMenuState(MenuType menuType)
        {
            _menuWindowParent.SetActive(menuType != MenuType.None);

            _menus.ForEach(r =>
            {
                bool isActiveMenu = r.Type == menuType;
                r.SetEnable(isActiveMenu);

                if (isActiveMenu)
                    ActiveMenu = menuType;
            });
        }

        private void OnPressButtonEvent(MenuType menuType, Transform lineParent)
        {
            SetMenuState(menuType);
            SetSelectionLine(menuType, lineParent);
        }

        private void DisableActiveMenu(MenuType type)
        {
            Action action = type switch
            {
                MenuType.Inventory => ToggleInventoryMenu,
                _ => null,
            };

            action?.Invoke();
        }

        private void SetSelectionLine(MenuType menuType = MenuType.None, Transform customTransform = null)
        {
            Transform targetTransform = customTransform ??
                                        _menuButtonEventArgs.FirstOrDefault(r => r.MenuType == menuType)?.transform;

            if (targetTransform != null)
                _activeMenuSelectionLine.transform.SetParent(targetTransform, false);
        }

        private void SetSelectionLine(int index)
        {
            if (_activeIndex > 0)
            {
                MenuButtonEventArgs menu = _menuButtonEventArgs.FirstOrDefault(r => r.MenuType == (MenuType)index);
                _activeMenuSelectionLine.transform.SetParent(menu.transform, false);
            }
        }

        private void OnDestroy() => _menuButtonEventArgs.ForEach(r => r.OnButtonPressed -= OnPressButtonEvent);
    }
}