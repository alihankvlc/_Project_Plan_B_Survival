using Zenject;
using UnityEngine;
using _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Slot_Settings;

namespace _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Common
{
    public class ToolBelt : MonoBehaviour
    {
        private IPlayerInventory _inventory;

        [Inject]
        public void Constructor(IPlayerInventory inventory)
        {
            _inventory = inventory;
        }
    }
}

