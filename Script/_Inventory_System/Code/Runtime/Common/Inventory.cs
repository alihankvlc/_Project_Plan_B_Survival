using System;
using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Zenject;
using UnityEngine;
using _Item_System_.Runtime.Database;
using _Item_System_.Runtime.Base;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Other_.Runtime.Code;
using Logger = _Other_.Runtime.Code.Logger;

namespace _Inventory_System_.Code.Runtime.Common
{
    public interface IPlayerInventory
    {
        public void AddItemToInventory(int itemId, int count = 1);
        public void RemoveItemFromInventory(int index, bool allDestroy = false, int count = 1);
        public void SwapItem(int currentSlotIndex, int nextSlotIndex);
        public void MoveSlotItem(Slot currentSlot, Slot nextSlot, SlotItem currentSlotItem);
        public bool HasItemInInventory(int itemId, out SlotItem slotItem);
        public bool HasItemInSlotOfType(SlotType type, out List<SlotItem> slotItem);
        public bool GetItemInSlotByIndex(int index, out SlotItem slotItem);
        public float InventoryWeight { get; }
        public bool IsFull { get; }
    }

    public sealed class Inventory : Singleton<Inventory>, IPlayerInventory
    {
        [Header("Inventory Settings")]
        [SerializeField, ReadOnly] private float _currentWeight;
        [SerializeField, ReadOnly] private bool _isFull;

        [Space, Header("Item Data Settings")]
        [SerializeField, InlineEditor] private List<SlotItem> _slotsInItems = new List<SlotItem>();
        private ItemDatabaseProvider _itemDatabase;
        private InventoryWeight _weightHandler;
        private SlotHandler _slotHandler;

        public bool IsFull => _isFull;
        public float InventoryWeight => _currentWeight;

        public static event Action<ItemData, int> OnItemAddedToInventory;

        [Inject]
        private void Constructor(ItemDatabaseProvider itemDatabase, SlotHandler slotHandler, InventoryWeight weightHandler)
        {
            _itemDatabase = itemDatabase;
            _slotHandler = slotHandler;
            _weightHandler = weightHandler;
        }

        private void Update()
        {
            _isFull = _slotsInItems.Count >= _slotHandler.Slots.Count;

            if (Input.GetKeyDown(KeyCode.R)) AddItemToInventory(1, 1);
            if (Input.GetKeyDown(KeyCode.T)) AddItemToInventory(2, 2);
        }

        public void AddItemToInventory(int itemId, int count = 1)
        {
            ItemData itemData = _itemDatabase.GetItemData(itemId);
            if (itemData == null)
            {
                Logger.Log.Warning(this, $"Item with id {itemId} not found in database", Color.cyan);
                return;
            }

            while (count > 0)
            {
                if (itemData.Stackable)
                {
                    SlotItem slotItem = _slotsInItems.FirstOrDefault(si => si.Data == itemData && si.SlotInItemCount < itemData.StackCapacity);
                    if (slotItem != null)
                    {
                        int availableSpace = itemData.StackCapacity - slotItem.SlotInItemCount;
                        int amountToStack = Mathf.Min(count, availableSpace);
                        slotItem.StackItem(StackType.Increase, amountToStack);
                        UpdateInventoryWeight(itemData.Weight * amountToStack);
                        OnItemAddedToInventory?.Invoke(itemData, amountToStack);
                        count -= amountToStack;
                        continue;
                    }
                }

                if (_isFull) break;

                int amountToAdd = Mathf.Min(count, itemData.StackCapacity);
                AddItemToNewSlot(itemData, amountToAdd);
                count -= amountToAdd;
            }
        }

        public void RemoveItemFromInventory(int index, bool allDestroy = false, int count = 1)
        {
            SlotItem slotItem;
            if (GetItemInSlotByIndex(index, out slotItem))
            {
                int decreaseCount = slotItem.SlotInItemCount;
                slotItem.StackItem(StackType.Decrease, allDestroy ? decreaseCount : count);
                UpdateInventoryWeight(-slotItem.Data.Weight * (allDestroy ? decreaseCount : count));

                if (slotItem.SlotInItemCount == 0)
                {
                    slotItem.Slot.SetSlotStatus(SlotStatus.Empty);
                    _slotsInItems.Remove(slotItem);
                    Destroy(slotItem.gameObject);
                }
            }
        }

