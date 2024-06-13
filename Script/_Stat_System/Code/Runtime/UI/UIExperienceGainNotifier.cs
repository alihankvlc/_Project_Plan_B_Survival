using System;
using _Inventory_System_.Code.Runtime.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace _Stat_System.Runtime.UI
{
    public class UIExperienceGainNotifier : MonoBehaviour
    {
        [Header("UI Settings")] [SerializeField]
        private TextMeshProUGUI _countTextMesh;

        [SerializeField] private float _disableDuration = 3f;

        private int _exp;

        private float _timer;
        private CanvasGroup _canvasGroup;
        private Tweener _fadeTweener;

        public event Action<UIExperienceGainNotifier> OnDisable;

        public int Count { get; private set; }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _disableDuration - 1 && (_fadeTweener == null || !_fadeTweener.IsActive()))
            {
                _fadeTweener = _canvasGroup.DOFade(0, 1f).OnComplete(DisableAndDestroy);
            }
        }

        public void Constructor(int expGain)
        {
            Count = expGain;
            _countTextMesh.SetText("+" + Count.ToString());

            _canvasGroup.alpha = 1;
            _timer = 0;
        }

        public void SetExperienceGain(int expGain)
        {
            Count += expGain;
            _countTextMesh.SetText("+" + Count.ToString());

            _timer = 0;
            _fadeTweener?.Kill();
            _canvasGroup.alpha = 1;
        }

        private void DisableAndDestroy()
        {
            OnDisable?.Invoke(this);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _fadeTweener?.Kill();
        }
    }
}