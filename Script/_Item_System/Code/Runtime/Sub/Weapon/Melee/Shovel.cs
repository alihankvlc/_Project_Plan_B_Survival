using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Weapon.Melee
{
    [CreateAssetMenu(fileName = "New_Shovel", menuName = "_Project_Plan_B/Create Item/Weapon/Melee/Shovel")]
    public sealed class Shovel : MeleeData
    {
        public override MeleeType MeleeType => MeleeType.Shovel;
    }
}