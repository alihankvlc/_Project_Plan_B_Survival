using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Equipment_System_.Runtime.Sub;
using _Input_System_.Code.Runtime;
using _Inventory_System_.Code.Runtime.Common;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Item_System_.Runtime.Base;
using _Other_.Runtime.Code;
using _Player_System_.Runtime.Combat.Common;
using _Player_System_.Runtime.Common;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Player_System_.Runtime.Combat.Base
{
    public abstract class PlayerCombat<T> : MonoBehaviour where T : WeaponData
    {
        [SerializeField, ReadOnly] protected T ActiveWeaponData;
        [SerializeField, ReadOnly] protected SlotItem ActiveSlot;
        [SerializeField, ReadOnly] public virtual CombatType Type { get; protected set; }

        protected IPlayerComponent PlayerComponent;
        protected IPlayerInputHandler PlayerInputHandler;
        protected IToolBeltHandler ToolbeltHandler;
        protected IMeleeTriggerListener MeleeTriggerListener;

        private LayerMask _groundLayerMask;

        private readonly int AIM_HASH_ID = Animator.StringToHash("OnAim");

        private const string ITEM_NEEDED_REPAIR_TEXT = "The item needs repair!";
        private const string GROUND_LAYER_NAME = "Ground";
        
        [Inject]
        private void Constructor(IPlayerInputHandler playerInputHandler, IPlayerComponent component)
        {
            PlayerInputHandler = playerInputHandler;
            PlayerComponent = component;
        }

        protected virtual void Start()
        {
            _groundLayerMask = LayerMask.GetMask(GROUND_LAYER_NAME);
            WeaponEquipmentController.OnEquipmentWeapon += EquippedWeaponData;
        }

        protected abstract void Attack();

        protected virtual void Update()
        {
            PlayerComponent.Animator.SetBool(AIM_HASH_ID, PlayerInputHandler.Aim);

            if (PlayerInputHandler.Aim)
            {
                RotateTowardsMouse();
            }

            if (ActiveSlot == null || ActiveWeaponData == null)
            {
                return;
            }

            if (PlayerInputHandler.Attack)
            {
                if (ActiveSlot.SlotInItemDurability > 0)
                {
                    PlayerComponent.Animator.Play(ActiveWeaponData.ATTACK_HASH_ID);
                    Attack();

                    return;
                }

                LoggingUtility.Log.Message(this, ITEM_NEEDED_REPAIR_TEXT,
                    Color.red, false, true);
            }
        }

        protected virtual void EquippedWeaponData(SlotItem slotItem)
        {
            ActiveSlot = slotItem;
            ActiveWeaponData = slotItem != null ? slotItem.Data as T : null;
        }

        protected float GetAnimationClipDuration()
        {
            return ActiveWeaponData != null ? GetAnimationClip(ActiveWeaponData.ATTACK_HASH_ID).length : 0f;
        }

        private AnimationClip GetAnimationClip(int hashID)
        {
            AnimatorController animatorController =
                PlayerComponent.Animator.runtimeAnimatorController as AnimatorController;

            AnimationClip existingClip = null;

            animatorController.layers.ForEach(layer =>
            {
                layer.stateMachine.states.ForEach(childAnimatorState =>
                {
                    if (childAnimatorState.state.nameHash == hashID)
                    {
                        existingClip = childAnimatorState.state.motion as AnimationClip;
                    }
                });
            });

            return existingClip;
        }

        private void RotateTowardsMouse() //TODO: PlayerAttackBehaviour sınıfına taşıycam.
        {
            if (RaycastHandler.SendRay(out RaycastHit hitInfo, _groundLayerMask))
            {
                Vector3 direction = hitInfo.point - transform.position;
                direction.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.forward = targetRotation * Vector3.forward;
            }
        }

        protected virtual void OnDestroy()
        {
            WeaponEquipmentController.OnEquipmentWeapon -= EquippedWeaponData;
        }
    }
}