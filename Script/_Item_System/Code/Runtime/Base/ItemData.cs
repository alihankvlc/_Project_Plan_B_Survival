using _Project_Plan_B_Common;
using _Project_Plan_B_Survival_Database.Code;
using _Project_Plan_B_Survival_Item_System.Runtime.Database;
using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEngine;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Base
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
        Medkit,
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
        [SerializeField, ReadOnly] public virtual ItemType _itemType { get; protected set; }
        [SerializeField] private bool _isStackable;
        [SerializeField, ShowIf("@_isStackable")] private int _stackCapacity;
        [SerializeField] private bool _isScrappable;
        [SerializeField] private ObtainableType _obtainableType;

        public int Id => _dataId;
        public string Name => _displayName;
        public string Description => _displayDescription;
        public Sprite Icon => _icon;
        public ItemType ItemType => _itemType;
        public bool IsStackable => _isStackable;
        public bool IsSrappable => _isScrappable;
        public int StackCapacity => _stackCapacity;
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

        public void Set_Item_IsSrappable(bool param)
        {
            _isScrappable = param;
        }

        public void Set_Item_ObtainableType(ObtainableType type)
        {
            _obtainableType = type;
        }

    }
}