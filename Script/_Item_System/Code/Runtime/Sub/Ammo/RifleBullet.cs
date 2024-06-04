using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Ammo
{
    [CreateAssetMenu(fileName = "New_Rifle_Bullet", menuName = "_Project_Plan_B/Create Item/Weapon/Firearm/Create Ammo/Rifle")]
    public sealed class RifleBullet : AmmoData
    {
        public override AmmoType AmmoType => AmmoType.RifleBullet;
    }
}