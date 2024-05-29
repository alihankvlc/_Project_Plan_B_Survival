using _Project_Plan_B_Common;
using _Project_Plan_B_Survival_Inventory_System.Code.Runtime.UI;
using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project_Plan_B_Survival
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("UI Inventory Settings")]
        [SerializeField] private List<UIAddItemToInventoryInfo> _uiAddItemToInventoryInfos = new();
        [SerializeField] private UIAddItemToInventoryInfo _uiAddItemToInventoryInfo;
        [SerializeField] private Transform _uiAddItemToInventoryParent;

        private void Start()
        {
            Inventory.OnItemAddedToInventory += ItemAddedToInventory;
        }
        public void RemoveInformationProvider(UIAddItemToInventoryInfo provider)
        {
            provider.OnDisable -= RemoveInformationProvider;

            if (_uiAddItemToInventoryInfos.Contains(provider))
                _uiAddItemToInventoryInfos.Remove(provider);

        }

        private void ItemAddedToInventory(ItemData data, int count)
        {
            if (HasInformation(data, out UIAddItemToInventoryInfo info))
            {
                info.SetItemCount(count);
                return;
            }

            GameObject infoObject = Instantiate(_uiAddItemToInventoryInfo.gameObject, _uiAddItemToInventoryParent);
            UIAddItemToInventoryInfo infoComponent = infoObject.GetComponent<UIAddItemToInventoryInfo>();

            infoComponent.OnDisable += RemoveInformationProvider;

            infoComponent.Init(data.Icon, data.Id, count);
            _uiAddItemToInventoryInfos.Add(infoComponent);
        }
        private bool HasInformation(ItemData data, out UIAddItemToInventoryInfo info)
        {
            info = _uiAddItemToInventoryInfos.FirstOrDefault(r => r.Id == data.Id);
            return info != null;
        }

        private void OnDestroy()
        {
            Inventory.OnItemAddedToInventory -= ItemAddedToInventory;
        }
    }
}

