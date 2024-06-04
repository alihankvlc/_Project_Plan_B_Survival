using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Weapon.Melee
{
    [CreateAssetMenu(fileName = "New_Spear", menuName = "_Project_Plan_B/Create Item/Weapon/Melee/Spear")]
    public sealed class Spear : MeleeData
    {
        public override MeleeType MeleeType => MeleeType.Spear;
    }
}