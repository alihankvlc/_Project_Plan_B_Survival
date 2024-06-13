using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Other_.Runtime.Code
{
    public static class RaycastHandler
    {
        private static Camera _mainCamera;

        public static bool SendRay(out RaycastHit hitInfo)
        {
            _mainCamera ??= Camera.main;

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hitInfo);
        }

        public static bool SendRay(out RaycastHit hitInfo, float distance)
        {
            _mainCamera ??= Camera.main;

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hitInfo, distance);
        }

        public static bool SendRay(out RaycastHit hitInfo, LayerMask layerMask)
        {
            _mainCamera ??= Camera.main;

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask);
        }

        public static bool SendRay(out RaycastHit hitInfo, float distance, LayerMask layerMask)
        {
            _mainCamera ??= Camera.main;

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hitInfo, distance, layerMask);
        }
    }
}