using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Item_System_.Runtime.Common
{
    [RequireComponent(typeof(Outline))]
    public class ItemFindScanVisualHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _visualObject;
        private const float DISABLE_OUTLINE_DURATION = 3f;

        private bool _isPlayingCourutine;
        private Outline _outline;

        private void Start()
        {
            _outline = GetComponent<Outline>();
        }

        public void EnableOutline()
        {
            if (_isPlayingCourutine) return;
            StartCoroutine(DisableOutlineCoroutine());
        }

        private IEnumerator DisableOutlineCoroutine()
        {
            _isPlayingCourutine = true;
            _outline.enabled = true;
            _visualObject.SetActive(true);

            Vector3 defaultScale = transform.localScale;

            transform.DOScale(defaultScale * 1.25f, 0.35f).OnComplete(() =>
            {
                transform.DOScale(defaultScale, 0.35f).SetEase(Ease.InBounce);
            });

            WaitForSeconds duration = new(DISABLE_OUTLINE_DURATION);
            yield return duration;

            _isPlayingCourutine = false;

            _outline.enabled = false;
            _visualObject.SetActive(false);
        }
    }
}