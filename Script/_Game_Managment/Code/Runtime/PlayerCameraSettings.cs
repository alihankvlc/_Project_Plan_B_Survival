using System.Collections;
using System.Collections.Generic;
using _Input_System_.Code.Runtime;
using _Inventory_System_.Code.Runtime.Common;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace _Game_Managment.Runtime
{
    public class PlayerCameraSettings : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        [Header("Zoom Settings")]
        [SerializeField] private float _zoomAmountChangeSpeed;

        [Inject] IPlayerInputHandler _input;

        private float _zoomAmount;

        private int ORTO_MIN_ZOOM_SIZE = 2;
        private int ORTO_MAX_ZOOM_SIZE = 5;

        private void Start()
        {
            _zoomAmount = _virtualCamera.m_Lens.OrthographicSize;
        }

        private void Update()
        {
            if (InventoryManager.Instance.InventoryEnable) return;
            
            _zoomAmount = _input.MouseScroll > 0 ? Mathf.Clamp(_zoomAmount - _zoomAmountChangeSpeed, ORTO_MIN_ZOOM_SIZE, ORTO_MAX_ZOOM_SIZE) :
            _input.MouseScroll < 0 ? Mathf.Clamp(_zoomAmount + _zoomAmountChangeSpeed, ORTO_MIN_ZOOM_SIZE, ORTO_MAX_ZOOM_SIZE) : _zoomAmount;

            _virtualCamera.m_Lens.OrthographicSize = _zoomAmount;
        }
    }
}

