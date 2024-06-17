using System;
using System.Collections;
using System.Collections.Generic;
using _Input_System_.Code.Runtime;
using _Item_System_.Runtime.Common;
using _Loot_System_.Runtime;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public enum ItemFinderState
{
    Inactive,
    Searching,
    Cooldown
}

public enum PoolingType
{
    EnableObject,
    DisableObject
}

public sealed class PlayerItemFindScanController : MonoBehaviour
{
    [Header("State Info Settings")] [SerializeField]
    private ItemFinderState _itemFinderState = ItemFinderState.Inactive;

    [Header("Item Finder General Settings")] [SerializeField]
    private float _itemFinderUsageDuration = 5f;

    [SerializeField] private float _itemFinderRemoveObjectDuration = 2f;

    [SerializeField] private GameObject _itemFinderPrefab;

    private IPlayerInputHandler _playerInput;

    private float _itemFinderUsageTimer;
    private float _itemFinderRemoveObjectTimer;

    [Inject]
    private void Constructor(IPlayerInputHandler playerInput)
    {
        _playerInput = playerInput;
    }

    private void Start()
    {
        _itemFinderUsageTimer = _itemFinderUsageDuration;
        _itemFinderRemoveObjectTimer = _itemFinderRemoveObjectDuration;
    }

    private void Update()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        switch (_itemFinderState)
        {
            case ItemFinderState.Searching:
                StartSearching();
                break;
            case ItemFinderState.Cooldown:
                StartCooldown();
                break;
            case ItemFinderState.Inactive:
                SetActive();
                break;
        }
    }

    private void SetActive()
    {
        if (_playerInput.FindItem)
        {
            _itemFinderState = ItemFinderState.Searching;
            _itemFinderPrefab.SetActive(true);
        }
    }

    private void StartSearching()
    {
        _itemFinderRemoveObjectTimer -= Time.deltaTime;

        if (_itemFinderRemoveObjectTimer <= 0)
        {
            _itemFinderRemoveObjectTimer = _itemFinderRemoveObjectDuration;
            _itemFinderState = ItemFinderState.Cooldown;

            _itemFinderPrefab.SetActive(false);
        }
    }

    private void StartCooldown()
    {
        _itemFinderUsageTimer -= Time.deltaTime;

        if (_itemFinderUsageTimer <= 0)
        {
            _itemFinderUsageTimer = _itemFinderUsageDuration;
            _itemFinderState = ItemFinderState.Inactive;
        }
    }
}