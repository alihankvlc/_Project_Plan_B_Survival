using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project_Plan_B_Survival_Input_System.Code
{
    public interface IPlayerInputProvider
    {
        public Vector2 Move { get; }
        public bool Aim { get; }
        public bool Inventory { get; }
    }
    public sealed class InputManager : MonoBehaviour, IPlayerInputProvider
    {
        [SerializeField] private PlayerInput _playerInput;

        private InputActionMap _action_Map;

        private InputAction _moveInputAction;
        private InputAction _aimInputAction;
        private InputAction _inventoryInputAction;


        private const string INPUT_MOVE_ENTRY = "Move";
        private const string INPUT_AIM_ENTRY = "Aim";
        private const string INPUT_INVENTORY_ENTRY = "Inventory";

        public Vector2 Move { get; private set; }

        public bool Aim { get; private set; }
        public bool Inventory { get; private set; }

        private void Awake()
        {
            _action_Map = _playerInput.currentActionMap;

            _moveInputAction = _action_Map.FindAction(INPUT_MOVE_ENTRY);
            _aimInputAction = _action_Map.FindAction(INPUT_AIM_ENTRY);
            _inventoryInputAction = _action_Map.FindAction(INPUT_INVENTORY_ENTRY);

        }
        private void OnEnable()
        {
            _action_Map.Enable();

            _moveInputAction.performed += (InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();
            _moveInputAction.canceled += (InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();

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

            _aimInputAction.performed -= (InputAction.CallbackContext context) => Aim = context.ReadValueAsButton();
            _aimInputAction.canceled -= (InputAction.CallbackContext context) => Aim = context.ReadValueAsButton();

        }
    }
}


