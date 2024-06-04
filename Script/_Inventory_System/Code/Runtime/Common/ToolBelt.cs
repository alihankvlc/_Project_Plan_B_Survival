using Zenject;
using UnityEngine;

namespace _Inventory_System_.Code.Runtime.Common
{
    public sealed class ToolBelt : MonoBehaviour
    {
        private IPlayerInventory _inventory;

        [Inject]
        public void Constructor(IPlayerInventory inventory)
        {
            _inventory = inventory;
        }
    }
}

