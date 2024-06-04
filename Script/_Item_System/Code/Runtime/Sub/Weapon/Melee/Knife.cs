using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Weapon.Melee
{
    [CreateAssetMenu(fileName = "New_Ax", menuName = "_Project_Plan_B/Create Item/Weapon/Melee/Knife")]
    public sealed class Knife : MeleeData
    {
        public override MeleeType MeleeType => MeleeType.Knife;

    }
}