using _Inventory_System_.Code.Runtime.Common;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Inventory_System_.Code.Runtime.UI;
using _Item_System_.Runtime.Base;
using _Other_.Runtime.Code;
using _Stat_System.Runtime.Base;
using _Stat_System.Runtime.Common;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Logger = _Other_.Runtime.Code.Logger;

namespace _UI_Managment_.Runtime.Common
{
    public interface IVisualHandler
    {
        public void MoveToToolBeltSlot(bool isMoveToDefault);
    }

    [Serializable]
    public class UIPlayerStat
    {
        [SerializeField] private StatType _type;
        [SerializeField] private Slider _slider;
        [SerializeField] private CanvasGroup[] _indicators;

        public StatType Type => _type;
        public Slider Slider => _slider;
        public CanvasGroup[] IndicatorArray => _indicators;
    }


    public sealed class UIManager : Singleton<UIManager>, IVisualHandler, IStatObserver
    {
        public int deneme;
        [Header("UI Player Stat Settings")]
        [SerializeField] private List<UIPlayerStat> _uiPlayerStats = new();
        [SerializeField] private GameObject _uiPlayerStatsParent;

        [Header("UI Inventory Settings")]
        [SerializeField] private List<UIAddItemToInventoryInfo> _uiAddItemToInventoryInfos = new();
        [SerializeField] private UIAddItemToInventoryInfo _uiAddItemToInventoryInfo;
        [SerializeField] private Transform _uiAddItemToInventoryParent;
        [SerializeField] private TextMeshProUGUI _inventoryWeight;

        [Header("UI Inventory Item Description Settings")]
        [SerializeField] private GameObject _uiInventoryItemDescription;
        [SerializeField] private TextMeshProUGUI _itemNameTextMesh;
        [SerializeField] private TextMeshProUGUI _itemDescriptionTextMesh;
        [SerializeField] private TextMeshProUGUI _itemWeightTextMesh;
        [SerializeField] private TextMeshProUGUI _itemSellPriceTextMesh;

        [Header("UI System Message Settings")]
        [SerializeField] private TextMeshProUGUI _systemMessageTextMesh;

        [Header("ToolBelt UI Slot Parent Settings")]
        [SerializeField] private Transform _toolBeltTransform;
        [SerializeField] private Transform _toolBeltInventoryParent;
        [SerializeField] private Transform _toolBeltDefaultParent;

        private StatManager _statSubject;
        private IMenuManager _menuManager;
        private Tween _fadeInTween;

        private const int STAT_FIRST_INDICATOR_LEVEL = 1;
        private const int STAT_SECOND_INDICATOR_LEVEL = 2;
        private const int STAT_THIRD_INDICATOR_LEVEL = 3;
        private const int STAT_FIRST_INDICATOR_THRESHOLD = 5;
        private const int STAT_SECOND_INDICATOR_THRESHOLD = 15;

        private const float FADE_IN_DELAY_DURATION = 2f;

        [Inject]
        private void Constructor(StatManager subject, IMenuManager menuManager)
        {
            _statSubject = subject;
            _menuManager = menuManager;

            _statSubject.RegisterObserver(this);
        }

        private void Start()
        {
            Inventory.OnItemAddedToInventory += ItemAddedToInventory;
            InventoryWeight.OnChangeInventoryWeight += SetInventoryWeight;

            ToolBelt.OnItemEquipped += ShowEquippedItem;
            ToolBelt.OnItemUnequipped += HideEquippedItem;

            Logger.LogAction += ShowToPlayerMessage;
        }

        private void Update()
        {
            _uiPlayerStatsParent.SetActive(_menuManager.ActiveMenu == MenuType.None);
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

        public void ShowInventoryItemInfo(ItemData data, bool isEnable)
        {
            _uiInventoryItemDescription.SetActive(isEnable);

            if (isEnable)
            {
                _uiInventoryItemDescription.transform.DOScale(Vector3.one, 0.2f);

                _itemNameTextMesh.SetText(data.DisplayName);
                _itemDescriptionTextMesh.SetText(data.Description);
                _itemWeightTextMesh.SetText($"{data.Weight}");
                _itemSellPriceTextMesh.SetText($"${data.SellPrice}");
            }
        }

        private void SetInventoryWeight(float weight, float maxWeight)
        {
            string info = $"{weight} / {maxWeight}";

            _inventoryWeight.SetText(info);
            _inventoryWeight.color = weight > maxWeight ? Color.red : Color.white;
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

        private void ShowEquippedItem(SlotItem slotItem)
        {
        }

        private void HideEquippedItem(SlotItem slotItem)
        {
            
        }

        public void MoveToToolBeltSlot(bool isMoveToDefault)
        {
            _toolBeltTransform.SetParent(isMoveToDefault ? _toolBeltDefaultParent : _toolBeltInventoryParent, false);
        }

        public void OnModifyStat(IStat stat)
        {
            if (stat.Group != StatGroup.Player) return;

            UIPlayerStat playerStat = _uiPlayerStats.FirstOrDefault(r => r.Type == stat.Type);
            Slider slider = playerStat.Slider;

            slider.maxValue = stat.BaseValue;
            slider.value = stat.Value;


            SetPlayerStatThresholdColor((int)playerStat.Slider.value, slider, Color.red);
        }

        public int GetPlayerStatIndicatorLevel(int statChangeAmount)
        {
            int level = 0;

            level = Mathf.Abs(statChangeAmount) switch
            {
                int n when (n > 0 && n < STAT_FIRST_INDICATOR_THRESHOLD)
                => STAT_FIRST_INDICATOR_LEVEL,
                int n when (n >= STAT_FIRST_INDICATOR_THRESHOLD && n < STAT_SECOND_INDICATOR_THRESHOLD)
                => STAT_SECOND_INDICATOR_LEVEL,
                int n when (n >= STAT_SECOND_INDICATOR_THRESHOLD)
                => STAT_THIRD_INDICATOR_LEVEL,
                _ => 0
            };

            return level;
        }
        private void SetPlayerStatThresholdColor(int value, Slider slider, Color color, float threshold = 15f)
        {
            bool checkThreshold = value < slider.maxValue * threshold / 100f;
            slider.fillRect.GetComponent<Image>().color = checkThreshold ? color : Color.white;
        }

        private void OnDestroy()
        {
            Inventory.OnItemAddedToInventory -= ItemAddedToInventory;
            InventoryWeight.OnChangeInventoryWeight -= SetInventoryWeight;

            ToolBelt.OnItemEquipped -= ShowEquippedItem;
            ToolBelt.OnItemUnequipped -= HideEquippedItem;

            Logger.LogAction -= ShowToPlayerMessage;
        }
    }
}
