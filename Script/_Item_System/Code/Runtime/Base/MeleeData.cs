using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Base
{
    public abstract class MeleeData : WeaponData
    {
        public override WeaponType WeaponType => WeaponType.Melee;
        [SerializeField, ReadOnly] public virtual MeleeType MeleeType { get; protected set; }
    }
}