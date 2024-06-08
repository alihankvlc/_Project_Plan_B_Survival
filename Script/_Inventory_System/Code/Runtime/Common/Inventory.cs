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
using LoggingUtility = _Other_.Runtime.Code.LoggingUtility;
using Sirenix.Utilities;

namespace _Inventory_System_.Code.Runtime.Common
{
    public interface IItemManagement // TODO: IItemManagement Inventory içerisinde işleniyor bunu ayrı bir sınıfa taşıyabilirim.
    {
        public void AddItemToInventory(int itemId, int count = 1);
        public void RemoveItemFromInventory(int index, int count = 1);
        public void RemoveItemFromInventory(ItemData data, int count = 1);
        public bool HasItemInInventory(int itemId, out SlotItem slotItem);
    }

    public interface ISlotManagement // TODO: ISlotManagment Inventory içerisinde işleniyor bunu ayrı bir sınıfa taşıyabilirim.
    {
        public void SwapItem(int currentSlotIndex, int nextSlotIndex);
        public void MoveSlotItem(Slot currentSlot, Slot nextSlot, SlotItem currentSlotItem);
        public bool HasItemInSlotOfType(SlotType type, out List<SlotItem> slotItem);
        public bool HasItemInSlotOfType(SlotType type, int index, out SlotItem slotItem);
        public bool GetItemInSlotByIndex(int index, out SlotItem slotItem);
        public int GetItemQuantity(int itemId);
    }

    public interface IInventoryState
    {
        public float InventoryWeight { get; }
        public bool HasInventorySpace(int itemId, int count = 1);
    }

    public sealed class Inventory : Singleton<Inventory>, IItemManagement, ISlotManagement, IInventoryState
    {
        [Header("Inventory Settings")]
        [SerializeField, ReadOnly] private float _currentWeight;

        [Space, Header("Item Data Settings")]
        [SerializeField, InlineEditor] private List<SlotItem> _slotsInItems = new List<SlotItem>();
        private ItemDatabaseProvider _itemDatabase;
        private InventoryWeight _weightHandler;
        private SlotHandler _slotHandler;
        private IToolBeltHandler _toolBeltHandler;

        public float InventoryWeight => _currentWeight;

        public static event Action<ItemData, int> OnItemAddedToInventory;

        [Inject]
        private void Constructor(ItemDatabaseProvider itemDatabase, SlotHandler slotHandler,
        InventoryWeight weightHandler, IToolBeltHandler toolBeltHandler)
        {
            _itemDatabase = itemDatabase;
            _slotHandler = slotHandler;
            _weightHandler = weightHandler;
            _toolBeltHandler = toolBeltHandler;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                AddItemToInventory(1, 1);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                AddItemToInventory(2, 2);
            }
        }

