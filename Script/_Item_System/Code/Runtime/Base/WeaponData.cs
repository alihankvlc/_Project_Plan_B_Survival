using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Item_System_.Runtime.Base
{
    public abstract class WeaponData : ItemData /*, IWeaponEquippable*/
    {
        [Header("General Settings")] [SerializeField]
        private int _level;

        [SerializeField] private int _durability;
        [SerializeField] private int _damage;
        [SerializeField] private float _range;
        [SerializeField] private float _attackSpeed;

        [SerializeField, ReadOnly] public virtual CombatType WeaponType { get; protected set; }
        public override ItemType _itemType => ItemType.Weapon;
        public CombatType Weapon_Type => WeaponType;
        public int Level => _level;
        public int Durability => _durability;
        public int Damage => _damage;
        public float Range => _range;
        public float AttackSpeed => _attackSpeed;

        public virtual int ATTACK_HASH_ID { get; protected set; }
        public virtual int EQUIP_HASH_ID { get; protected set; }

        public void Set_Item_Max_Durability(int durability) => _durability = durability;
    }
}