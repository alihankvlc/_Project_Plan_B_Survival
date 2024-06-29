using UnityEngine;
using UnityEngine.AI;

namespace _Zombie_System_.Runtime.Common
{
    public class AttackState : ZombieStateBase
    {
        public AttackState(StateMachine stateMachine, ZombieBehaviour behaviour) : base(stateMachine, behaviour)
        {
        }

        public override void OnStateEnter()
        {
            Zombie.SetStateType(ZombieStateType.Attack);
            Zombie.Animator.SetBool(Zombie.ATTACK_HASH_ID, true);
        }

        public override void OnStateTick()
        {

        }

        public override void OnStateExit()
        {
            Zombie.Animator.SetBool(Zombie.ATTACK_HASH_ID, false);
        }
    }
}