using System;
using System.Linq;
using _Crafting_System_.Runtime.Common;
using _Inventory_System_.Code.Runtime.Common;
using _Item_System_.Runtime.Base;
using _Other_.Runtime.Code;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Crafting_System_.Runtime.UI
{
    public interface ICraftingPanel
    {
        void UpdateCraftingItemPanelDisplay(ItemData data);
        void SetEnableWindow(bool isEnable, ItemData data = null);
        int CraftingOutputQuantity { get; }
    }

    public class CraftingPanelDisplay : Singleton<CraftingPanelDisplay>, ICraftingPanel
    {
        [Header("Window Settings")]
        [SerializeField] private CanvasGroup _craftingWindowCanvasGroup;

        [Header("Crafting Item Display Settings")]
        [SerializeField] private Image _itemImageContainer;
        [SerializeField] private TextMeshProUGUI _itemDescriptionTextMesh;
        [SerializeField] private TextMeshProUGUI _itemNameTextMesh;
        [SerializeField] private TextMeshProUGUI _itemSellPriceTextMesh;
        [SerializeField] private TextMeshProUGUI _itemWeightTextMesh;

        [Header("Requirement Item Settings")]
        [SerializeField, ReadOnly] private CraftingRequirement[] _craftingRequirements;

        [Header("Requirement Item Display Settings")]
        [SerializeField] private CraftingRequirementDisplay[] _requirementDisplays;

        [Header("Crafting Button Settings")]
        [SerializeField] private GameObject _outputItemQuantityContainer;
        [SerializeField] private Button _craftButton;
        [SerializeField] private Button _craftingQuantityIncreaseButton;
        [SerializeField] private Button _craftingQuantityDecreaseButton;

        [Header("Crafting Output Settings")]
        [SerializeField] private TMP_InputField _outputItemQuantityInputField;
        [SerializeField] private int _craftingOutputQuantity = 1;

        private ISlotManagement _itemSlotManagement;
        private ItemData _craftingOutputData;

        private const int OUTPUT_ITEM_QUANTITY_MIN = 1;
        private const int OUTPUT_ITEM_QUANTITY_MAX = 999;

        public delegate void CraftingItemRequested(ItemData data, int quantity = 1);
        public static event CraftingItemRequested OnCraftingItemRequested;

        public int CraftingOutputQuantity
        {
            get => _craftingOutputQuantity;
            private set
            {
                _craftingOutputQuantity = Mathf.Clamp(value, OUTPUT_ITEM_QUANTITY_MIN, OUTPUT_ITEM_QUANTITY_MAX);
                UpdateRequirementDisplays();
            }
        }

        [Inject]
        private void Constructor(ISlotManagement itemSlotManagement)
        {
            _itemSlotManagement = itemSlotManagement;
        }

        private void Start()
        {
            _craftButton.onClick.AddListener(() => PressCraftingButton(_craftingOutputData));
            _craftingQuantityIncreaseButton.onClick.AddListener(() => AdjustOutputItemQuantity(1));
            _craftingQuantityDecreaseButton.onClick.AddListener(() => AdjustOutputItemQuantity(-1));
            _outputItemQuantityInputField.onValueChanged.AddListener(OnQuantityInputFieldChanged);
        }
        private void Update()
        {
            if (_craftingOutputData == null || !AllRequirementsMet(_craftingOutputData))
            {
                _craftButton.interactable = false;
                return;
            }

            _craftButton.interactable = true;
        }

        public void UpdateCraftingItemPanelDisplay(ItemData data) //TODO: SRP için farklı bir sınıfa taşıyabilirim.
        {
            _outputItemQuantityContainer.SetActive(data is ConsumableData);

            _itemImageContainer.sprite = data.Icon;
            _itemNameTextMesh.SetText(data.DisplayName);
            _itemDescriptionTextMesh.SetText(data.DisplayDescription);
            _itemSellPriceTextMesh.SetText(data.SellPrice.ToString());
            _itemWeightTextMesh.SetText(data.Weight.ToString());

            _craftingRequirements = data.CraftingRequirement?.ToArray();

            ResetOutputItemQuantity();
            UpdateRequirementDisplays();

            _craftingOutputData = data;
            _craftButton.interactable = AllRequirementsMet(data); //FIXME: Interactable quantity değişitğinde güncellenmiyor.
        }

        public void SetEnableWindow(bool isEnable, ItemData data = null)
        {
            _craftingWindowCanvasGroup.transform.DOScale(isEnable ? 1 : 0, 0.2f);
            _craftingWindowCanvasGroup.DOFade(isEnable ? 1 : 0, 0.2f);

            if (!isEnable)
                ResetOutputItemQuantity();

            if (data != null)
                UpdateCraftingItemPanelDisplay(data);
        }

        private void UpdateRequirementDisplays()
        {
            if (_requirementDisplays == null || _craftingRequirements == null) return;

            int length = Mathf.Min(_requirementDisplays.Length, _craftingRequirements.Length);

            for (int i = 0; i < length; i++)
            {
                _requirementDisplays[i].gameObject.SetActive(true);
                _requirementDisplays[i].SetItemData(_craftingRequirements[i].Data);
                _requirementDisplays[i].SetBaseRequiredQuantity(_craftingRequirements[i].Quantity * _craftingOutputQuantity);
            }

            for (int i = length; i < _requirementDisplays.Length; i++)
                _requirementDisplays[i].gameObject.SetActive(false);
        }

        private bool AllRequirementsMet(ItemData data)
        {
            return data.CraftingRequirement.All(r => CheckItemQuantityInInventory(r.Data.Id, r.Quantity * _craftingOutputQuantity));
        }

        private bool CheckItemQuantityInInventory(int dataId, int count)
        {
            return _itemSlotManagement.GetItemQuantity(dataId) >= count;
        }

        private void OnQuantityInputFieldChanged(string newValue)
        {
            if (int.TryParse(newValue, out int result))
                CraftingOutputQuantity = result;
        }

        private void AdjustOutputItemQuantity(int amount)
        {
            CraftingOutputQuantity += amount;
            _outputItemQuantityInputField.SetTextWithoutNotify(CraftingOutputQuantity.ToString());
        }

        private void ResetOutputItemQuantity()
        {
            CraftingOutputQuantity = OUTPUT_ITEM_QUANTITY_MIN;
            _outputItemQuantityInputField.SetTextWithoutNotify(OUTPUT_ITEM_QUANTITY_MIN.ToString());
        }

        private void PressCraftingButton(ItemData data)
        {
            OnCraftingItemRequested?.Invoke(data, _craftingOutputQuantity);
        }
    }
}
