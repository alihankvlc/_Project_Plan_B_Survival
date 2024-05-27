using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using UnityEngine;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Sub.Weapon.Firearm
{
    [CreateAssetMenu(fileName = "New_Rifle", menuName = "_Project_Plan_B/Create Item/Weapon/Firearm/Rifle")]
    public sealed class Rifle : FirearmData
    {
        public override FirearmType FirearmType => FirearmType.Rifle;
    }
}