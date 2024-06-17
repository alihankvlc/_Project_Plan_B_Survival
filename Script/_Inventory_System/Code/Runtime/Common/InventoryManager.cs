using System;
using System.Collections.Generic;
using System.Linq;
using _Input_System_.Code.Runtime;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Inventory_System_.Code.Runtime.UI;
using _Loot_System_.Runtime;
using _Other_.Runtime.Code;
using _UI_Managment_.Runtime.Common;
using _UI_Managment_.Runtime.Menu.Common;
using UnityEngine;
using Zenject;

namespace _Inventory_System_.Code.Runtime.Common
{
    public sealed class InventoryManager : MonoBehaviour
    {
        [Space, Header("Inventory Settings")] [SerializeField]
        private Inventory _inventory;

        [SerializeField] private int _inventoryMaxWeight;

        private InventoryWeight _weightHandler;
        private InventoryWindowManager _inventoryWindowManager;

        [Inject]
        private void Constructor(InventoryWeight inventoryWeight)
        {
            _weightHandler = inventoryWeight;
            _weightHandler.SetMaxWeight(_inventoryMaxWeight);
        }
    }
}