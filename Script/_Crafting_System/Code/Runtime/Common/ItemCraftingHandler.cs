using _Inventory_System_.Code.Runtime.Common;
using _Item_System_.Runtime.Base;
using _Other_.Runtime.Code;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Crafting_System_.Runtime.UI
{
    [RequireComponent(typeof(ItemCraftingDisplay))]
    public class ItemCraftingHandler : MonoBehaviour
    {

        [SerializeField] private ItemCraftingDisplay _itemCraftingDisplay;
        [SerializeField] private int _craftingOutputQuantity;
        [SerializeField] private ItemData _outputItemData;

        private IItemManagement _itemManagement;
        private IInventoryState _inventoryState;


        private bool _isCrafting;
        private bool _isBlocked;
        private float _craftingDuration;
        private float _craftingTimer;

        private const string CAN_NOT_ENOUGH_INVENTORY_SPACE_TEXT = "Not enough inventory space";

        private void Start()
        {
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        [Inject]
        private void Constructor(IItemManagement itemManagement, IInventoryState inventoryState)
        {
            _itemManagement = itemManagement;
            _inventoryState = inventoryState;
        }

        private void Update()
        {

            if (_isCrafting && _inventoryState.HasInventorySpace(_outputItemData.Id, count: _craftingOutputQuantity))
            {
                _isBlocked = false;
                _craftingTimer -= Time.deltaTime;

                _itemCraftingDisplay.SetCraftingDuration(_craftingTimer);
                _itemCraftingDisplay.UpdateCraftingSlider(_craftingTimer, AddToInventory, ref _craftingOutputQuantity);

                if (_craftingTimer <= 0)
                {
                    ResetCrafting();
                    transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        Destroy(_itemCraftingDisplay.gameObject);
                    });
                }
            }
            else if (!_inventoryState.HasInventorySpace(_outputItemData.Id, count: _craftingOutputQuantity) && !_isBlocked)
            {
                LoggingUtility.Log.Warning(this, CAN_NOT_ENOUGH_INVENTORY_SPACE_TEXT, Color.red, true);
                _isBlocked = true;
            }

            _itemCraftingDisplay.SetBlocked(_isBlocked);
        }

        public void CraftItem(ItemData data, int quantity = 1)
        {
            _outputItemData = data;
            _craftingOutputQuantity = quantity;

            _craftingDuration = _outputItemData.CraftingDuration * _craftingOutputQuantity;
            _craftingTimer = _craftingDuration;

            _itemCraftingDisplay.SetItemData(_outputItemData, _craftingOutputQuantity);

            _isCrafting = true;
        }

        private void AddToInventory()
        {
            _itemCraftingDisplay.SetItemCount(_craftingOutputQuantity);
            _itemManagement.AddItemToInventory(_outputItemData.Id, count: 1);
        }

        private void ResetCrafting()
        {
            _craftingTimer = 0;
            _isCrafting = false;
        }

    }
}

