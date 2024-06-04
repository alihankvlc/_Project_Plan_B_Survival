using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Weapon.Melee
{
    [CreateAssetMenu(fileName = "New_Bat", menuName = "_Project_Plan_B/Create Item/Weapon/Melee/Bat")]
    public sealed class Bat : MeleeData
    {
        public override MeleeType MeleeType => MeleeType.Bat;

    }
}