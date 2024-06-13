using _Input_System_.Code.Runtime;
using UnityEngine;
using Zenject;

namespace _Player_System_.Runtime.Common
{
    [RequireComponent(typeof(Animator), typeof(CharacterController))]
    public sealed class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")] [SerializeField]
        private float _walkSpeed = 2f;

        [SerializeField] private float _runSpeed = 5f;
        [SerializeField] private float _crouchSpeed = 1.25f;
        [SerializeField] private float _rotationSpeed = 10f;

        [Header("Ground & Gravity Settings")] [SerializeField]
        private bool _isGrounded = false;

        [SerializeField] private float _gravity = -25f;
        [SerializeField] private float _groundRadius = 0.2f;
        [SerializeField] private float _groundOffset = -0.14f;
        [SerializeField] private LayerMask _groundLayerMask;

        [Header("Animation Settings")] [SerializeField]
        private float _animDampTime = 3f;

        [Header("Crouch  Settings")] [SerializeField]
        private bool _crouchObstacleDetection = false;

        [SerializeField] private float _obstacleDetectionOffset = 1.35f;
        [SerializeField] private float _obstacleDetectionRadius = 0.15f;
        [SerializeField] private Vector3 _crouchCenter = new Vector3(0f, 0.57f, 0f);
        [SerializeField] private float _crouchHeight = 1f;
        [SerializeField] private LayerMask _checkObstacleAboveMask;

        private Vector3 _verticalVelocity;
        private Vector3 _defaultCenter;

        private float _targetSpeed;
        private float _defaultHeight;

        private Animator _animator;
        private CharacterController _controller;

        private IPlayerInputHandler _input;


        private const string MOVE_HASH_ENTRY = "OnMove";
        private readonly int MOVE_HASH_ID = Animator.StringToHash(MOVE_HASH_ENTRY);

        private const string GROUNDED_HASH_ENTRY = "OnGround";
        private readonly int GROUNDED_HASH_ID = Animator.StringToHash(GROUNDED_HASH_ENTRY);

        private const string CROUCH_HASH_ENTRY = "OnCrouch";
        private readonly int CROUCH_HASH_ID = Animator.StringToHash(CROUCH_HASH_ENTRY);

        [Inject]
        public void Construct(IPlayerInputHandler inputProvider)
        {
            _input = inputProvider;
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _controller = GetComponent<CharacterController>();

            _defaultCenter = _controller.center;
            _defaultHeight = _controller.height;
        }

        private void Update()
        {
            _controller.center = _input.Crouch || CheckObstacleAbove() ? _crouchCenter : _defaultCenter;
            _controller.height = _input.Crouch || CheckObstacleAbove() ? _crouchHeight : _defaultHeight;

            HandleMovement();
            HandleRotation();
            CheckGrounded();

            if (_input.Crouch)
                CheckObstacleAbove();
        }

        private void HandleMovement()
        {
            _controller.Move((GetMoveDirection() * GetMoveSpeed() + ApplyGravity()) * Time.deltaTime);
        }

        private void HandleRotation()
        {
            if (GetMoveDirection() != Vector3.zero)
                RotateTowardsMovement();
        }

        private void RotateTowardsMovement()
        {
            Quaternion targetRotation = Quaternion.LookRotation(GetMoveDirection());
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        private Vector3 ApplyGravity()
        {
            _verticalVelocity.y = _isGrounded && _verticalVelocity.y < 0
                ? -2f
                : _verticalVelocity.y + _gravity * Time.deltaTime;
            return _verticalVelocity;
        }

        private bool CheckGrounded()
        {
            Vector3 spherePosition = GetSpherePosition(_groundOffset);

            _isGrounded = Physics.CheckSphere(spherePosition, _groundRadius, _groundLayerMask,
                QueryTriggerInteraction.Ignore);
            _animator.SetBool(GROUNDED_HASH_ID, _isGrounded);

            return _isGrounded;
        }

        private bool CheckObstacleAbove()
        {
            Vector3 spherePosition = GetSpherePosition(_obstacleDetectionOffset);

            _crouchObstacleDetection =
                Physics.CheckSphere(spherePosition, _obstacleDetectionRadius, _checkObstacleAboveMask);

            return _crouchObstacleDetection;
        }

        private Vector3 GetMoveDirection()
        {
            Vector3 inputDirection = new(_input.Move.x, 0, _input.Move.y);
            return inputDirection;
        }

        private Vector3 GetSpherePosition(float offset)
        {
            return transform.position + Vector3.up * offset;
        }

        private float GetMoveSpeed()
        {
            _targetSpeed = _input.Move != Vector2.zero
                ? (_input.Crouch || CheckObstacleAbove() ? _crouchSpeed : (_input.Run ? _runSpeed : _walkSpeed))
                : 0f;

            _animator.SetFloat(MOVE_HASH_ID, _targetSpeed, _animDampTime, Time.deltaTime * 30f);
            _animator.SetBool(CROUCH_HASH_ID,
                _input.Crouch ? true : !CheckObstacleAbove() ? false : _animator.GetBool(CROUCH_HASH_ID));

            return _targetSpeed;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _isGrounded ? Color.yellow : Color.red;
            Gizmos.DrawWireSphere(GetSpherePosition(_groundOffset), _groundRadius);

            Gizmos.color = _crouchObstacleDetection ? Color.yellow : Color.red;
            Gizmos.DrawWireSphere(GetSpherePosition(_obstacleDetectionOffset), _obstacleDetectionRadius);
        }
    }
}