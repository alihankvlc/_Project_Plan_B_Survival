using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Ammo
{
    [CreateAssetMenu(fileName = "New_Pistol_Bullet", menuName = "_Project_Plan_B/Create Item/Weapon/Firearm/Create Ammo/Pistol")]
    public sealed class PistolBullet : AmmoData
    {
        public override AmmoType AmmoType => AmmoType.PistolBullet;
    }
}