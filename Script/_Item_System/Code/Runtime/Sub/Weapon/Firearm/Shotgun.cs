using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Weapon.Firearm
{
    [CreateAssetMenu(fileName = "New_Shotgun", menuName = "_Project_Plan_B/Create Item/Weapon/Firearm/Shotgun")]
    public sealed class Shotgun : FirearmData
    {
        public override FirearmType FirearmType => FirearmType.Shotgun;
    }
}