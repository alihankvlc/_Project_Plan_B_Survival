using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

namespace _Zombie_System_.Runtime.Common
{
    public enum ZombieStateType
    {
        Idle,
        Patrol,
        Detection,
        Chase,
        Attack
    }

    public class ZombieBehaviour : MonoBehaviour
    {
        [SerializeField] private ZombieStateType _activeStateType;

        [Header("Combat Settings")] [SerializeField]
        private float _attackCheckRadius;

        [Header("Patrol Settings")] [SerializeField]
        private float _patrolRadius;

        [SerializeField] private LayerMask _patrolLayerMask;

        [Header("Detection Settings")] [SerializeField]
        private float _noiseDetectionRange;

        [SerializeField] private float _detectionToAttackDelay = 3;

        [SerializeField] private LayerMask _canListenLayer;

        private StateMachine _stateMachine = new();

        private IdleState _idleState;
        private PatrolState _patrolState;
        private DetectionState _detectionState;
        private ChaseState _chaseState;
        private AttackState _attackState;

        private const int STATE_MIN_DURATION = 3;
        private const int STATE_MAX_DURATION = 5;

        private const float AI_WALK_SPEED = 0.35f;
        private const float AI_CHASE_SPEED = 3f;

        public int IdleStateChangeDuration => Random.Range(STATE_MIN_DURATION, STATE_MAX_DURATION);

        public float IdleStateTimer => _idleState.StateTimer;
        public float PatrolStateTimer => _patrolState.StateTimer;
        public float DetectionToAttackDelay => _detectionToAttackDelay;
        public float PatrolRadius => _patrolRadius;
        public float DetectionRange => _noiseDetectionRange;
        public float ChaseSpeed => AI_CHASE_SPEED;
        public float WalkSpeed => AI_WALK_SPEED;
        public float AttackRange => _attackCheckRadius;

        public LayerMask CanListenLayer => _canListenLayer;
        public ZombieStateType ActiveStateType => _activeStateType;

        [HideInInspector] public Animator Animator;
        [HideInInspector] public NavMeshAgent Ai;

        public readonly int SPEED_HASH_ID = Animator.StringToHash("Speed");
        public readonly int CHASE_HASH_ID = Animator.StringToHash("OnChase");
        public readonly int ATTACK_HASH_ID = Animator.StringToHash("OnAttack");

        private void Start()
        {
            Animator = GetComponent<Animator>();
            Ai = GetComponent<NavMeshAgent>();

            _idleState = new(_stateMachine, this);
            _patrolState = new(_stateMachine, this);
            _detectionState = new(_stateMachine, this);
            _chaseState = new(_stateMachine, this);
            _attackState = new(_stateMachine, this);


            _stateMachine.AddState(_idleState);
            _stateMachine.AddState(_patrolState);
            _stateMachine.AddState(_detectionState);
            _stateMachine.AddState(_chaseState);
            _stateMachine.AddState(_attackState);

            _stateMachine.SetState<IdleState>();

            Vector2 randomPointInCircle = Random.insideUnitCircle * _patrolRadius;
            Vector3 randomPoint = new Vector3(transform.position.x + randomPointInCircle.x, 0,
                randomPointInCircle.y + transform.position.z);
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        public void SetStateType(ZombieStateType type)
        {
            _activeStateType = type;
        }

        public void Attack()
        {
            Collider[] colliders = Physics.OverlapSphere(AttackRangeSpherPosition(), _attackCheckRadius);

            if (colliders != null && colliders.Length > 0)
            {

            }
        }

        private Vector3 AttackRangeSpherPosition()
        {
            return transform.position + transform.up + transform.forward;
        }

        private void OnDrawGizmos()
        {
            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, Vector3.up, _patrolRadius);

            Handles.color = Color.cyan;
            Handles.DrawWireDisc(transform.position, Vector3.up, _noiseDetectionRange);

            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(AttackRangeSpherPosition(), _attackCheckRadius);
        }
    }
}