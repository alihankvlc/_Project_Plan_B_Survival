using UnityEngine;
using UnityEngine.AI;

namespace _Zombie_System_.Runtime.Common
{
    public class IdleState : ZombieStateBase
    {
        private readonly Vector3 _startPosition;

        public IdleState(StateMachine stateMachine, ZombieBehaviour behaviour) : base(stateMachine, behaviour)
        {
            _startPosition = Zombie.transform.position;
        }

        public override void OnStateEnter()
        {
            Zombie.Animator.SetFloat(Zombie.SPEED_HASH_ID, 0, 0.3f, Time.deltaTime * 15f);

            StateTimer = Zombie.IdleStateChangeDuration;
            Zombie.SetStateType(ZombieStateType.Idle);
        }

        public override void OnStateTick()
        {
            StateTimer -= Time.deltaTime;

            if (base.Detection(out Vector3 targetPosition))
            {
                StateMachine.SetState<DetectionState>();
                return;
            }
            
            Vector3 position = GetRandomPatrolPoint(Zombie.transform.position, Zombie.PatrolRadius);

            if (StateTimer <= 0)
            {
                bool isFarFromStart = Vector3.Distance(Zombie.Ai.transform.position, _startPosition) >= 15;

                Zombie.Ai.SetDestination(isFarFromStart ? _startPosition :
                    IsPathValid(position) ? position : _startPosition);

                StateMachine.SetState<PatrolState>();
            }
        }

        public override void OnStateExit()
        {
        }

        private bool IsPathValid(Vector3 targetPosition)
        {
            if (!Zombie.Ai.isOnNavMesh || !Zombie.Ai.enabled)
            {
                return false;
            }

            NavMeshPath path = new NavMeshPath();
            Zombie.Ai.CalculatePath(targetPosition, path);

            return path.status == NavMeshPathStatus.PathComplete;
        }

        private Vector3 GetRandomPatrolPoint(Vector3 center, float radius)
        {
            Vector2 randomPoint2D = Random.insideUnitCircle * radius;
            return new Vector3(center.x + randomPoint2D.x, center.y, center.z + randomPoint2D.y);
        }
    }
}