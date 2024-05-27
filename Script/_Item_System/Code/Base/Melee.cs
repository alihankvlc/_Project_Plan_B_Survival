using System.Collections;
using UnityEngine;

namespace _Project_Plan_B_Item_System.Code.Base
{
    [CreateAssetMenu(fileName = "New_Melee", menuName = "_Project_Plan_B/Create Item/Weapon/Melee")]
    public class Melee : Weapon_Item
    {
        public override WeaponType WeaponType => WeaponType.Melee;
    }
}