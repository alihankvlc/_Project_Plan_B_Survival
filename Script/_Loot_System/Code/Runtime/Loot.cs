using System;
using System.Collections.Generic;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Item_System_.Runtime.Base;
using _Player_System_.Runtime.Common;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using Zenject;
using Random = UnityEngine.Random;

namespace _Loot_System_.Runtime
{
    public sealed class Loot : MonoBehaviour //TODO: Base sınıf haline getirip dolaplar,kapaklar vs vs vs gibi ayıracam..
    {
        [Header("Loot Settings")] [SerializeField]
        private List<LootSettings> _lootSettings = new();

        [SerializeField] private Richness _lootRichness;

        [Header("Loot In Item Settings")] [SerializeField, ReadOnly]
        private List<ItemData> _lootInDatas = new();

        private ILootableHandler _lootableHandler;
        private ILootWindowHandler _lootWindowHandler;

        private bool _isStartingLooting = false;
        private bool _isOpenLoot = false;

        public bool IsStartingLooting => _isStartingLooting;

        [SerializeField] private Transform _temp;
        public List<ItemData> LootInDatas => _lootInDatas;

        [Inject]
        private void Constructor(ILootableHandler lootableHandler, ILootWindowHandler lootWindowHandler)
        {
            _lootableHandler = lootableHandler;
            _lootWindowHandler = lootWindowHandler;
        }

        private void Start()
        {
            _lootableHandler.GenerateLoot(_lootSettings, _lootInDatas, _lootRichness);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && _isStartingLooting)
                StopLooting();

            if (_isStartingLooting)
                _lootableHandler.StartLooting(ShowLoot);
        }

        public void OpenLoot()
        {
            if (_isOpenLoot)
            {
                ShowLoot();
                return;
            }

            _isStartingLooting = true;
        }

        public void StopLooting()
        {
            _isStartingLooting = false;
            _lootableHandler.StopLooting();
        }

        private void ShowLoot()
        {
            _lootableHandler.SetActiveLootItems(this);
            _lootWindowHandler.OpenLootWindow();

            _temp.DOLocalRotate(new Vector3(-110,0,0), 0.5f);
            _isOpenLoot = true;
            _isStartingLooting = false;
        }

        public void RemoveInLootItem(ItemData itemData)
        {
            _lootableHandler.RemoveItemFromLootWindow(itemData, ref _lootInDatas);
        }
    }
}