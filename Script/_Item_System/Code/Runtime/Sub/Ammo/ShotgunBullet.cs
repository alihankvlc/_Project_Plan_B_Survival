using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using UnityEngine;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Sub.Ammo
{
    [CreateAssetMenu(fileName = "New_Shotgun_Bullet", menuName = "_Project_Plan_B/Create Item/Weapon/Firearm/Create Ammo/Shotgun")]
    public sealed class ShotgunBullet : AmmoData
    {
        public override AmmoType AmmoType => AmmoType.ShotgunBullet;
    }
}