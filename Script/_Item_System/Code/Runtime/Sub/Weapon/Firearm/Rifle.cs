using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Weapon.Firearm
{
    [CreateAssetMenu(fileName = "New_Rifle", menuName = "_Project_Plan_B/Create Item/Weapon/Firearm/Rifle")]
    public sealed class Rifle : FirearmData
    {
        public override FirearmType FirearmType => FirearmType.Rifle;
    }
}