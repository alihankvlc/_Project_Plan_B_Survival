using System.Collections;
using UnityEngine;

namespace _Project_Plan_B_Item_System.Code.Base
{
    public class Weapon_Item : Item
    {
        [SerializeField] private int _damage;
        public override ItemType _itemType => ItemType.Weapon;
        public virtual WeaponType WeaponType { get; protected set; }
    }
}