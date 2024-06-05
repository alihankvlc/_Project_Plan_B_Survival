using _Database_System_.Code.Runtime;
using _Item_System_.Runtime.Database;
using _Other_.Runtime.Code;
using Sirenix.OdinInspector;
using System;
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
    }

    public enum ScrappableType
    {
        None,
    }

    public enum ItemType
    {
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

    public enum WeaponType
    {
        Melee,
        Firearm,
    }

    public enum ResourcesType
    {
        Coal,
        Wood,
        Stone,
        Iron
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
    
    public interface IEquippable
    {
        public void EquipWeapon(Animator animator, ref bool isEquipped);
        public void UnequipWeapon(Animator animator, ref bool isEquipped);
        public void Attack(ref Animator animator, ref bool isEquipped);
    }

    public abstract class ItemData : DeletableScriptableObject, IData
    {

#if UNITY_EDITOR
        [SerializeField, Multiline, Space] private string _Editor_Description;

        [Button("Destory")]
        private void DestroyButton()
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
        }
        protected override void OnDestroy()
        {
            RemoveItemDataFromDatabase();
        }

        private void OnValidate()
        {
            if (!_isStackable) _stackCapacity = 1;
        }

        private void RemoveItemDataFromDatabase()
        {
            ItemDatabase.Instance.RemoveItem(_dataId);
        }
#endif

        [Header("Display Settings")]
        [SerializeField] private string _displayName;
        [SerializeField] private string _displayDescription;
        [SerializeField] private Sprite _icon;

        [Header("Data Settings")]
        [SerializeField] private int _dataId;
        [SerializeField] private float _weight;
        [SerializeField] private int _sellPrice;
        [SerializeField, ReadOnly] public virtual ItemType _itemType { get; protected set; }
        [SerializeField] private bool _isStackable;
        [SerializeField, ShowIf("@_isStackable")] private int _stackCapacity;
        [SerializeField] private bool _isScrappable;
        [SerializeField] private ObtainableType _obtainableType;

        public int Id => _dataId;
        public string DisplayName => _displayName;
        public string Description => _displayDescription;
        public Sprite Icon => _icon;
        public ItemType ItemType => _itemType;
        public bool Stackable => _isStackable;
        public bool IsSrappable => _isScrappable;
        public int StackCapacity => _stackCapacity;
        public float Weight => _weight;
        public int SellPrice => _sellPrice;
        public ObtainableType ObtainableType => _obtainableType;

        public void Set_Item_Id(int id)
        {
            _dataId = id;
        }

        public void Set_Item_Display_Name(string name)
        {
            _displayName = name;
        }

        public void Set_Item_Display_Description(string description)
        {
            _displayDescription = description;
        }

        public void Set_Item_Icon(Sprite icon)
        {
            _icon = icon;
        }

        public void Set_Item_Type(ItemType itemType)
        {
            _itemType = itemType;
        }

        public void Set_Item_IsStackable(bool param)
        {
            _isScrappable = param;
        }

        public void Set_Item_Stack_Capacity(int amount)
        {
            _stackCapacity = amount;
        }
        public void Set_Item_Weight(float weight)
        {
            _weight = weight;
        }

        public void Set_Item_IsSrappable(bool param)
        {
            _isScrappable = param;
        }

        public void Set_Item_ObtainableType(ObtainableType type)
        {
            _obtainableType = type;
        }

        public void Set_Item_Sell_Price(int price)
        {
            _sellPrice = price;
        }

    }
}