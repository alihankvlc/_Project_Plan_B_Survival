
using _Crafting_System_.Runtime.Common;
using _Item_System_.Runtime.Base;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Crafting_System_.Runtime.UI
{
    public class CraftingPanelSlotInItemDisplay : MonoBehaviour, IPointerClickHandler
    {
        [Header("Display Settings")]
        [SerializeField] private Image _itemImageContainer;
        [SerializeField] private TextMeshProUGUI _itemLevelTextMesh;

        [Header("Status Settings")]
        [SerializeField] private CraftingStatus _craftingStatus;
        [SerializeField] private GameObject _lockIcon;

        private ItemData _data;

        public void UpdateDisplay(ItemData data, CraftingStatus craftingStatus)
        {
            _itemImageContainer.sprite = data.Icon;
            _craftingStatus = craftingStatus;

            _data = data;

            _lockIcon.SetActive(craftingStatus == CraftingStatus.CanNotCraft);

            if ((data is WeaponData weaponData) && weaponData.Level > 0 && craftingStatus == CraftingStatus.CanCraft)
                SetCraftingStatus(CraftingStatus.CanCraft, true, weaponData.Level);
        }

        public void SetItemLevel(int itemLevel)
        {
            _itemLevelTextMesh.text = $"Lv. {itemLevel}";
        }

        public void SetCraftingStatus(CraftingStatus craftingStatus, bool isWeapon = false, int itemLevel = 0)
        {
            _craftingStatus = craftingStatus;
            _lockIcon.SetActive(craftingStatus == CraftingStatus.CanNotCraft);

            if (isWeapon)
            {
                _itemLevelTextMesh.gameObject.SetActive(isWeapon && craftingStatus == CraftingStatus.CanCraft);
                _itemLevelTextMesh.text = $"Lv. {Mathf.Clamp(itemLevel, 1, int.MaxValue)}";
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            CraftingPanelDisplay.Instance.SetEnableWindow(true, _data);
        }
    }
}
