using _Player_System_.Runtime.Common;
using Sirenix.Utilities;
using UnityEngine;

namespace _Zombie_System_.Runtime.Common
{
    public abstract class ZombieStateBase : IState
    {
        public StateMachine StateMachine { get; private set; }

        protected readonly ZombieBehaviour Zombie;

        public float StateTimer { get; protected set; }

        public ZombieStateBase(StateMachine stateMachine, ZombieBehaviour behaviour)
        {
            StateMachine = stateMachine;
            Zombie = behaviour;
        }

        public virtual void OnStateEnter()
        {
        }

        public virtual void OnStateTick()
        {
        }

        public virtual void OnStateExit()
        {
        }

        public virtual bool Detection(out Vector3 targetPosition)
        {
            Vector3 position = Zombie.transform.position;
            LayerMask layerMask = Zombie.CanListenLayer;

            Transform targetTransform = null;

            float range = Zombie.DetectionRange;
            float threshold = float.MaxValue;

            Collider[] colliders = Physics.OverlapSphere(position, range, layerMask);

            if (colliders.Length != 0)
            {
                foreach (var r in colliders)
                {
                    float distance = Vector3.Distance(position, r.transform.position);

                    if (r.TryGetComponent(out INoiseMaker noiseMaker))
                    {
                        float newThreshold = distance / noiseMaker.NoiseLevel;
                        if (newThreshold < threshold)
                        {
                            targetTransform = r.transform;
                            threshold = newThreshold;
                        }
                    }
                }
            }

            if (targetTransform != null)
            {
                targetPosition = targetTransform.position;
                return threshold <= 10;
            }

            targetPosition = Vector3.zero;
            return false;
        }
    }
}