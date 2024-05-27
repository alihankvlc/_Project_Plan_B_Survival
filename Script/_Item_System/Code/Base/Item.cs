using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project_Plan_B_Item_System.Code.Base
{
    public enum ItemType
    {
        Consumable,
        Weapon
    }
    public enum ConsumableType
    {
        Medkit,
    }

    public enum WeaponType
    {
        Melee,
        Firearm
    }
    public abstract class Item : ScriptableObject
    {
        [Header("Display Settings")]
        [SerializeField] private string _displayName;
        [SerializeField] private string _displayDescription;
        [SerializeField] private Sprite _icon;

        [Header("Data Settings")]
        [SerializeField] private int _dataId;
        [SerializeField, ReadOnly] public virtual ItemType _itemType { get; protected set; }
        [SerializeField] private bool _isStackable;

#if UNITY_EDITOR
        [SerializeField, Multiline, Space] private string _Editor_Description;
#endif

        public int Id => _dataId;
        public string Name => _displayName;
        public string Description => _displayDescription;
        public bool Stackable => _isStackable;
        public Sprite Icon => _icon;
    }
}