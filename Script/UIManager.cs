using _Project_Plan_B_Common;
using _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Common;
using _Project_Plan_B_Survival_Inventory_System.Code.Runtime.UI;
using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace _Project_Plan_B_Survival
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("UI Inventory Settings")]
        [SerializeField] private List<UIAddItemToInventoryInfo> _uiAddItemToInventoryInfos = new();
        [SerializeField] private UIAddItemToInventoryInfo _uiAddItemToInventoryInfo;
        [SerializeField] private Transform _uiAddItemToInventoryParent;

        [Header("UI System Message Settings")]
        [SerializeField] private TextMeshProUGUI _systemMessageTextMesh;

        private Tween _fadeInTween;
        private const float FADE_IN_DELAY_DURATION = 2f;

        private void Start()
        {
            Inventory.OnItemAddedToInventory += ItemAddedToInventory;
            Logger.LogAction += ShowToPlayerMessage;
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

            infoComponent.Constructor(data.Icon, data.Id, count);
            _uiAddItemToInventoryInfos.Add(infoComponent);
        }
        private bool HasInformation(ItemData data, out UIAddItemToInventoryInfo info)
        {
            info = _uiAddItemToInventoryInfos.FirstOrDefault(r => r.Id == data.Id);
            return info != null;
        }

        private void ShowToPlayerMessage(string message)
        {
            GameObjectEnableFadeIn(_systemMessageTextMesh.gameObject, 0.5f);
            _systemMessageTextMesh.SetText(message);
        }


        private void GameObjectEnableFadeIn(GameObject gameObject, float time)
        {
            SetEnableGameObject(gameObject, true);

            if (_fadeInTween != null && _fadeInTween.IsActive())
                _fadeInTween.Kill();

            _fadeInTween = gameObject.transform.DOScale(1, time).OnComplete(() => SetEnableGameObject(gameObject, false));
            _fadeInTween.SetDelay(FADE_IN_DELAY_DURATION);
        }



        private void SetEnableGameObject(GameObject obj, bool isEnable)
        {
            obj.SetActive(isEnable);
        }

        private void OnDestroy()
        {
            Inventory.OnItemAddedToInventory -= ItemAddedToInventory;
            Logger.LogAction -= ShowToPlayerMessage;
        }
    }
}

