using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;
namespace _Project_Plan_B_Survival_Inventory_System.Code.Runtime.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIAddItemToInventoryInfo : MonoBehaviour
    {
        [Header("UI Settings")]
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _countTextMesh;
        [SerializeField] private float _disableDuration = 3f;

        [Header("Item Settings")]
        [SerializeField, ReadOnly] private int _itemId;
        [SerializeField, ReadOnly] private int _count;

        private float _timer;
        private CanvasGroup _canvasGroup;
        private Tweener _fadeTweener;

        public event Action<UIAddItemToInventoryInfo> OnDisable;

        public int Id => _itemId;

        public int Count
        {
            get => _count;
            private set
            {
                _count = value;
                _countTextMesh.SetText("+" + _count.ToString());
            }
        }

        private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

        private void Start() => transform.DOScale(Vector3.one, 0.5f);

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _disableDuration - 1 && (_fadeTweener == null || !_fadeTweener.IsActive()))
                _fadeTweener = _canvasGroup.DOFade(0, 1f).OnComplete(DisableAndDestroy);
        }

        public void Constructor(Sprite icon, int itemId, int count = 1)
        {
            _icon.sprite = icon;
            _itemId = itemId;
            Count = count;

            _canvasGroup.alpha = 1;
            _timer = 0;
        }

        public void SetItemCount(int count)
        {
            Count += count;
            _timer = 0;

            _fadeTweener?.Kill();
            _canvasGroup.alpha = 1;
        }

        private void DisableAndDestroy()
        {
            OnDisable?.Invoke(this);
            Destroy(gameObject);
        }

        private void OnDestroy() => _fadeTweener?.Kill();
    }
}
