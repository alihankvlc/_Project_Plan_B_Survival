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
        [SerializeField] private float _aimSpeed = 1.30f;

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

        private float _hVelocityInput = 0f;
        private float _vVelocityInput = 0f;

        private Animator _animator;
        private CharacterController _controller;

        private IPlayerInputHandler _input;


        private readonly int MOVE_HASH_ID = Animator.StringToHash("OnMove");
        private readonly int GROUNDED_HASH_ID = Animator.StringToHash("OnGround");
        private readonly int CROUCH_HASH_ID = Animator.StringToHash("OnCrouch");
        private readonly int H_VELOCITY_HASH_ID = Animator.StringToHash("H_Velocity");
        private readonly int V_VELOCITY_HASH_ID = Animator.StringToHash("V_Velocity");

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
            Vector3 forwardDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * Vector3.forward;
            Vector3 rightDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * Vector3.right;

            Vector2 moveInput = _input.Move.normalized;
            Vector3 moveDirection = moveInput.x * rightDirection + moveInput.y * forwardDirection;

            moveDirection.y = 0;

            _hVelocityInput = _input.Move != Vector2.zero ? moveDirection.x : 0.0f;
            _vVelocityInput = _input.Move != Vector2.zero ? moveDirection.z : 0.0f;
            
            _animator.SetFloat(H_VELOCITY_HASH_ID, _hVelocityInput, _animDampTime, Time.deltaTime * 30f);
            _animator.SetFloat(V_VELOCITY_HASH_ID, _vVelocityInput, _animDampTime, Time.deltaTime * 30f);

            if (!_input.Aim)
            {
                _animator.SetFloat(MOVE_HASH_ID, _targetSpeed, _animDampTime, Time.deltaTime * 30f);
                _animator.SetBool(CROUCH_HASH_ID,
                    _input.Crouch ? true :
                    !CheckObstacleAbove() ? false : _animator.GetBool(CROUCH_HASH_ID));
            }
            
            
            return new Vector3(_input.Move.x, 0, _input.Move.y);
        }


        private Vector3 GetSpherePosition(float offset)
        {
            return transform.position + Vector3.up * offset;
        }

        private float GetMoveSpeed()
        {
            _targetSpeed = _input.Move != Vector2.zero
                ? ((_input.Crouch || CheckObstacleAbove() && !_input.Aim)
                    ? _crouchSpeed
                    : (_input.Run && !_input.Aim ? _runSpeed : _input.Aim ? _aimSpeed : _walkSpeed))
                : 0f;

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