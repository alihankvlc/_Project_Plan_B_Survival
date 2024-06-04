using Sirenix.OdinInspector;
using UnityEngine;

namespace _Item_System_.Runtime.Base
{
    public abstract class MeleeData : WeaponData
    {
        public override WeaponType WeaponType => WeaponType.Melee;
        [SerializeField, ReadOnly] public virtual MeleeType MeleeType { get; protected set; }
    }
}