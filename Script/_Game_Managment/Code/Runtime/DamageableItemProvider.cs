using System;
using System.Collections;
using System.Collections.Generic;
using _Inventory_System_.Code.Runtime.Common;
using _Stat_System.Runtime.Base;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public sealed class DamageableItemProvider : MonoBehaviour, IDamageable, IBreakable
{
    [SerializeField] private int _resourceCapacity;
    [SerializeField, ReadOnly] private int _resourceAmount;
    [SerializeField, ReadOnly] private int _calculaterThreshold;
    [SerializeField] private string _thisName;

    public int tempDamage;

    private IItemManagement _itemManagment;

    private const float CALCULATER_THRESHOLD_PERCENT = 0.05f;

    [SerializeField, ReadOnly] private int _damageCalculater;

    public int CurrentValue => _resourceAmount;
    public int BaseValue => _resourceCapacity;
    public string ItemName => _thisName;

    [Inject]
    private void Constructor(IItemManagement itemManagment)
    {
        _itemManagment = itemManagment;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(tempDamage);
        }
    }

    private void Start()
    {
        _resourceAmount = _resourceCapacity;
        _calculaterThreshold = Mathf.RoundToInt(_resourceCapacity * CALCULATER_THRESHOLD_PERCENT);
    }

    public void TakeDamage(int amount)
    {
        _resourceAmount -= amount;
        _damageCalculater += amount;

        if (_resourceAmount <= 0)
        {
            Break();
            return;
        }

        if (_damageCalculater >= _calculaterThreshold)
        {
            int itemCount = CalculateItemCount(amount);
            GiveItem(4, itemCount);
        }
    }

    public void Break()
    {
        Destroy(gameObject);
    }

    private int CalculateItemCount(int damageAmount)
    {
        int itemCount = 0;
        int itemBonus = 0;

        while (_damageCalculater >= _calculaterThreshold)
        {
            itemCount++;

            if (_damageCalculater >= _calculaterThreshold)
                itemBonus++;

            _damageCalculater -= _calculaterThreshold;
        }

        _damageCalculater = 0;
        int finalItemCount = itemCount + (itemBonus > 1 ? itemBonus : 0);

        return Mathf.Max(1, finalItemCount);
    }

    private void GiveItem(int itemId, int itemCount)
    {
        _itemManagment.AddItemToInventory(itemId, itemCount);
    }
}