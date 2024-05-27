using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project_Plan_B_Survival_Input_System.Code
{
    public interface IPlayerInputProvider
    {
        public Vector2 OnMove { get; }
        public bool OnAim { get; }
    }
    public sealed class InputManager : MonoBehaviour, IPlayerInputProvider
    {
        [SerializeField] private PlayerInput _playerInput;

        private InputActionMap _action_Map;

        private InputAction _moveInputAction;
        private InputAction _aimInputAction;

        private const string INPUT_MOVE_ENTRY = "Move";
        private const string INPUT_AIM_ENTRY = "Aim";

        public Vector2 OnMove { get; private set; }

        public bool OnAim { get; private set; }

        private void Awake()
        {
            _action_Map = _playerInput.currentActionMap;

            _moveInputAction = _action_Map.FindAction(INPUT_MOVE_ENTRY);
            _aimInputAction = _action_Map.FindAction(INPUT_AIM_ENTRY);

        }
        private void OnEnable()
        {
            _action_Map.Enable();

            _moveInputAction.performed += (InputAction.CallbackContext context) => OnMove = context.ReadValue<Vector2>();
            _moveInputAction.canceled += (InputAction.CallbackContext context) => OnMove = context.ReadValue<Vector2>();

            _aimInputAction.performed += (InputAction.CallbackContext context) => OnAim = context.ReadValueAsButton();
            _aimInputAction.canceled += (InputAction.CallbackContext context) => OnAim = context.ReadValueAsButton();
        }

        private void OnDisable()
        {
            _action_Map.Disable();

            _moveInputAction.performed -= (InputAction.CallbackContext context) => OnMove = context.ReadValue<Vector2>();
            _moveInputAction.canceled -= (InputAction.CallbackContext context) => OnMove = context.ReadValue<Vector2>();

            _aimInputAction.performed -= (InputAction.CallbackContext context) => OnAim = context.ReadValueAsButton();
            _aimInputAction.canceled -= (InputAction.CallbackContext context) => OnAim = context.ReadValueAsButton();
        }
    }
}


