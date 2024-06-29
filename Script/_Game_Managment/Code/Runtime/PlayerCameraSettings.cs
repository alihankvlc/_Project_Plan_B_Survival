using System;
using System.Collections;
using System.Collections.Generic;
using _Input_System_.Code.Runtime;
using _Inventory_System_.Code.Runtime.Common;
using _Inventory_System_.Code.Runtime.UI;
using Cinemachine;
using Sirenix.OdinInspector;
using Zenject;
using UnityEngine;

namespace _Game_Managment.Runtime
{
    public class PlayerCameraSettings : MonoBehaviour
    {
        [Header("Zoom Settings")] [SerializeField]
        private float _zoomAmountChangeSpeed;

        [Header("Rotation Settings")] [SerializeField]
        private float _rotationChangeAmount;

        private CinemachineVirtualCamera _virtualCamera;
        private CinemachineFramingTransposer _framingTransposer;
        private IPlayerInputHandler _playerInput;
        private IWindowFromInventoryHandler _inventoryWindow;

        [SerializeField, ReadOnly] private float _zoomAmount;
        [SerializeField, ReadOnly] private float _rotationAmount;

        private int ORTO_MIN_ZOOM_SIZE = 5;
        private int ORTO_MAX_ZOOM_SIZE = 12;

        [Inject]
        private void Constructor(IPlayerInputHandler input, IWindowFromInventoryHandler inventoryWindow)
        {
            _playerInput = input;
            _inventoryWindow = inventoryWindow;
        }

        private void Start()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            CinemachineComponentBase componentBase = _virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);

            if (componentBase is CinemachineFramingTransposer framingTransposer)
            {
                _framingTransposer = framingTransposer;
            }

            _zoomAmount = _framingTransposer.m_CameraDistance;
        }

        private void LateUpdate()
        {
            if (_inventoryWindow.IsWindowEnable) return;

            Zoom();
            Rotate();
        }

        private void Zoom()
        {
            _zoomAmount = _playerInput.MouseScroll > 0
                ? Mathf.Clamp(_zoomAmount - _zoomAmountChangeSpeed, ORTO_MIN_ZOOM_SIZE, ORTO_MAX_ZOOM_SIZE)
                : _playerInput.MouseScroll < 0
                    ? Mathf.Clamp(_zoomAmount + _zoomAmountChangeSpeed, ORTO_MIN_ZOOM_SIZE, ORTO_MAX_ZOOM_SIZE)
                    : _zoomAmount;

            _framingTransposer.m_CameraDistance = _zoomAmount;
        }

        private void Rotate()
        {
            if (Input.GetMouseButton(2) && !_playerInput.Aim)
            {
                float horizontalInput = Input.GetAxis("Mouse X");
                _rotationAmount += horizontalInput * (_rotationChangeAmount * Time.deltaTime);

                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, _rotationAmount,
                    transform.eulerAngles.z);
            }
        }
    }
}