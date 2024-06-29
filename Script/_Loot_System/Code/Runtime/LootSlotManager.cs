using System;
using System.Collections.Generic;
using System.Linq;
using _Inventory_System_.Code.Runtime.Common;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Item_System_.Runtime.Base;
using _Loot_System_.Runtime;
using _Other_.Runtime.Code;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public interface ILootSlotHandler
{
    void FillLootSlots(List<ItemData> items);
    List<SlotItem> SelectedLootItems();
}

public interface ILootSlotEventSubscriber
{
    void RegisterButtonEvent(LootSlotButtonEventArgs subject);
    void UnregisterButtonEvent(LootSlotButtonEventArgs subject);
}

public sealed class LootSlotManager : Singleton<LootSlotManager>, ILootSlotHandler, ILootSlotEventSubscriber
{
    [Header("Slot Settings")] [SerializeField]
    private SlotItem _slotItem;

    [SerializeField] private List<LootSlotButtonEventArgs> _lootSlotButtonEvents = new();

    [Header("Interaction Settings")] [SerializeField]
    private Button _addLootToInventoryButton;

    private ISlotManager _slotManager;
    private ILootableHandler _lootableHandler;
    private IItemManagement _itemManagement;
    private ILootWindowHandler _lootWindowHandler;

    [Inject]
    private void Constructor(ILootWindowHandler lootWindowHandler, ISlotManager slotManager,
        IItemManagement itemManagement, ILootableHandler lootableHandler)
    {
        _lootWindowHandler = lootWindowHandler;
        _slotManager = slotManager;
        _itemManagement = itemManagement;
        _lootableHandler = lootableHandler;
    }

    private void Start()
    {
        _addLootToInventoryButton.onClick.AddListener(OnAddLootToInventoryButtonClicked);
    }

    private void Update()
    {
        if (_lootWindowHandler.IsWindowEnable)
            _addLootToInventoryButton.gameObject.SetActive(SelectedLootItems().Count > 0);
    }

    public void RegisterButtonEvent(LootSlotButtonEventArgs subject)
    {
        if (!_lootSlotButtonEvents.Contains(subject))
        {
            subject.OnButtonClicked += OnSelectedButtoClicked;
            _lootSlotButtonEvents.Add(subject);
        }
    }

    public void UnregisterButtonEvent(LootSlotButtonEventArgs subject)
    {
        if (_lootSlotButtonEvents.Contains(subject))
        {
            subject.OnButtonClicked -= OnSelectedButtoClicked;
            _lootSlotButtonEvents.Remove(subject);
        }
    }

    public void OnSelectedButtoClicked(object sender)
    {
        if (sender is LootSlotButtonEventArgs slotButton)
            slotButton.Select();
    }

    public void FillLootSlots(List<ItemData> items)
    {
        items.ForEach(r =>
        {
            Slot availableSlot = FindAvailableLootSlot();
            if (availableSlot == null) return;

            GameObject displayItem = Instantiate(_slotItem.gameObject, availableSlot.transform);

            SlotItem slotItemContent = displayItem.GetComponent<SlotItem>();
            displayItem.name = r.DisplayName;

            slotItemContent.Constructor(r, availableSlot, 1);
            availableSlot.SetSlotStatus(SlotStatus.Occupied);
            _lootableHandler.ActiveLootInItems.Add(slotItemContent);
        });
    }

    public List<SlotItem> SelectedLootItems()
    {
        return _lootSlotButtonEvents.Where(r => r.IsSelected).Select(r => r.SlotItem).ToList();
    }

    public LootingSlot FindAvailableLootSlot()
    {
        return _slotManager.LootingSlots.FirstOrDefault(r => r.Status == SlotStatus.Empty && r.Type == SlotType.Loot);
    }

    private void OnAddLootToInventoryButtonClicked()
    {
        foreach (SlotItem slotItem in SelectedLootItems())
        {
            _itemManagement.AddItemToInventory(slotItem.Data.Id, slotItem.SlotInItemCount);
            _lootableHandler.ActiveLoot.RemoveInLootItem(slotItem.Data);
            slotItem.Slot.SetSlotStatus(SlotStatus.Empty);
        }
    }

    private void OnDestroy()
    {
        if (_addLootToInventoryButton != null)
            _addLootToInventoryButton.onClick.RemoveListener(OnAddLootToInventoryButtonClicked);

        _lootSlotButtonEvents.ForEach(r => r.OnButtonClicked -= OnSelectedButtoClicked);
        _lootSlotButtonEvents.Clear();
    }
}