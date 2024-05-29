using UnityEngine;

namespace _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Common
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private bool _isEnable;
        [SerializeField] private Inventory _inventory;

        public bool IsOpen => _isEnable;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                _isEnable = !_isEnable;
                ToggleInventory(_isEnable);
            }
        }
        public void ToggleInventory(bool isEnable)
        {
            _inventory.gameObject.SetActive(isEnable);
        }
    }

}