using _Item_System_.Runtime.Base;
using _Player_System_.Runtime.Combat.Base;
using UnityEngine;

namespace _Player_System_.Runtime.Combat.Sub
{
    public class RangedCombat : PlayerCombat<FirearmData>
    {
        public override CombatType Type => CombatType.Firearm;


        protected override void Attack()
        {
        }
    }
}