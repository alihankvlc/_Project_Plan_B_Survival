using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Weapon.Firearm
{
    [CreateAssetMenu(fileName = "New_Bow", menuName = "_Project_Plan_B/Create Item/Weapon/Firearm/Bow")]
    public sealed class Bow : FirearmData
    {
        public override FirearmType FirearmType => FirearmType.Bow;
    }
}