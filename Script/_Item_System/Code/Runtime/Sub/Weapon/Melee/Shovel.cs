using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using UnityEngine;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Sub.Weapon.Melee
{
    [CreateAssetMenu(fileName = "New_Pistol", menuName = "_Project_Plan_B/Create Item/Weapon/Melee/Shovel")]
    public sealed class Shovel : MeleeData
    {
        public override MeleeType MeleeType => MeleeType.Shovel;
    }
}