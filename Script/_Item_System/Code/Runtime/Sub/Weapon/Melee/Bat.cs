using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using UnityEngine;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Sub.Weapon.Melee
{
    [CreateAssetMenu(fileName = "New_Pistol", menuName = "_Project_Plan_B/Create Item/Weapon/Melee/Bat")]
    public sealed class Bat : MeleeData
    {
        public override MeleeType MeleeType => MeleeType.Bat;

    }
}