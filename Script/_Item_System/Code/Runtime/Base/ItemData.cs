using _Database_System_.Code.Runtime;
using _Item_System_.Runtime.Database;
using _Crafting_System_.Runtime.Common;
using _Other_.Runtime.Code;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Item_System_.Runtime.Base
{
    [Flags]
    public enum ObtainableType
    {
        None = 0,
        Crafting = 1 << 0,
        Looting = 1 << 1,
        Trading = 1 << 2,
        Mining = 1 << 3,
    }

    public enum ScrappableType
    {
        None,
    }

    public enum ItemType
    {
        None,
        Consumable,
        Weapon,
        Ammo,
        Resources
    }

    public enum ConsumableType
    {
        HealthItems,
        Food,
        Drink,
    }

    public enum Rarity
    {
        Common, // Sıradan
        Uncommon, // Olağanüstü
        Rare, // Nadir
        Epic, // Epik
        Legendary // Efsanevi
    }

    public enum CombatType
    {
        Melee,
        Firearm,
    }

    public enum FirearmType
    {
        Rifle,
        Pistol,
        Shotgun,
        Bow
    }

    public enum AmmoType
    {
        RifleBullet,
        PistolBullet,
        BowBullet,
        ShotgunBullet,
    }

    public enum MeleeType
    {
        None,
        Ax,
        Bat,
        Shovel,
        Knife,
        Spear
    }

    public enum HealthItemType
    {
        Bandage,
        Antibiotics,
        Painkillers,
        Vitamins
    }

    public enum MaterialType
    {
        Iron
    }

    public enum SmeltResult
    {
        None,
        Brass,
        Glass,
        Clay,
        Iron,
        Lead,
        Stone
    }

    public enum MineableType
    {
        None,
        Coal,
        Stone,
        Iron
    }

    public abstract class ItemData : DeletableScriptableObject, IData
    {
#if UNITY_EDITOR
        [SerializeField, Multiline, Space] private string _Editor_Description;

        protected virtual void OnValidate()
        {
            if (!_isStackable) IsNotStackableItem();

            if (!CanMineable(out MineableType type))
                _mineableType = MineableType.None;

            if (!CanCrafting())
            {
                _craftingRequirement = null;
                _craftingDuration = 0f;
            }
        }

        [Button("Destory")]
        private void DestroyButton() => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));

        protected override void OnDestroy() => RemoveItemDataFromDatabase();
        private void IsNotStackableItem() => _stackCapacity = 1;
        private void RemoveItemDataFromDatabase() => ItemDatabase.Instance.RemoveItem(_dataId);
#endif

        [Header("Display Settings")] [SerializeField]
        private string _displayName;

        [SerializeField] private string _displayDescription;
        [SerializeField] private Sprite _icon;

        [Header("Data Settings")] [SerializeField]
        private int _dataId;

        [SerializeField] private float _weight;
        [SerializeField] private int _sellPrice;

        [SerializeField, ReadOnly]
        public virtual ItemType _itemType { get; protected set; } //FIXME : Fix public - protected

        [SerializeField] private bool _isStackable;
        [SerializeField] private Rarity _rarity;

        [SerializeField, ShowIf("@_isStackable")]
        private int _stackCapacity;

        [SerializeField] private bool _isScrappable;
        [SerializeField] private ObtainableType _obtainableType;

        [SerializeField, ShowIf(nameof(CanCrafting))]
        private CraftingRequirement[] _craftingRequirement;

        [SerializeField, ShowIf(nameof(CanCrafting))]
        private float _craftingDuration;

        [SerializeField, ShowIf(nameof(CanMineable))]
        private MineableType _mineableType;

        public int Id => _dataId;
        public string DisplayName => _displayName;
        public string DisplayDescription => _displayDescription;
        public Sprite Icon => _icon;
        public ItemType ItemType => _itemType;
        public Rarity RarityType => _rarity;
        public MineableType MineableType => _mineableType;
        public bool Stackable => _isStackable;
        public bool IsSrappable => _isScrappable;
        public int StackCapacity => _stackCapacity;
        public float Weight => _weight;
        public float CraftingDuration => _craftingDuration;
        public int SellPrice => _sellPrice;
        public ObtainableType ObtainableType => _obtainableType;
        public CraftingRequirement[] CraftingRequirement => _craftingRequirement;

        public bool CanCrafting()
        {
            return (_obtainableType & ObtainableType.Crafting) != 0;
        }

        public bool IsLootable()
        {
            return (_obtainableType & ObtainableType.Looting) != 0;
        }

        public bool CanMineable(out MineableType mineableType)
        {
            mineableType = _mineableType;
            return (_obtainableType & ObtainableType.Mining) != 0;
        }

        public void Set_Item_Id(int id) => _dataId = id;
        public void Set_Item_Display_Name(string name) => _displayName = name;
        public void Set_Item_Display_Description(string description) => _displayDescription = description;
        public void Set_Item_Icon(Sprite icon) => _icon = icon;
        public void Set_Item_Type(ItemType itemType) => _itemType = itemType;
        public void Set_Item_IsStackable(bool param) => _isScrappable = param;
        public void Set_Item_Stack_Capacity(int amount) => _stackCapacity = amount;
        public void Set_Item_Weight(float weight) => _weight = weight;
        public void Set_Item_IsSrappable(bool param) => _isScrappable = param;
        public void Set_Item_ObtainableType(ObtainableType type) => _obtainableType = type;
        public void Set_Item_Sell_Price(int price) => _sellPrice = price;
        public void Set_Item_Crafting_Duration(float duration) => _craftingDuration = duration;
    }
}