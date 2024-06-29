using UnityEngine;

namespace _Zombie_System_.Runtime.Common
{
    public class DetectionState : ZombieStateBase
    {
        public DetectionState(StateMachine stateMachine, ZombieBehaviour behaviour) : base(stateMachine, behaviour)
        {
        }

        public override void OnStateEnter()
        {
            StateTimer = Zombie.DetectionToAttackDelay;
            Zombie.SetStateType(ZombieStateType.Detection);

            Zombie.Ai.ResetPath();
            Zombie.Ai.isStopped = true;
        }

        public override void OnStateTick()
        {
            StateTimer -= Time.deltaTime;

            if (StateTimer <= 0)
            {
                if (!base.Detection(out Vector3 targetPosition))
                {
                    StateMachine.SetState<IdleState>();
                    return;
                }

                StateMachine.SetState<ChaseState>();
            }
        }

        public override void OnStateExit()
        {
            Zombie.Ai.isStopped = false;
        }
    }
}