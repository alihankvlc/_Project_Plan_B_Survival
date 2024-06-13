using System;
using UnityEngine;
using UnityEngine.UI;

namespace _UI_Managment_.Runtime.Menu.Common
{
    public sealed class MenuButtonEventArgs : MonoBehaviour
    {
        [SerializeField] private MenuType _menuType;

        private Button _button;

        public MenuType MenuType => _menuType;
        public event Action<MenuType, Transform> OnButtonPressed;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => OnButtonPressed?.Invoke(_menuType, transform));
        }

    }
}

