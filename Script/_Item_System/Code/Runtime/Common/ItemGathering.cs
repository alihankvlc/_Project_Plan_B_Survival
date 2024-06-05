using _Inventory_System_.Code.Runtime.Common;
using UnityEngine;
using Zenject;

namespace _Item_System_.Runtime.Common
{
    public class ItemGathering : MonoBehaviour
    {
        [Header("Gathering Settings")]
        [SerializeField] private float _gatheringDistance = 3f;
        [SerializeField] private LayerMask _gatheringLayerMask;

        private Camera _mainCamera;
        private IItemManagement _itemManagement;

        [Inject]
        private void Constructor(IItemManagement itemManagment)
        {
            _itemManagement = itemManagment;
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _gatheringLayerMask))
            {
                float distance = Vector3.Distance(transform.position, hitInfo.point);

                if (distance < _gatheringDistance && hitInfo.collider.TryGetComponent(out IItemContainer container))
                {
                    if (Input.GetKeyDown(KeyCode.E) && container != null)
                        container.AddItemToInventory(_itemManagement);
                }
            }
        }
    }
}

