using _Project_Plan_B_Survival_Input_System.Code;
using UnityEngine;
using Zenject;

namespace _Project_Plan_B_Player.Code
{
    [RequireComponent(typeof(Animator), typeof(CharacterController))]
    public sealed class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 720f;

        [Header("Ground & Gravity Settings")]
        [SerializeField] private bool _isGrounded = false;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _groundRadius = 0.14f;
        [SerializeField] private float _groundOffset = 0f;
        [SerializeField] private LayerMask _groundLayerMask;

        [Header("Animation Settings")]
        [SerializeField] private float _animDampTime = 0.1f;

        private Vector3 _verticalVelocity;

        private Animator _animator;
        private CharacterController _controller;

        private IPlayerInputProvider _input;

        private const string MOVE_HASH_ENTRY = "OnMove";
        private readonly int MOVE_HASH_ID = Animator.StringToHash(MOVE_HASH_ENTRY);

        private const string GROUNDED_HASH_ENTRY = "OnGround";
        private readonly int GROUNDED_HASH_ID = Animator.StringToHash(GROUNDED_HASH_ENTRY);

        [Inject]
        public void Construct(IPlayerInputProvider inputProvider)
        {
            _input = inputProvider;
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            HandleMovement();
            HandleRotation();
            CheckGrounded();
        }

        private void HandleMovement()
        {
            _controller.Move((GetMoveDirection() * _moveSpeed + ApplyGravity()) * Time.deltaTime);
        }

        private void HandleRotation()
        {
            if (_input.OnAim)
                RotateTowardsMouse();
            else if (GetMoveDirection() != Vector3.zero)
                RotateTowardsMovement();
        }

        private void RotateTowardsMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Vector3 direction = hitInfo.point - transform.position;
                direction.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }

        private void RotateTowardsMovement()
        {
            Quaternion targetRotation = Quaternion.LookRotation(GetMoveDirection());
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        private Vector3 ApplyGravity()
        {
            _verticalVelocity.y = _isGrounded && _verticalVelocity.y < 0 ? -2f : _verticalVelocity.y + _gravity * Time.deltaTime;
            return _verticalVelocity;
        }

        private bool CheckGrounded()
        {
            Vector3 spherePosition = transform.position + Vector3.up * _groundOffset;
            _isGrounded = Physics.CheckSphere(spherePosition, _groundRadius, _groundLayerMask, QueryTriggerInteraction.Ignore);
            _animator.SetBool(GROUNDED_HASH_ID, _isGrounded);
            return _isGrounded;
        }

        private Vector3 GetMoveDirection()
        {
            Vector3 moveDirection = new Vector3(_input.OnMove.x, 0, _input.OnMove.y);
            _animator.SetFloat(MOVE_HASH_ID, Vector3.ClampMagnitude(moveDirection, 1).magnitude, _animDampTime, Time.deltaTime * 30f);
            return moveDirection;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _isGrounded ? Color.yellow : Color.red;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * _groundOffset, _groundRadius);
        }
    }
}
