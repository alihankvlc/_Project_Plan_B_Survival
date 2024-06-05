using UnityEngine;
using UnityEngine.InputSystem;

namespace _Input_System_.Code.Runtime
{
    public interface IPlayerInputHandler
    {
        public Vector2 Move { get; }
        public bool Aim { get; }
        public bool Inventory { get; }
        public bool Run { get; }
        public bool Crouch { get; }
        public float MouseScroll { get; }
    }
    public sealed class InputManager : MonoBehaviour, IPlayerInputHandler
    {
        [SerializeField] private PlayerInput _playerInput;

        private InputActionMap _action_Map;

        private InputAction _moveInputAction;
        private InputAction _runInputAction;
        private InputAction _crouchInputAction;
        private InputAction _aimInputAction;
        private InputAction _inventoryInputAction;
        private InputAction _mouseScrollInputAction;


        private const string INPUT_MOVE_ENTRY = "Move";
        private const string INPUT_RUN_ENTRY = "Run";
        private const string INPUT_CROUCH_ENTRY = "Crouch";
        private const string INPUT_AIM_ENTRY = "Aim";
        private const string INPUT_INVENTORY_ENTRY = "Inventory";
        private const string INPUT_MOUSE_SCROLL_ENTRY = "MouseScroll";

        public Vector2 Move { get; private set; }

        public bool Aim { get; private set; }
        public bool Inventory { get; private set; }
        public bool Run { get; private set; }
        public bool Crouch { get; private set; }
        public float MouseScroll { get; private set; }


        private void Awake()
        {
            _action_Map = _playerInput.currentActionMap;

            _moveInputAction = _action_Map.FindAction(INPUT_MOVE_ENTRY);
            _runInputAction = _action_Map.FindAction(INPUT_RUN_ENTRY);
            _crouchInputAction = _action_Map.FindAction(INPUT_CROUCH_ENTRY);
            _aimInputAction = _action_Map.FindAction(INPUT_AIM_ENTRY);
            _inventoryInputAction = _action_Map.FindAction(INPUT_INVENTORY_ENTRY);
            _mouseScrollInputAction = _action_Map.FindAction(INPUT_MOUSE_SCROLL_ENTRY);

        }
        private void OnEnable()
        {
            _action_Map.Enable();

            _moveInputAction.performed += (InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();
            _moveInputAction.canceled += (InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();

            _mouseScrollInputAction.performed += (InputAction.CallbackContext context) => MouseScroll = context.ReadValue<float>();
            _mouseScrollInputAction.canceled += (InputAction.CallbackContext context) => MouseScroll = context.ReadValue<float>();

            _runInputAction.performed += (InputAction.CallbackContext context) => Run = context.ReadValueAsButton();
            _runInputAction.canceled += (InputAction.CallbackContext context) => Run = context.ReadValueAsButton();

            _crouchInputAction.performed += (InputAction.CallbackContext context) => Crouch = context.ReadValueAsButton();
            _crouchInputAction.canceled += (InputAction.CallbackContext context) => Crouch = context.ReadValueAsButton();

            _aimInputAction.performed += (InputAction.CallbackContext context) => Aim = context.ReadValueAsButton();
            _aimInputAction.canceled += (InputAction.CallbackContext context) => Aim = context.ReadValueAsButton();

        }

        private void Update()
        {
            Inventory = _inventoryInputAction.WasPressedThisFrame();
        }

        private void OnDisable()
        {
            _action_Map.Disable();

            _moveInputAction.performed -= (InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();
            _moveInputAction.canceled -= (InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();

            _runInputAction.performed -= (InputAction.CallbackContext context) => Run = context.ReadValueAsButton();
            _runInputAction.canceled -= (InputAction.CallbackContext context) => Run = context.ReadValueAsButton();

            _mouseScrollInputAction.performed -= (InputAction.CallbackContext context) => MouseScroll = context.ReadValue<float>();
            _mouseScrollInputAction.canceled -= (InputAction.CallbackContext context) => MouseScroll = context.ReadValue<float>();

            _crouchInputAction.performed -= (InputAction.CallbackContext context) => Crouch = context.ReadValueAsButton();
            _crouchInputAction.canceled -= (InputAction.CallbackContext context) => Crouch = context.ReadValueAsButton();

            _aimInputAction.performed -= (InputAction.CallbackContext context) => Aim = context.ReadValueAsButton();
            _aimInputAction.canceled -= (InputAction.CallbackContext context) => Aim = context.ReadValueAsButton();

        }
    }
}


