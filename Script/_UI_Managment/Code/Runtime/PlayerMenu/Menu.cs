using System;
using UnityEngine;

namespace _UI_Managment_.Runtime.Menu.Common
{
    public enum MenuType { None = 0, Inventory = 1, Crafting = 2, Skills = 3, Stats = 4 }

    [Serializable]
    public sealed class Menu
    {
        [SerializeField] private MenuType _type;
        [SerializeField] private GameObject _window;

        public GameObject Window => _window;
        public MenuType Type => _type;

        public void SetEnable(bool isEnable)
        {
            _window.SetActive(isEnable);
        }

    }
}

