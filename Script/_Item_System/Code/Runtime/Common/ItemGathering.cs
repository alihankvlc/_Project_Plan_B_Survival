using System;
using _Inventory_System_.Code.Runtime.Common;
using _Other_.Runtime.Code;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Item_System_.Runtime.Common
{
    public sealed class ItemGathering : MonoBehaviour
    {
        [Header("Gathering Settings")] 
        [SerializeField] private float _gatheringDistance = 3f;
        [SerializeField] private LayerMask _gatheringLayerMask;
        [SerializeField] private float _midHeightThreshold = 1f; // Mid ve Down yükseklik sınırları
        [SerializeField] private float _downHeightThreshold = 0.5f;

        private IItemManagement _itemManagement;
        private Animator _animator; // TODO: Animator kullandığım sınıflara inject edip kullanıcam.

        private readonly int PICKUP_MID_HASH_ID = Animator.StringToHash("Pickup_Mid");
        private readonly int PICKUP_DOWN_HASH_ID = Animator.StringToHash("Pickup_Down");

        [Inject]
        private void Constructor(IItemManagement itemManagment)
        {
            _itemManagement = itemManagment;
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (RaycastHandler.SendRay(out RaycastHit hitInfo, Mathf.Infinity, _gatheringLayerMask))
            {
                float distance = Vector3.Distance(transform.position, hitInfo.point);

                if (distance < _gatheringDistance && hitInfo.collider.TryGetComponent(out IItemContainer container))
                {
                    if (Input.GetKeyDown(KeyCode.E) && container != null)
                    {
                        RotateTowardsItem(hitInfo.point, () =>
                        {
                            container.AddItemToInventory(_itemManagement);
                            PlayPickupAnimation(hitInfo.point);
                        });
                    }
                }
            }
        }

        private void RotateTowardsItem(Vector3 targetPosition, Action onComplete)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.DORotateQuaternion(targetRotation, 0.2f).OnComplete(() => onComplete());
        }

        private void PlayPickupAnimation(Vector3 itemPosition)
        {
            float itemHeight = itemPosition.y - transform.position.y;

            int triggerId = itemHeight > _midHeightThreshold
                ? PICKUP_MID_HASH_ID
                : (itemHeight > _downHeightThreshold ? PICKUP_DOWN_HASH_ID : PICKUP_DOWN_HASH_ID);

            _animator.SetTrigger(triggerId);
        }
    }
}
