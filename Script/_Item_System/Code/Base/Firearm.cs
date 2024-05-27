using _Project_Plan_B_Item_System.Code.Base;
using System.Collections;
using UnityEngine;

namespace _Project_Plan_B_Item_System.Code.Base
{
    [CreateAssetMenu(fileName = "New_Firearm", menuName = "_Project_Plan_B/Create Item/Weapon/Firearm")]
    public class Firearm : Weapon_Item
    {
        public override WeaponType WeaponType => WeaponType.Firearm;
    }
}