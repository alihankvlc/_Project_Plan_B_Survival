using Sirenix.OdinInspector;
using UnityEngine;

namespace _Item_System_.Runtime.Base
{

    public abstract class WeaponData : ItemData, IEquippable
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

        protected virtual int ATTACK_HASH_ID { get; set; }
        protected virtual int EQUIP_HASH_ID { get; set; }

        public void EquipWeapon(Animator animator, ref bool isEquipped)
        {
            if (!isEquipped) animator.SetBool(EQUIP_HASH_ID, true);
            isEquipped = true;
        }

        public void UnequipWeapon(Animator animator, ref bool isEquipped)
        {
            if (isEquipped) animator.SetBool(EQUIP_HASH_ID, false);
            isEquipped = false;
        }

        public void Attack(ref Animator animator, ref bool isEquipped)
        {
            if (isEquipped) animator.Play(ATTACK_HASH_ID);
        }
        public void Set_Item_Max_Durability(int durability) => _durability = durability;
    }
}