        public void SwapItem(int currentSlotIndex, int nextSlotIndex)
        {
            Slot currentSlot = _slotHandler.Slots.FirstOrDefault(s => s.Index == currentSlotIndex);
            Slot nextSlot = _slotHandler.Slots.FirstOrDefault(s => s.Index == nextSlotIndex);

            if (currentSlot == null || nextSlot == null)
                return;

            SlotItem currentSlotItem = currentSlot.SlotInItem;
            SlotItem nextSlotItem = nextSlot.SlotInItem;

            if (currentSlotItem == null)
                return;

            if (nextSlotItem == null)
            {
                MoveSlotItem(currentSlot, nextSlot, currentSlotItem);
                return;
            }

            if (currentSlotItem.Data.Id != nextSlotItem.Data.Id)
            {
                SwapSlotItems(currentSlot, nextSlot, currentSlotItem, nextSlotItem);
                return;
            }

            ProcessSameTypeItems(currentSlot, nextSlot, currentSlotItem, nextSlotItem);
        }

        public void MoveSlotItem(Slot currentSlot, Slot nextSlot, SlotItem currentSlotItem)
        {
            currentSlotItem.MoveToSlot(nextSlot);
            currentSlot.SetSlotStatus(SlotStatus.Empty);
            nextSlot.SetSlotStatus(SlotStatus.Occupied);

            nextSlot.SetSlotItem(currentSlotItem);
            currentSlot.SetSlotItem(null);
        }

        public bool HasItemInInventory(int itemId, out SlotItem slotItem)
        {
            slotItem = _slotsInItems.FirstOrDefault(si => si.Data.Id == itemId);
            return slotItem != null;
        }

        public bool HasItemInSlotOfType(SlotType type, out List<SlotItem> slotItems)
        {
            slotItems = _slotsInItems.Where(si => si.Slot.Type == type).ToList();
            return slotItems.Count > 0;
        }

        public bool GetItemInSlotByIndex(int index, out SlotItem slotItem)
        {
            slotItem = _slotsInItems.FirstOrDefault(si => si.Slot.Index == index);
            return slotItem != null;
        }

        private void ProcessSameTypeItems(Slot currentSlot, Slot nextSlot, SlotItem currentSlotItem, SlotItem nextSlotItem)
        {
            if (!currentSlotItem.Data.Stackable || !nextSlotItem.Data.Stackable)
                return;

            int stackableSpace = nextSlotItem.Data.StackCapacity - nextSlotItem.SlotInItemCount;

            if (stackableSpace == 0 || currentSlotItem.SlotInItemCount - currentSlotItem.Data.StackCapacity == 0)
            {
                SwapSlotItems(currentSlot, nextSlot, currentSlotItem, nextSlotItem);
                //Logger.Log.Warning(this, $"The destination slot is full. Item cannot be stacked further.", Color.cyan, showToPlayer: true);
                return;
            }

            int amountToStack = Mathf.Min(currentSlotItem.SlotInItemCount, stackableSpace);
            nextSlotItem.StackItem(StackType.Increase, amountToStack);
            currentSlotItem.StackItem(StackType.Decrease, amountToStack);

            if (currentSlotItem.SlotInItemCount <= 0)
            {
                currentSlot.SetSlotStatus(SlotStatus.Empty);
                _slotsInItems.Remove(currentSlotItem);
                Destroy(currentSlotItem.gameObject);
            }
        }

        private void AddItemToNewSlot(ItemData data, int count)
        {
            Slot availableSlot = _slotHandler.FindAvailableSlot(data);
            if (availableSlot == null) return;

            GameObject displayItem = Instantiate(_slotHandler.SlotItem.gameObject, availableSlot.transform);
            SlotItem slotItemContent = displayItem.GetComponent<SlotItem>();
            displayItem.name = data.DisplayName;

            slotItemContent.Constructor(data, availableSlot, count);
            availableSlot.SetSlotStatus(SlotStatus.Occupied);
            _slotsInItems.Add(slotItemContent);
            UpdateInventoryWeight(data.Weight * count);

            OnItemAddedToInventory?.Invoke(data, count);
        }

        private void SwapSlotItems(Slot currentSlot, Slot nextSlot, SlotItem currentSlotItem, SlotItem nextSlotItem)
        {
            currentSlotItem.MoveToSlot(nextSlot);
            nextSlotItem.MoveToSlot(currentSlot);

            SlotStatus tempStatus = currentSlot.Status;
            currentSlot.SetSlotStatus(nextSlot.Status);
            nextSlot.SetSlotStatus(tempStatus);

            int currentIndex = _slotsInItems.IndexOf(currentSlotItem);
            int nextIndex = _slotsInItems.IndexOf(nextSlotItem);

            _slotsInItems[currentIndex] = nextSlotItem;
            _slotsInItems[nextIndex] = currentSlotItem;
        }

        private void UpdateInventoryWeight(float weightChange)
        {
            _weightHandler.IncreaseWeight(weightChange, ref _currentWeight);
        }
    }
}

