using _Item_System_.Runtime.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Item_System_.Runtime.UI
{
    public sealed class ItemDisplay : MonoBehaviour
    {
        [Header("Display Settings")]
        [SerializeField] private TextMeshProUGUI _itemCountTextMesh;
        [SerializeField] private Image _itemImageContainer;

        [Header("Durability&Item Level Settings")]
        [SerializeField] private Slider _durabilitySlider;
        [SerializeField] private TextMeshProUGUI _itemLevelTextMesh;

        private bool _isEnableDurability;

        public Image ItemImage => _itemImageContainer;
        public TextMeshProUGUI ItemCountTextMesh => _itemCountTextMesh;

        public void UpdateSlotDisplay(ItemData data, int count = 1)
        {
            UpdateItemIcon(data.Icon);
            UpdateItemCount(count);

            if (data is WeaponData weaponData)
            {
                _durabilitySlider.maxValue = weaponData.Durability;
                _durabilitySlider.value = weaponData.Durability;

                _itemLevelTextMesh.SetText(weaponData.Level.ToString());

                _itemCountTextMesh.gameObject.SetActive(false);
                _durabilitySlider.gameObject.SetActive(true);

                _isEnableDurability = true;
            }
        }

        public void UpdateItemCount(int count)
        {
            _itemCountTextMesh.SetText(count.ToString());
        }

        public void UpdateItemIcon(Sprite icon)
        {
            _itemImageContainer.sprite = icon;
        }

        public void DisableUIElements()
        {
            _itemCountTextMesh.gameObject.SetActive(false);

            if (_isEnableDurability)
                _durabilitySlider.gameObject.SetActive(false);
        }

        public void EnableUIElements()
        {
            _itemCountTextMesh.gameObject.SetActive(!_isEnableDurability);

            if (_isEnableDurability)
                _durabilitySlider.gameObject.SetActive(true);

            _itemImageContainer.transform.localPosition = Vector3.zero;
        }

        public void UpdateDurability(int value)
        {
            _durabilitySlider.value = value;
        }

        public void SetItemLevel(int level)
        {
            _itemLevelTextMesh.SetText(level.ToString());
        }

        public void SetDurabilitySliderMaxValue(int value)
        {
            _durabilitySlider.maxValue = value;
        }
    }
}

