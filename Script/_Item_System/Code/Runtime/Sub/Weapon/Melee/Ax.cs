using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Weapon.Melee
{
    [CreateAssetMenu(fileName = "New_Ax", menuName = "_Project_Plan_B/Create Item/Weapon/Melee/Ax")]
    public sealed class Ax : MeleeData
    {
        public override MeleeType MeleeType => MeleeType.Ax;
        
        public override int EQUIP_HASH_ID => Animator.StringToHash("Equip_Ax");
        public override int ATTACK_HASH_ID => Animator.StringToHash("Attack_Ax");
    }
}