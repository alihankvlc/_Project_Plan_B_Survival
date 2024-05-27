using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Base
{
    public abstract class AmmoData : ItemData
    {
        [Header("Ammo Settings")]
        [SerializeField] private int _damageBonus;
        [SerializeField] private int _speed;
        public override ItemType _itemType => ItemType.Ammo;
        [SerializeField, ReadOnly] public virtual AmmoType AmmoType { get; protected set; }

        public int DamageBonus => _damageBonus;
        public int Speed => _speed;

        public void Set_Item_Ammo_Damage_Bonus(int bonus)
        {
            _damageBonus = bonus;
        }

        public void Set_Item_Ammo_Speed(int speed)
        {
            _speed = speed;
        }
    }
}