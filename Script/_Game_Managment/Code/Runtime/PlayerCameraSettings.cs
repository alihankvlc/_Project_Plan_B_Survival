using System.Collections;
using System.Collections.Generic;
using _Input_System_.Code.Runtime;
using _Inventory_System_.Code.Runtime.Common;
using _Inventory_System_.Code.Runtime.UI;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace _Game_Managment.Runtime
{
    public class PlayerCameraSettings : MonoBehaviour
    {
        [Header("General Settings")] [SerializeField]
        private CinemachineVirtualCamera _virtualCamera;

        [Header("Zoom Settings")] [SerializeField]
        private float _zoomAmountChangeSpeed;

        private IPlayerInputHandler _playerInput;
        private IWindowFromInventoryHandler _inventoryWindow;

        private float _zoomAmount;

        private int ORTO_MIN_ZOOM_SIZE = 2;
        private int ORTO_MAX_ZOOM_SIZE = 6;

        [Inject]
        private void Constructor(IPlayerInputHandler input, IWindowFromInventoryHandler inventoryWindow)
        {
            _playerInput = input;
            _inventoryWindow = inventoryWindow;
        }

        private void Start()
        {
            _zoomAmount = _virtualCamera.m_Lens.OrthographicSize;
        }

        private void Update()
        {
            if (_inventoryWindow.IsWindowEnable) return;

            _zoomAmount = _playerInput.MouseScroll > 0
                ? Mathf.Clamp(_zoomAmount - _zoomAmountChangeSpeed, ORTO_MIN_ZOOM_SIZE, ORTO_MAX_ZOOM_SIZE)
                : _playerInput.MouseScroll < 0
                    ? Mathf.Clamp(_zoomAmount + _zoomAmountChangeSpeed, ORTO_MIN_ZOOM_SIZE, ORTO_MAX_ZOOM_SIZE)
                    : _zoomAmount;

            _virtualCamera.m_Lens.OrthographicSize = _zoomAmount;
        }
    }
}