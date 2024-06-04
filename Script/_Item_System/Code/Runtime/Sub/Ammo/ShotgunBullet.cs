using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Ammo
{
    [CreateAssetMenu(fileName = "New_Shotgun_Bullet", menuName = "_Project_Plan_B/Create Item/Weapon/Firearm/Create Ammo/Shotgun")]
    public sealed class ShotgunBullet : AmmoData
    {
        public override AmmoType AmmoType => AmmoType.ShotgunBullet;
    }
}