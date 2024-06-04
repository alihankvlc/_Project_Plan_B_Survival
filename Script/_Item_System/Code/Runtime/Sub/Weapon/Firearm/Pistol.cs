using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Weapon.Firearm
{
    [CreateAssetMenu(fileName = "New_Pistol", menuName = "_Project_Plan_B/Create Item/Weapon/Firearm/Pistol")]
    public sealed class Pistol : FirearmData
    {
        public override FirearmType FirearmType => FirearmType.Pistol;
    }
}