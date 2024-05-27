using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using UnityEngine;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Sub.Weapon.Firearm
{
    [CreateAssetMenu(fileName = "New_Shotgun", menuName = "_Project_Plan_B/Create Item/Weapon/Firearm/Shotgun")]
    public sealed class Shotgun : FirearmData
    {
        public override FirearmType FirearmType => FirearmType.Shotgun;
    }
}