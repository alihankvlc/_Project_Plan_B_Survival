using System;
using _Equipment_System_.Runtime.Sub;
using _Game_Managment.Runtime;
using _Inventory_System_.Code.Runtime.Common;
using _Inventory_System_.Code.Runtime.UI;
using _Item_System_.Runtime.Common;
using _Loot_System_.Runtime;
using Cinemachine;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

namespace _Player_System_.Runtime.Common
{
    public interface IPlayerComponent
    {
        public Animator Animator { get; }
        public CinemachineImpulseSource ImpulseSource { get; }
        public Transform PlayerTransform { get; }
        public bool UIEnable();
    }

    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(WeaponEquipmentController))]
    [RequireComponent(typeof(ConsumableEquipmentController))]
    [RequireComponent(typeof(PlayerStatHandler))]
    [RequireComponent(typeof(ItemGathering))]
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class PlayerBehaviour : MonoBehaviour, IPlayerComponent //TODO: Güncellicem...
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CinemachineImpulseSource _cinemachineImpulse;
        public Animator Animator => _animator;
        public CinemachineImpulseSource ImpulseSource => _cinemachineImpulse;
        public Transform PlayerTransform => transform;

        public ILootWindowHandler LootWindowHandler { get; private set; }
        public ILootableHandler LootableHandler { get; private set; }
        public IWindowFromInventoryHandler InventoryWindowHandler { get; private set; }

        [Inject]
        private void Consturctor(IWindowFromInventoryHandler inventoryWindow, ILootWindowHandler lootWindow,
            ILootableHandler lootableHandler)
        {
            InventoryWindowHandler = inventoryWindow;
            LootWindowHandler = lootWindow;
            LootableHandler = lootableHandler;
        }

        public bool UIEnable()
        {
            return InventoryWindowHandler.IsWindowEnable || LootWindowHandler.IsWindowEnable ||
                   LootableHandler.IsLooting;
        }
    }
}