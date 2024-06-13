using System;
using _Equipment_System_.Runtime.Sub;
using _Game_Managment.Runtime;
using _Item_System_.Runtime.Common;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Player_System_.Runtime.Common
{
    public interface IPlayerComponent
    {
        public Animator Animator { get; }
        public CinemachineImpulseSource ImpulseSource { get; }
        public Transform PlayerTransform { get; }
    }

    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(WeaponEquipmentController))]
    [RequireComponent(typeof(ConsumableEquipmentController))]
    [RequireComponent(typeof(PlayerStatHandler))]
    [RequireComponent(typeof(PlayerCameraSettings))]
    [RequireComponent(typeof(ItemGathering))]
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class PlayerBehaviour : MonoBehaviour, IPlayerComponent //TODO: Güncellicem...
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private CinemachineImpulseSource _cinemachineImpulse;
        public Animator Animator => _animator;
        public CinemachineImpulseSource ImpulseSource => _cinemachineImpulse;
        public Transform PlayerTransform => transform;
    }
}