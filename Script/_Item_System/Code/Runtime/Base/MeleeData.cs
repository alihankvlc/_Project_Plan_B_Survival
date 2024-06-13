using Sirenix.OdinInspector;
using UnityEngine;

namespace _Item_System_.Runtime.Base
{
    public abstract class MeleeData : WeaponData
    {
        [SerializeField, ReadOnly]
        public virtual MeleeType MeleeType { get; protected set; }
        [SerializeField, ReadOnly]
        public override CombatType WeaponType => CombatType.Melee;

    }
}