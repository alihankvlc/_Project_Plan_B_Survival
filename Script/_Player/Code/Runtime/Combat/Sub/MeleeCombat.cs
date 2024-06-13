using System;
using System.Collections;
using _Item_System_.Runtime.Base;
using _Other_.Runtime.Code;
using _Player_System_.Runtime.Combat.Base;
using _Player_System_.Runtime.Combat.Common;
using _Stat_System.Runtime.Base;
using _Stat_System.Runtime.Sub;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace _Player_System_.Runtime.Combat.Sub
{
    public class MeleeCombat : PlayerCombat<MeleeData>
    {
        public override CombatType Type => CombatType.Melee;
        [SerializeField, ReadOnly] private MeleeType _meleeType;

        [Header("Check Attack Settings")] [SerializeField]
        private float _checkAttackRadius;

        [SerializeField] private Vector3 _checkAttackOffset;
        [SerializeField] private LayerMask _layerMask;

        private IMeleeTriggerListener _meleeTriggerListener;

        [Inject]
        private void Constructor(IMeleeTriggerListener meleeTriggerListener)
        {
            _meleeTriggerListener = meleeTriggerListener;
        }

        protected override void Start()
        {
            base.Start();

            _meleeTriggerListener.OnMeleeEnableTrigger.AddListener(TriggerEnableCollider);
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void Attack()
        {
        }

        private void OnDrawGizmos()
        {
            Vector3 spherePos = (transform.position + transform.forward + transform.up) + _checkAttackOffset;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spherePos, _checkAttackRadius);
        }

        private void TriggerEnableCollider()
        {
            Vector3 spherePos = (transform.position + transform.forward + transform.up) + _checkAttackOffset;
            Collider[] colliders = Physics.OverlapSphere(spherePos, _checkAttackRadius, _layerMask);

            if (colliders == null || colliders.Length == 0)
            {
                return;
            }

            Transform closestTarget = null;
            float closestDistance = Mathf.Infinity;

            colliders.ForEach(r =>
            {
                float distance = Vector3.Distance(transform.position, r.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = r.transform;
                }
            });

            if (closestTarget != null && closestDistance < ActiveWeaponData.Range)
            {
                if (RaycastHandler.SendRay(out RaycastHit hitInfo, _layerMask))
                {
                    if (hitInfo.collider.TryGetComponent(out IDamageable damageable))
                        ApplyDamage(damageable);

                    return;
                }
                // else if (closestTarget.TryGetComponent(out IDamageable closestDamageable))
                // {
                //     ApplyDamage(closestDamageable);
                // }
            }
        }

        private void ApplyDamage(IDamageable damageable)
        {
            damageable.TakeDamage(ActiveWeaponData != null ? ActiveWeaponData.Damage : 1);
            ActiveSlot.UpdateDurability(-3);
            PlayerComponent.ImpulseSource.GenerateImpulse();
        }

        protected override void OnDestroy()
        {
            base.Update();
            _meleeTriggerListener.OnMeleeEnableTrigger.RemoveListener(TriggerEnableCollider);
        }
    }
}