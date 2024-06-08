using System;
using _Item_System_.Runtime.Base;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Crafting_System_.Runtime.UI
{
    public class ItemCraftingDisplay : MonoBehaviour
    {
        [Header("Display Settings")]
        [SerializeField] private Image _itemIconImage;
        [SerializeField] private Slider _itemCountSlider;
        [SerializeField] private TextMeshProUGUI _itemDurationTextMesh;
        [SerializeField] private TextMeshProUGUI _itemLevelTextMesh;
        [SerializeField] private TextMeshProUGUI _itemCountTextMesh;

        [SerializeField] private GameObject _lockedObject;
        [SerializeField] private GameObject _craftingDurationObject;

        private float _duration;
        private float _itemDuration;

        public void SetItemData(ItemData data, int count = 1)
        {
            SetCraftingDuration(data.CraftingDuration);

            _itemIconImage.sprite = data.Icon;

            _duration = data.CraftingDuration * count;
            _itemDuration = data.CraftingDuration;

            _itemCountSlider.maxValue = _duration;
            _itemCountSlider.value = _duration;

            if (data is WeaponData weaponData)
            {
                _itemLevelTextMesh.gameObject.SetActive(weaponData.Level > 0);
                _itemLevelTextMesh.text = $"Lv. {weaponData.Level}";
                return;
            }

            _itemCountSlider.gameObject.SetActive(count > 1);
            _itemCountTextMesh.text = count.ToString();
        }


        public void SetCraftingDuration(float duration)
        {
            int minutes = Mathf.FloorToInt(duration / 60f);
            int seconds = Mathf.FloorToInt(duration % 60f);

            minutes = Mathf.Clamp(minutes, 0, int.MaxValue);
            seconds = Mathf.Clamp(seconds, 0, 59);

            _itemDurationTextMesh.SetText($"{minutes}:{seconds:D2}");
        }

        public void UpdateCraftingSlider(float elapsedTime, Action action, ref int craftingQuantity)
        {
            _itemCountSlider.value = _duration - elapsedTime;

            if (elapsedTime % _itemDuration < Time.deltaTime && elapsedTime > 0)
            {
                action?.Invoke();

                craftingQuantity--;
                _itemCountTextMesh.SetText($"{craftingQuantity}");

                transform.DOScale(Vector3.one * 1.1f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InBack);
                });

                if (craftingQuantity <= 0)
                    _itemCountSlider.gameObject.SetActive(false);
            }
        }

        public void SetBlocked(bool isBlocked)
        {
            _lockedObject.SetActive(isBlocked);
            _craftingDurationObject.SetActive(!isBlocked);
        }
        public void SetItemCount(int count)
        {
            _itemCountTextMesh.SetText(count.ToString());
        }
    }

}
