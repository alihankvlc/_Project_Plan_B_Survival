using _Project_Plan_B_Survival_Input_System.Code;
using UnityEngine;
using Zenject;

namespace _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Common
{
    public class InventoryManager : MonoBehaviour
    {
        [Space, Header("Inventory Window Settings")]
        [SerializeField] private bool _isEnable;
        [SerializeField] private GameObject _inventoryWindow;

        [Inject] IPlayerInputProvider _input;
        public bool IsOpen => _isEnable;

        private void Update()
        {
            if (_input.Inventory)
            {
                _isEnable = !_isEnable;
                ToggleInventory(_isEnable);
            }
        }
        public void ToggleInventory(bool isEnable)
        {
            _inventoryWindow.gameObject.SetActive(isEnable);
        }
    }

}