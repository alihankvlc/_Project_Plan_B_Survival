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
using _Player_System_.Runtime.Combat.Sub;
using _Player_System_.Runtime.Common;
using _Stat_System.Runtime.Sub;
using _Stat_System.Runtime.UI;
using _UI_Managment_.Runtime.Menu.Common;
using DistantLands.Cozy;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;
using LoggingUtility = _Other_.Runtime.Code.LoggingUtility;

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
        [Header("UI Player Stat Settings")] [SerializeField]
        private List<UIPlayerStat> _uiPlayerStats = new();

        [SerializeField] private TextMeshProUGUI _levelUpInfoTextMesn;

        [SerializeField] private GameObject _uiPlayerStatsParent;

        [SerializeField] private UIExperienceGainNotifier _uiExperienceGainNotifier;

        [Header("UI Inventory Settings")] [SerializeField]
        private UIAddedItemNotifier _uiAddedItemNotifier;

        [SerializeField] private TextMeshProUGUI _inventoryWeight;

        [Header("UI Inventory Item Description Settings")] [SerializeField]
        private GameObject _uiInventoryItemDescription;

        [SerializeField] private TextMeshProUGUI _itemNameTextMesh;
        [SerializeField] private TextMeshProUGUI _itemDescriptionTextMesh;
        [SerializeField] private TextMeshProUGUI _itemWeightTextMesh;
        [SerializeField] private TextMeshProUGUI _itemSellPriceTextMesh;

        [Header("UI System Message Settings")] [SerializeField]
        private TextMeshProUGUI _systemMessageTextMesh;

        [Header("ToolBelt UI Slot Parent Settings")] [SerializeField]
        private Transform _toolBeltTransform;

        [SerializeField] private Transform _toolBeltInventoryParent;
        [SerializeField] private Transform _toolBeltDefaultParent;

        [Header("Looting Timer UI Settings")] [SerializeField]
        private TextMeshProUGUI _lootingTimerTextMesh;

        [SerializeField] private Slider _lootingTimerSlider;
        [SerializeField] private GameObject _lootingTimerParent;

        [Header("Time UI Settings")] [SerializeField]
        private TextMeshProUGUI _timeTextMesh;

        [Header("Player Damage UI Settings")] [SerializeField]
        private CanvasGroup _damageableCanvasGroup;

        [SerializeField] private TextMeshProUGUI _damageableLayerHealthTextMesh;
        [SerializeField] private TextMeshProUGUI _damageableLayerName;
        [SerializeField] private Slider _damageableSlider;

        private List<UIExperienceGainNotifier> _uiExperienceGainNotifierInfos = new();
        private List<UIAddedItemNotifier> _uiAddItemNotifierInfos = new();

        private bool _isShowDamageInfo = false;

        private StatObserverManager _statObserverSubject;
        private IMenuManager _menuManager;
        private Tween _fadeInTween;

        private const int FADE_IN_DELAY_DURATION = 2;
        private const int DISABLE_DAMAGE_INFO_DURATION = 3;

        private float _disableDamageInfoObjectTimer;

        [FormerlySerializedAs("_uiAddItemToInventoryParent")] [Header("Information Settings")] [SerializeField]
        private Transform _uiPlayerInformationParent;

        [Inject]
        private void Constructor(StatObserverManager subject, IMenuManager menuManager)
        {
            _statObserverSubject = subject;
            _menuManager = menuManager;

            _statObserverSubject.RegisterObserver(this);
        }

        private void Start()
        {
            LoggingUtility.LogAction += ShowToPlayerMessage;

            Inventory.OnItemAddedToInventory += ItemAddedNotifier;
            InventoryWeight.OnChangeInventoryWeight += SetInventoryWeight;

            Experience.OnChangeLevel += OnPlayerLevelUp;
            PlayerStatHandler.OnExperienceGainedEvent += ExperienceGainNotifier;

            LootManager.OnLootTimerUpdateEvent += SetLootTimer;

            MeleeCombat.OnDamageApplied += ShowDamageableLayerHealth;
        }

        private void Update()
        {
            _uiPlayerStatsParent.SetActive(_menuManager.ActiveMenu == MenuType.None);

            int hours = CozyTimeModule.Instance.currentTime.hours;
            int minutes = CozyTimeModule.Instance.currentTime.minutes;
            int day = CozyTimeModule.Instance.currentDay;

            UpdateTime(day, hours, minutes);

            if (_isShowDamageInfo)
            {
                _disableDamageInfoObjectTimer -= Time.deltaTime;

                if (_disableDamageInfoObjectTimer <= 0)
                    UIResetDamageInfo();
            }
        }

        public void RemoveAddedItemEvent(UIAddedItemNotifier provider)
        {
            provider.OnDisable -= RemoveAddedItemEvent;

            if (_uiAddItemNotifierInfos.Contains(provider))
                _uiAddItemNotifierInfos.Remove(provider);
        }

        public void UpdateTime(int day, int hours, int minute)
        {
            string timeInfo = $"DAY: {day} TIME: {hours:D2}:{minute:D2}";
            _timeTextMesh.SetText(timeInfo);
        }

        public void RemoveExperienceGainEvent(UIExperienceGainNotifier provider)
        {
            provider.OnDisable -= RemoveExperienceGainEvent;

            if (_uiExperienceGainNotifierInfos.Contains(provider))
                _uiExperienceGainNotifierInfos.Remove(provider);
        }

        private void ItemAddedNotifier(ItemData data, int count)
        {
            if (HasInformation(data, out UIAddedItemNotifier info))
            {
                info.SetItemCount(count);
                return;
            }

            GameObject infoObject = Instantiate(_uiAddedItemNotifier.gameObject, _uiPlayerInformationParent);
            UIAddedItemNotifier infoComponent = infoObject.GetComponent<UIAddedItemNotifier>();

            infoComponent.OnDisable += RemoveAddedItemEvent;

            infoComponent.Constructor(data.Icon, data.Id, count);
            _uiAddItemNotifierInfos.Add(infoComponent);
        }

        private void ExperienceGainNotifier(int expGain)
        {
            if (HasExpInfo(out UIExperienceGainNotifier expInfo))
            {
                expInfo.SetExperienceGain(expGain);
                return;
            }

            GameObject expObject = Instantiate(_uiExperienceGainNotifier.gameObject, _uiPlayerInformationParent);
            UIExperienceGainNotifier expComponent = expObject.GetComponent<UIExperienceGainNotifier>();

            expComponent.OnDisable += RemoveExperienceGainEvent;

            expComponent.Constructor(expGain);
            _uiExperienceGainNotifierInfos.Add(expComponent);
        }

        private bool HasExpInfo(out UIExperienceGainNotifier expInfo)
        {
            expInfo = _uiExperienceGainNotifierInfos.FirstOrDefault();
            return expInfo != null;
        }

        private bool HasInformation(ItemData data, out UIAddedItemNotifier info)
        {
            info = _uiAddItemNotifierInfos.FirstOrDefault(r => r.Id == data.Id);
            return info != null;
        }

        public void ShowInventoryItemInfo(ItemData data, bool isEnable)
        {
            _uiInventoryItemDescription.SetActive(isEnable);

            if (isEnable)
            {
                _uiInventoryItemDescription.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutCubic);

                _itemNameTextMesh.SetText(data.DisplayName);
                _itemDescriptionTextMesh.SetText(data.DisplayDescription);
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

        private void ShowToPlayerMessage(string message)
        {
            GameObjectEnableFadeIn(_systemMessageTextMesh.gameObject, 0.5f);
            _systemMessageTextMesh.SetText(message);
        }

        private void SetEnableGameObject(GameObject obj, bool isEnable)
        {
            obj.SetActive(isEnable);
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

        private void OnPlayerLevelUp(int newLevel) //TODO : Daha  SKILLS kısmına gelmediğim için default skill point = 1
        {
            string info = $"LEVEL UP! YOU ARE NOW LEVEL <color=yellow>{newLevel}</color> " +
                          $"AND HAVE <color=yellow>1</color> SKILL PONTS TO SPEND.";

            GameObjectEnableFadeIn(_levelUpInfoTextMesn.gameObject, 1.5f);
            _levelUpInfoTextMesn.SetText(info);
        }

        private void SetPlayerStatThresholdColor(int value, Slider slider, Color color, float threshold = 15f)
        {
            bool checkThreshold = value < slider.maxValue * threshold / 100f;
            slider.fillRect.GetComponent<Image>().color = checkThreshold ? color : Color.white;
        }

        private void SetLootTimer(int duration, float timer, bool isEnable)
        {
            _lootingTimerParent.SetActive(isEnable);
            _lootingTimerTextMesh.SetText(timer.ToString("F1"));

            _lootingTimerSlider.maxValue = duration;
            _lootingTimerSlider.value = timer;
        }

        private void ShowDamageableLayerHealth(int health, int baseHealth, string itemName)
        {
            UIStartDamageInfoTimer();

            if (health <= 0)
                UIResetDamageInfo();

            string info = $"{health}/{baseHealth} HP";

            _damageableSlider.maxValue = baseHealth;
            _damageableSlider.value = health;

            _damageableLayerHealthTextMesh.SetText(info);
            _damageableLayerName.SetText(itemName);
        }

        private void UIResetDamageInfo()
        {
            _disableDamageInfoObjectTimer = DISABLE_DAMAGE_INFO_DURATION;
            _isShowDamageInfo = false;

            _damageableCanvasGroup.DOFade(0f, 0.25f).OnComplete(() =>
            {
                _damageableCanvasGroup.gameObject.SetActive(false);
            });
        }

        private void UIStartDamageInfoTimer()
        {
            if (!_isShowDamageInfo)
            {
                _isShowDamageInfo = true;
                _damageableCanvasGroup.gameObject.SetActive(true);
                _damageableCanvasGroup.DOFade(1f, 0.25f);
            }

            _disableDamageInfoObjectTimer = DISABLE_DAMAGE_INFO_DURATION;
        }

        private void GameObjectEnableFadeIn(GameObject gameObject, float time)
        {
            SetEnableGameObject(gameObject, true);

            if (_fadeInTween != null && _fadeInTween.IsActive())
                _fadeInTween.Kill();

            _fadeInTween = gameObject.transform.DOScale(1, time)
                .OnComplete(() => SetEnableGameObject(gameObject, false));
            _fadeInTween.SetDelay(FADE_IN_DELAY_DURATION);
        }

        private void OnDestroy()
        {
            LoggingUtility.LogAction -= ShowToPlayerMessage;

            Inventory.OnItemAddedToInventory -= ItemAddedNotifier;
            InventoryWeight.OnChangeInventoryWeight -= SetInventoryWeight;

            Experience.OnChangeLevel -= OnPlayerLevelUp;
            PlayerStatHandler.OnExperienceGainedEvent -= ExperienceGainNotifier;
            LootManager.OnLootTimerUpdateEvent -= SetLootTimer;

            MeleeCombat.OnDamageApplied -= ShowDamageableLayerHealth;
        }
    }
}