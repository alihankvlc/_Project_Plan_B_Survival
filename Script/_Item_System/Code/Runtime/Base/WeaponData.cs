using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Base
{
    public abstract class WeaponData : ItemData
    {
        [Header("General Settings")]
        [SerializeField] private int _level;
        [SerializeField] private int _durability;
        [SerializeField] private int _damage;
        [SerializeField] private int _range;

        [SerializeField, ReadOnly] public virtual WeaponType WeaponType { get; protected set; }
        public override ItemType _itemType => ItemType.Weapon;
        public WeaponType Weapon_Type => WeaponType;
        public int Level => _level;
        public int Durability => _durability;
        public int Damage => _damage;
        public int Range => _range;
    }
}