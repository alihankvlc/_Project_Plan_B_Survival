using System;
using _Crafting_System_.Runtime.UI;
using _Inventory_System_.Code.Runtime.Common;
using _Inventory_System_.Code.Runtime.UI;
using _Item_System_.Runtime.Base;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CraftingRequirementDisplay : MonoBehaviour
{
    [Header("Crafting Requirement Settings")] [SerializeField]
    private int _requiredQuantity;

    [SerializeField] private int _baseRequiredQuantity;
    [SerializeField, ReadOnly] private int _currentQuantity = 0;

    [Header("Display Settings")] [SerializeField]
    private TextMeshProUGUI _quantityText;

    [SerializeField] private Image _requirementImage;

    private IWindowFromInventoryHandler _inventoryWindow;
    private ISlotManagement _itemSlotManagment;
    private ICraftingPanel _craftingPanel;
    private ItemData _itemData;

    public int CurrentQuantity => _currentQuantity;
    public int RequiredQuantity => _requiredQuantity;
    public int BaseRequiredQuantity => _baseRequiredQuantity;

    [Inject]
    private void Consturctor(ISlotManagement itemSlotManagment, ICraftingPanel craftingPanel,
        IWindowFromInventoryHandler inventoryWindow)
    {
        _itemSlotManagment = itemSlotManagment;
        _craftingPanel = craftingPanel;
        _inventoryWindow = inventoryWindow;
    }

    private void Update()
    {
        if (_inventoryWindow.IsWindowEnable && _itemData != null)
        {
            CheckItemQuantityInInventory(_itemData.Id, ref _currentQuantity);
        }
    }

    public void SetItemData(ItemData data)
    {
        _itemData = data;
        _requirementImage.sprite = _itemData.Icon;
    }

    public void SetRequiredQuantity(int quantity)
    {
        _requiredQuantity = quantity;
        UpdateRequirementDisplay();
    }

    public void SetBaseRequiredQuantity(int quantity)
    {
        _baseRequiredQuantity = quantity;
        _requiredQuantity = _baseRequiredQuantity;

        UpdateRequirementDisplay();
    }

    public void UpdateCurrentQuantity(int quantity)
    {
        _currentQuantity = quantity;
        UpdateRequirementDisplay();
    }

    public void AddToCurrentQuantity(int quantity)
    {
        _currentQuantity += quantity;
        UpdateRequirementDisplay();
    }

    private void UpdateRequirementDisplay()
    {
        _quantityText.SetText($"{_currentQuantity}/{_requiredQuantity}");
        _quantityText.color = _currentQuantity >= _requiredQuantity ? Color.white : Color.red;
    }

    //FIXME:Bazen current quantity güncellenmiyor.
    private void CheckItemQuantityInInventory(int dataId, ref int currentRequirementQuantity)
    {
        int itemCount = _itemSlotManagment.GetItemQuantity(dataId);
        currentRequirementQuantity = itemCount;

        UpdateCurrentQuantity(itemCount);
    }
}