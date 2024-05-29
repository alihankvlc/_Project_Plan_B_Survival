using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using _Project_Plan_B_Survival_Item_System.Runtime.Database;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using UnityEngine;
using _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Common;
using _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Slot_Settings;


public interface IPlayerInventory
{
    public void AddItemToInventory(int itemId, int count = 1);
    public int Weight { get; }
    public bool IsFull { get; }
}

public class Inventory : MonoBehaviour, IPlayerInventory
{
    [Header("Inventory  Settings")]
    [SerializeField] private int _maxWeight = 40;
    [SerializeField, ReadOnly] private int _currentWeight;
    [SerializeField, ReadOnly] private bool _isFull;

    [Header("Item Data Settings")]
    [SerializeField, InlineEditor] private List<SlotItem> _slotsInItems = new();

    [Header("Slot Item Settings Content")]
    [SerializeField] private List<Slot> _slots = new();
    [SerializeField] private SlotItem _slotItem;

    private ItemDatabaseProvider _itemDatabase;
    private InventoryWeight _inventoryWeight;
    private SlotFinder _slotFinder;

    public bool IsFull
    {
        get => _isFull;
        private set => _isFull = value;
    }

    public int Weight => _currentWeight;

    public static event Action<ItemData, int> OnItemAddedToInventory;


    [Inject]
    private void Constructor(ItemDatabaseProvider itemDatabase)
    {
        _slotFinder = new(_slots);
        _inventoryWeight = new(_maxWeight);

        _itemDatabase = itemDatabase;
    }

    private void Update()
    {
        _isFull = _slotsInItems.Count >= _slots.Count;

        if (Input.GetKeyDown(KeyCode.R))
        {
            AddItemToInventory(itemId: 5, 10);
        }
    }

    public void AddItemToInventory(int itemId, int count = 1)
    {
        ItemData data = _itemDatabase.GetItemData(itemId);
        if (data == null)
        {
            Logger.Log.Warning(this, $"Item with id {itemId} not found in database", Color.cyan);
            return;
        }

        while (count > 0)
        {
            if (data.Stackable)
            {
                SlotItem slotItem = _slotsInItems.FirstOrDefault(r => r.Data == data && r.SlotInItemCount < data.StackCapacity);

                if (slotItem != null)
                {
                    int availableSpace = data.StackCapacity - slotItem.SlotInItemCount;
                    int amountToStack = Mathf.Min(count, availableSpace);

                    slotItem.StackItem(StackType.Increse, amountToStack);
                    OnItemAddedToInventory?.Invoke(data, amountToStack);

                    _inventoryWeight.IncreaseWeight(data.Weight * amountToStack, ref _currentWeight);
                    count -= amountToStack;
                    continue;
                }
            }

            if (_isFull) break;

            int amountToAdd = Mathf.Min(count, data.StackCapacity);
            count -= amountToAdd;

            AddItemToNewSlot(data, amountToAdd);
        }
    }

    private void AddItemToNewSlot(ItemData data, int count)
    {
        Slot availableSlot = _slotFinder.FindAvailableSlot(data);

        if (availableSlot == null)
            return;

        GameObject displayItem = Instantiate(_slotItem.gameObject, availableSlot.transform);
        SlotItem slotItemContent = displayItem.GetComponent<SlotItem>();

        _slotsInItems.Add(slotItemContent);
        slotItemContent.Init(data, availableSlot, count);
        availableSlot.SetSlotStatus(SlotStatus.Occupied);

        _inventoryWeight.IncreaseWeight(data.Weight * count, ref _currentWeight);
        OnItemAddedToInventory?.Invoke(data, count);
    }
}