        public void AddItemToInventory(int itemId, int count = 1)
        {
            ItemData itemData = _itemDatabase.GetItemData(itemId);

            if (itemData == null)
            {
                LoggingUtility.Log.Warning(this, $"Item with id {itemId} not found in database", Color.cyan);
                return;
            }

            if (!HasInventorySpace(itemId, count)) return;

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

                if (!HasInventorySpace(itemId, count)) break;

                int amountToAdd = Mathf.Min(count, itemData.StackCapacity);
                AddItemToNewSlot(itemData, amountToAdd);
                count -= amountToAdd;
            }
        }

        public void RemoveItemFromInventory(int index, int count = 1)
        {
            SlotItem slotItem;

            if (GetItemInSlotByIndex(index, out slotItem))
            {
                int decreaseCount = Mathf.Min(slotItem.SlotInItemCount, count);
                slotItem.StackItem(StackType.Decrease, decreaseCount);
                UpdateInventoryWeight(-slotItem.Data.Weight * decreaseCount);

                if (slotItem.SlotInItemCount == 0)
                {
                    slotItem.Slot.SetSlotStatus(SlotStatus.Empty);
                    _slotsInItems.Remove(slotItem);

                    if ((slotItem.Slot is ToolBeltSlot toolBeltSlot) && toolBeltSlot.IsSelectedSlot)
                        _toolBeltHandler.UnEquip();

                    Destroy(slotItem.gameObject);
                }
            }
        }

        public void RemoveItemFromInventory(ItemData data, int count = 1)
        {
            int remainingCount = count;

            for (int i = _slotsInItems.Count - 1; i >= 0; i--)
            {
                SlotItem slotItem = _slotsInItems[i];
                if (slotItem.Data == data)
                {
                    int removeCount = Mathf.Min(slotItem.SlotInItemCount, remainingCount);
                    slotItem.StackItem(StackType.Decrease, removeCount);
                    UpdateInventoryWeight(-slotItem.Data.Weight * removeCount);

                    remainingCount -= removeCount;

                    if (slotItem.SlotInItemCount == 0)
                    {
                        slotItem.Slot.SetSlotStatus(SlotStatus.Empty);
                        _slotsInItems.RemoveAt(i);

                        if ((slotItem.Slot is ToolBeltSlot toolBeltSlot) && toolBeltSlot.IsSelectedSlot)
                            _toolBeltHandler.UnEquip();

                        Destroy(slotItem.gameObject);
                    }

                    if (remainingCount <= 0)
                        break;
                }
            }
        }

        public void SwapItem(int currentSlotIndex, int nextSlotIndex)
        {
            Slot currentSlot = _slotHandler.Slots.FirstOrDefault(r => r.Index == currentSlotIndex);
            Slot nextSlot = _slotHandler.Slots.FirstOrDefault(r => r.Index == nextSlotIndex);

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

            if (currentSlotItem.Data.Id != nextSlotItem.Data.Id ||
                !currentSlotItem.Data.Stackable || !nextSlotItem.Data.Stackable)
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

            if (currentSlot is ToolBeltSlot currentToolBeltSlot && currentToolBeltSlot.IsSelectedSlot
            || (nextSlot is ToolBeltSlot nextToolBeltSlot && nextToolBeltSlot.IsSelectedSlot))
            {
                _toolBeltHandler.UnEquip();
            }
        }

        public bool HasInventorySpace(int itemId, int count = 1)
        {
            ItemData data = _itemDatabase.GetItemData(itemId);

            if (data.Stackable) return HasSpaceForStackableItem(data, count);
            else return HasSpaceForNonStackableItem();
        }

        public bool HasItemInInventory(int itemId, out SlotItem slotItem)
        {
            slotItem = _slotsInItems.FirstOrDefault(r => r.Data.Id == itemId);
            return slotItem != null;
        }

        public bool HasItemInSlotOfType(SlotType type, out List<SlotItem> slotItems)
        {
            slotItems = _slotsInItems.Where(r => r.Slot.Type == type).ToList();
            return slotItems.Count > 0;
        }

        public bool HasItemInSlotOfType(SlotType type, int index, out SlotItem slotItem)
        {
            slotItem = _slotsInItems.FirstOrDefault(r => r.Slot.Type == type && r.Slot.Index == index);
            return slotItem != null;
        }

        public bool GetItemInSlotByIndex(int index, out SlotItem slotItem)
        {
            slotItem = _slotsInItems.FirstOrDefault(r => r.Slot.Index == index);
            return slotItem != null;
        }

        public int GetItemQuantity(int itemId)
        {
            return _slotsInItems.Where(slotItem =>
            slotItem.Data.Id == itemId).Sum(slotItem => slotItem.SlotInItemCount);
        }

        private void ProcessSameTypeItems(Slot currentSlot, Slot nextSlot, SlotItem currentSlotItem, SlotItem nextSlotItem)
        {
            if (!currentSlotItem.Data.Stackable || !nextSlotItem.Data.Stackable)
                return;

            int stackableSpace = nextSlotItem.Data.StackCapacity - nextSlotItem.SlotInItemCount;

            if (stackableSpace == 0 || currentSlotItem.SlotInItemCount - currentSlotItem.Data.StackCapacity == 0)
            {
                SwapSlotItems(currentSlot, nextSlot, currentSlotItem, nextSlotItem);
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

            if ((currentSlot is ToolBeltSlot currentToolBeltSlot && currentToolBeltSlot.IsSelectedSlot)
            || (nextSlot is ToolBeltSlot nextToolbeltSlot && nextToolbeltSlot.IsSelectedSlot))
            {
                _toolBeltHandler.UnEquip();
            }
        }

        private bool HasSpaceForStackableItem(ItemData data, int count)
        {
            foreach (var slotItem in _slotsInItems.Where(r => r.Data.Id == data.Id))
                if (slotItem.SlotInItemCount < data.StackCapacity)
                    return true;

            return _slotsInItems.Count < _slotHandler.Slots.Count;
        }

        private bool HasSpaceForNonStackableItem()
        {
            return _slotsInItems.Count < _slotHandler.Slots.Count;
        }

        private void UpdateInventoryWeight(float weightChange)
        {
            _weightHandler.IncreaseWeight(weightChange, ref _currentWeight);
        }

    }
}

