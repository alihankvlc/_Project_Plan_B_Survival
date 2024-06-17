using System.Collections;
using UnityEngine;
using Sirenix.Utilities;

namespace _Item_System_.Runtime.Common
{
    public sealed class ItemFindScan : MonoBehaviour
    {
        [SerializeField] private float _radius;
        [SerializeField] private float _scanSpeed;

        [SerializeField] private LayerMask _itemLayerMask;
        [SerializeField] private Material _screenScannerMat;

        private const int SEGMENTS = 60;
        private const float ANGLE = 360f;
        private const string ITEM_LAYER_NAME = "Item";
        private const string SCREEN_SCAN_MAT_POSITION_NAME = "_Position";
        private const string SCREEN_SCAN_MAT_OPACITY_NAME = "_Opacity";
        private const string SCREEN_SCAN_MAT_RANGE_NAME = "_Range";
        private const float DEFAULT_RANGE = 0.001f;
        private const float DEFAULT_OPACITY = 3f;

        private float _timer;

        private void Start()
        {
            _itemLayerMask = LayerMask.GetMask(ITEM_LAYER_NAME);
        }

        private void OnEnable()
        {
            _screenScannerMat.SetVector(SCREEN_SCAN_MAT_POSITION_NAME, transform.position);
            _screenScannerMat.SetFloat(SCREEN_SCAN_MAT_OPACITY_NAME, DEFAULT_OPACITY);
        }

        private void Update()
        {
            _timer += Time.deltaTime * _scanSpeed;

            _screenScannerMat.SetFloat(SCREEN_SCAN_MAT_RANGE_NAME, _timer);
            _radius = _screenScannerMat.GetFloat(SCREEN_SCAN_MAT_RANGE_NAME);

            Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, _itemLayerMask);
            float halfAngle = ANGLE / 2.0f;

            colliders.ForEach(r =>
            {
                Vector3 directionToCollider = (r.transform.position - transform.position).normalized;
                float angleToCollider = Vector3.Angle(transform.forward, directionToCollider);

                if (angleToCollider <= halfAngle &&
                    r.TryGetComponent(out ItemFindScanVisualHandler outlineHandler))
                {
                    outlineHandler.EnableOutline();
                }
            });
        }

        private void OnDisable()
        {
            _screenScannerMat.SetFloat(SCREEN_SCAN_MAT_RANGE_NAME, DEFAULT_RANGE);
            _screenScannerMat.SetFloat(SCREEN_SCAN_MAT_OPACITY_NAME, 0f);

            _timer = 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            float halfAngle = ANGLE / 2.0f;
            float startAngle = -halfAngle;

            Vector3 startPoint = transform.position + Quaternion.Euler(0, startAngle, 0) * transform.forward * _radius;

            Vector3 previousPoint = startPoint;

            for (int i = 1; i <= SEGMENTS; i++)
            {
                float currentAngle = startAngle + (ANGLE / SEGMENTS) * i;
                Vector3 currentPoint = transform.position +
                                       Quaternion.Euler(0, currentAngle, 0) * transform.forward * _radius;
                Gizmos.DrawLine(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }
        }
    }
}