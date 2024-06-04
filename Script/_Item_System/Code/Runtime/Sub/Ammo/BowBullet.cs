using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Ammo
{
    [CreateAssetMenu(fileName = "New_Bow_Bullet", menuName = "_Project_Plan_B/Create Item/Weapon/Firearm/Create Ammo/Bow")]
    public sealed class BowBullet : AmmoData
    {
        public override AmmoType AmmoType => AmmoType.BowBullet;
    }
}