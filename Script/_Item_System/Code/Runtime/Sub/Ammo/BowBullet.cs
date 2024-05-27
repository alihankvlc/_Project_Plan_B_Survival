using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using UnityEngine;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Sub.Ammo
{
    [CreateAssetMenu(fileName = "New_Bow_Bullet", menuName = "_Project_Plan_B/Create Item/Weapon/Firearm/Create Ammo/Bow")]
    public sealed class BowBullet : AmmoData
    {
        public override AmmoType AmmoType => AmmoType.BowBullet;
    }
}