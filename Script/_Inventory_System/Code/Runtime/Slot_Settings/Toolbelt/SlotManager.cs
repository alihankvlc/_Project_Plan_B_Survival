using System;
using System.Collections.Generic;
using System.Linq;
using _Item_System_.Runtime.Base;
using _Loot_System_.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Inventory_System_.Code.Runtime.SlotManagment
{
    public interface ISlotManager
    {
        public Slot FindAvailableSlot(ItemData data);
        public List<InventorySlot> InventorySlots { get; }
        public List<ToolBeltSlot> ToolBeltSlots { get; }
        public List<LootingSlot> LootingSlots { get; }
        public List<Slot> SwapableSlots { get; }
        public List<Slot> AllSlots { get; }
        public SlotItem SlotItem { get; }
    }

    public class SlotManager : MonoBehaviour, ISlotManager
    {
        [Header("General Slot Settings")] [SerializeField]
        private List<Slot> _allSlots = new();

        [SerializeField] private List<Slot> _swapableSlots = new();

        [SerializeField] private SlotItem _slotItem;

        [Header("Inventory Slot Settings")] [SerializeField]
        private GameObject _inventorySlotGameObject;

        [SerializeField,InlineEditor] private List<InventorySlot> _inventorySlots = new();
        [SerializeField] private Transform _inventorySlotContainer;
        [SerializeField] private int _inventorySize;

        [Header("ToolBelt Slot Settings")] [SerializeField]
        private GameObject _toolbeltSlotGameObject;

        [SerializeField,InlineEditor] private List<ToolBeltSlot> _toolbeltSlots = new();

        [SerializeField] private Transform _toolBeltPlaceHolder;

        [SerializeField] private int _toolBeltSize;


        [Header("Loot Slot Settings")] [SerializeField,InlineEditor]
        private List<LootingSlot> _lootingSlots = new();

        [SerializeField] private GameObject _lootingSlotGameObject;
        [SerializeField] private int _lootSlotSize;
        [SerializeField] private Transform _lootSlotContainer;

        private SlotInitializer _slotInitializer;

        public SlotItem SlotItem => _slotItem;

        public List<InventorySlot> InventorySlots => _inventorySlots;
        public List<ToolBeltSlot> ToolBeltSlots => _toolbeltSlots;
        public List<LootingSlot> LootingSlots => _lootingSlots;
        public List<Slot> SwapableSlots => _swapableSlots;
        public List<Slot> AllSlots => _allSlots;

        [Inject]
        private void Constructor(SlotInitializer slotInitializer)
        {
            _slotInitializer = slotInitializer;

            _slotInitializer.InitializeToolBeltSlot(_toolbeltSlots, _toolbeltSlotGameObject, _toolBeltSize, 0,
                _toolBeltPlaceHolder);

            _allSlots.AddRange(_toolbeltSlots.Cast<Slot>());

            _slotInitializer.InitializeInventorySlot(_inventorySlots, _inventorySlotGameObject, _inventorySize,
                _toolbeltSlots.Count, _inventorySlotContainer);

            _allSlots.AddRange(_inventorySlots.Cast<Slot>());

            _slotInitializer.InitializeLootSlot(_lootingSlots, _lootingSlotGameObject, _lootSlotSize,
                _inventorySlots.Count + _toolbeltSlots.Count,
                _lootSlotContainer);

            _allSlots.AddRange(_lootingSlots.Cast<Slot>());

            _swapableSlots.AddRange(_toolbeltSlots);
            _swapableSlots.AddRange(_inventorySlots);
        }

        public Slot FindAvailableSlot(ItemData data)
        {
            Slot availableSlot = _toolbeltSlots.FirstOrDefault(r =>
                r.Type == SlotType.ToolBelt && r.Status == SlotStatus.Empty);

            if (availableSlot == null)
            {
                availableSlot = _inventorySlots.FirstOrDefault(r =>
                    r.Type == SlotType.Inventory && r.Status == SlotStatus.Empty);
            }

            availableSlot?.SetSlotStatus(SlotStatus.Occupied);
            return availableSlot;
        }
    }
}