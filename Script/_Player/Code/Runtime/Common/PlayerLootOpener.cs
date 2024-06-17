using System;
using _Loot_System_.Runtime;
using _Other_.Runtime.Code;
using UnityEngine;

namespace _Player_System_.Runtime.Common
{
    public class PlayerLootOpener : MonoBehaviour
    {
        [Header("Loot Settings")] [SerializeField]
        private float _openLootDistance = 1.5f;

        [SerializeField] private LayerMask _lootLayer;

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _lootLayer))
            {
                float remaingDistance = Vector3.Distance(hitInfo.point, transform.position);
                bool checkDistance = remaingDistance < _openLootDistance;
                if (checkDistance && hitInfo.collider.TryGetComponent(out Loot loot))
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        loot.OpenLoot();
                    }
                }
            }
        }
    }
}