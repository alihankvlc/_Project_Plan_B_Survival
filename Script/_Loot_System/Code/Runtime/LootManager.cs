using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Inventory_System_.Code.Runtime.Common;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Item_System_.Runtime.Base;
using _Loot_System_.Runtime;
using _Other_.Runtime.Code;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public enum LootType
{
    None,
    Weapon,
    Ammo,
    Consumable,
}

public enum Richness
{
    Poor,
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

[System.Serializable]
public class LootSettings
{
    public LootType Type;
    public int Size;
}

public interface ILootableHandler
{
    public Loot ActiveLoot { get; }
    public int LootSize { get; }
    public float LootOpeningDuration { get; }
    public bool IsLooting { get; }
    public List<SlotItem> ActiveLootInItems { get; }
    public void GenerateLoot(List<LootSettings> lootSettings, List<ItemData> datas, Richness richness);
    public void ResetLoot(List<LootSettings> lootSettings, List<ItemData> datas, Richness richness);
    public void SetActiveLootItems(Loot loot);
    public void RemoveItemFromLootWindow(ItemData data, ref List<ItemData> datas);
    public void StartLooting(Action action);
    public void StopLooting();
}

[RequireComponent(typeof(LootWindowManager))]
public class LootManager : Singleton<LootManager>, ILootableHandler
{
    [Header("Loot Size Settings")] [SerializeField]
    private int _maxLootSize = 20;

    [Header("Loot  Settings")] [SerializeField, InlineEditor]
    private List<SlotItem> _lootSlotInItems = new();

    [SerializeField] private int _lootOpeningDuration;

    [SerializeField] private Loot _activeLoot = null;

    private LootGenerator _lootGenerator;
    private ILootableHandler _lootableHandler;
    private ILootSlotHandler _lootSlotHandler;
    private ISlotManager _slotManager;
    private bool _isLooting;
    private float _lootTimer;

    public int LootSize => _maxLootSize;
    public float LootOpeningDuration => _lootOpeningDuration;
    public List<SlotItem> ActiveLootInItems => _lootSlotInItems;
    public Loot ActiveLoot => _activeLoot;
    public bool IsLooting => _isLooting;

    public delegate void LootTimerDelegate(int duration, float timer, bool isEnable);

    public static event LootTimerDelegate OnLootTimerUpdateEvent;

    [Inject]
    private void Constructor(LootGenerator generator, ILootableHandler lootableHandler,
        ILootSlotHandler lootSlotHandler, ISlotManager slotManager)
    {
        _lootGenerator = generator;
        _lootableHandler = lootableHandler;
        _lootSlotHandler = lootSlotHandler;
    }

    private void Start()
    {
        _lootTimer = _lootOpeningDuration;
    }

    public void StartLooting(Action action)
    {
        _isLooting = true;
        _lootTimer -= Time.deltaTime;

        OnLootTimerUpdateEvent?.Invoke(_lootOpeningDuration, _lootTimer, true);

        if (_lootTimer <= 0)
        {
            _lootTimer = _lootOpeningDuration;
            OnLootTimerUpdateEvent?.Invoke(_lootOpeningDuration, _lootTimer, false);
            action?.Invoke();
        }
    }

    public void StopLooting()
    {
        _isLooting = false;
        _lootTimer = _lootOpeningDuration;
        
        OnLootTimerUpdateEvent?.Invoke(_lootOpeningDuration, _lootTimer, false);
    }

    public void GenerateLoot(List<LootSettings> lootSettings, List<ItemData> datas, Richness richness)
    {
        int totalSize = _lootableHandler.LootSize;
        int currentSize = datas.Count;

        lootSettings.Shuffle();
        lootSettings.ForEach(r =>
        {
            if (currentSize >= totalSize) return;

            int remainingSize = totalSize - currentSize;
            int lootToAdd = Mathf.Min(r.Size, remainingSize);

            _lootGenerator.GenerateLoot(r.Type, richness, lootToAdd, ref datas).Shuffle();
            currentSize += lootToAdd;
        });
    }

    public void ResetLoot(List<LootSettings> lootSettings, List<ItemData> datas, Richness richness)
    {
        if (datas == null)
            return;

        datas.Clear();
        GenerateLoot(lootSettings, datas, richness);
    }

    public void SetActiveLootItems(Loot loot)
    {
        if (loot.LootInDatas == null)
        {
            return;
        }

        if (_lootSlotInItems != null)
        {
            _lootSlotInItems.ForEach(r =>
            {
                if (r.gameObject != null)
                    Destroy(r.gameObject);

                r.Slot.SetSlotStatus(SlotStatus.Empty);
            });

            _lootSlotInItems.Clear();
        }

        AddItemToLootWindow(loot);
    }

    public void RemoveItemFromLootWindow(ItemData data, ref List<ItemData> datas)
    {
        SlotItem slotItem = _lootSlotInItems.FirstOrDefault(r => r.Data == data);

        if (_lootSlotInItems.Contains(slotItem) && datas.Contains(data))
        {
            _lootSlotInItems.Remove(slotItem);
            datas.Remove(data);

            Destroy(slotItem.gameObject);
        }
    }

    private void AddItemToLootWindow(Loot loot)
    {
        _activeLoot = loot;
        _lootSlotHandler.FillLootSlots(loot.LootInDatas);
    }
}