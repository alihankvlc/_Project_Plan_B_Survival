using UnityEngine;

namespace _UI_Managment_.Runtime.Common
{
    public class UILookAt : MonoBehaviour
    {
        private Camera _mainCamera;
        private void Start() => _mainCamera = Camera.main;
        public void Update()
        => transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward);
    }
}
