using _Player_System_.Runtime.Common;
using _Stat_System.Runtime.Base;
using _Stat_System.Runtime.Sub;
using Sirenix.Utilities;
using UnityEngine;

namespace _Zombie_System_.Runtime.Common
{
    public class ChaseState : ZombieStateBase
    {
        public ChaseState(StateMachine stateMachine, ZombieBehaviour behaviour) : base(stateMachine, behaviour)
        {
        }

        public override void OnStateEnter()
        {
            Zombie.SetStateType(ZombieStateType.Chase);
            Zombie.Animator.SetBool(Zombie.CHASE_HASH_ID, true);

            Zombie.Ai.speed = Zombie.ChaseSpeed;
        }

        public override void OnStateTick()
        {
            if (base.Detection(out Vector3 targetPosition))
            {
                Zombie.Ai.SetDestination(targetPosition);

                Transform zombieTransform = Zombie.transform;
                Vector3 directionToTarget = (targetPosition - zombieTransform.position).normalized;

                Quaternion lookRotation = Quaternion.LookRotation(new(directionToTarget.x,
                    0, directionToTarget.z));

                zombieTransform.rotation = Quaternion.Slerp(zombieTransform.rotation, lookRotation,
                    Time.deltaTime * Zombie.Ai.angularSpeed);
            }
        }

        public override void OnStateExit()
        {
            Zombie.Animator.SetBool(Zombie.CHASE_HASH_ID, false);
        }
    }
}