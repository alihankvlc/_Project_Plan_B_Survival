using UnityEngine;
using UnityEngine.AI;

namespace _Zombie_System_.Runtime.Common
{
    public class PatrolState : ZombieStateBase
    {
        public PatrolState(StateMachine stateMachine, ZombieBehaviour behaviour) : base(stateMachine, behaviour)
        {
        }

        public override void OnStateEnter()
        {
            Zombie.Ai.speed = Zombie.WalkSpeed;
            Zombie.SetStateType(ZombieStateType.Patrol);
            
            Zombie.Animator.SetFloat(Zombie.SPEED_HASH_ID, 1, 0.3f, Time.deltaTime * 15f);
        }

        public override void OnStateTick()
        {
            if (base.Detection(out Vector3 targetPosition))
            {
                StateMachine.SetState<DetectionState>();
                return;
            }

            if (Zombie.Ai.remainingDistance <= Zombie.Ai.stoppingDistance + 1f)
                StateMachine.SetState<IdleState>();
        }


        public override void OnStateExit()
        {
            Zombie.Ai.isStopped = false;
        }
    }
}