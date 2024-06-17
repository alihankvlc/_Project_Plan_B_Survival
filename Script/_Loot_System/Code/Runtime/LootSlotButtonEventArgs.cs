using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace _Inventory_System_.Code.Runtime.SlotManagment
{
    [RequireComponent(typeof(LootSlotSelectionDisplay))]
    public sealed class LootSlotButtonEventArgs : MonoBehaviour
    {
        private Button _button;
        public event Action<object> OnButtonClicked;

        private ILootSlotEventSubscriber _subscriber;
        private LootSlotSelectionDisplay _display;

        private SlotItem _slotItem;

        private bool _isSelected;

        public bool IsSelected => _isSelected;
        public SlotItem SlotItem => _slotItem;

        private void Start()
        {
            _slotItem = GetComponent<SlotItem>();
            _button = GetComponent<Button>();

            _display = GetComponent<LootSlotSelectionDisplay>();
            _button.onClick.AddListener(() => OnButtonClicked?.Invoke(this));
        }

        private void OnEnable()
        {
            LootSlotManager.Instance.RegisterButtonEvent(this);
        }

        private void OnDisable()
        {
            _isSelected = false;
            _display.Select(false);
            
            _button.onClick.RemoveListener(() => OnButtonClicked?.Invoke(this));
            LootSlotManager.Instance.UnregisterButtonEvent(this);
        }

        public void Select()
        {
            _isSelected = !_isSelected;
            _display.Select(_isSelected);
        }
    }
}