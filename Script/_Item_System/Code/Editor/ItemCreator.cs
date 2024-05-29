using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using _Project_Plan_B_Survival_Item_System.Runtime.Database;
using _Project_Plan_B_Survival_Item_System.Runtime.Sub.Ammo;
using _Project_Plan_B_Survival_Item_System.Runtime.Sub.Consumable;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace _Project_Plan_B_Survival_Item_System.Editor
{
    public class ItemCreator : EditorWindow
    {
        private Vector2 _scrollPosition;

        #region ItemData_General_Settings

        private string _mainPath = "Assets/_Project_Plan_B_Survival_Main/ScriptableObjects/";

        //Display-Variables-Start
        private string _displayName;
        private string _displayDescription;
        private Sprite _displayIcon;
        //Display-Variables-End

        //Data-Variables-Start
        private ItemType _itemType;
        private ObtainableType _obtainableType;

        private int _dataId;
        private int _weight;
        private int _stackCapacity;
        private bool _isStackable;

        private bool _isScrappable;

        //Data-Variables-End
        #endregion
        #region Ammo_Data_Settings

        private int _ammoDamageBonus;
        private int _ammoSpeed;
        private AmmoType _ammoType;

        #endregion
        #region Consumable_Data_Settings
        //Medkit-Variables-Start
        private ConsumableType _consumableType;
        private int _regenHealthAmount;
        //Medkit-Variables-End

        #endregion

        [MenuItem("Tools/Item Data Creator")]
        public static void ShowWindow()
        {
            GetWindow<ItemCreator>();
        }

        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            AddLabel("Item Data Main Path");
            _mainPath = EditorGUILayout.TextField("Main Path", _mainPath);

            #region Item_Data_General_GUI
            AddLabel("Display Settings");
            _displayName = EditorGUILayout.TextField("Display Name", _displayName);
            _displayDescription = EditorGUILayout.TextField("Display Decsription", _displayDescription);

            AddLabel("Data Settings");
            _itemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", _itemType);
            _obtainableType = (ObtainableType)EditorGUILayout.EnumFlagsField("Obtainable", _obtainableType);
            _dataId = EditorGUILayout.IntField("Data Id", _dataId);
            _weight = EditorGUILayout.IntField("Weight", _weight);
            _isScrappable = EditorGUILayout.Toggle("Scrappable", _isScrappable);
            _isStackable = EditorGUILayout.Toggle("Stackkable", _isStackable);

            if (_isStackable)
                _stackCapacity = EditorGUILayout.IntField("Stack Capacity", _stackCapacity);

            _displayIcon = EditorGUILayout.ObjectField("Icon", _displayIcon, typeof(Sprite), false) as Sprite;

            switch (_itemType)
            {
                case ItemType.Consumable:
                    switch (_consumableType)
                    {
                        case ConsumableType.Medkit:
                            _consumableType = (ConsumableType)EditorGUILayout.EnumPopup("Consumable Type", _consumableType);
                            _regenHealthAmount = EditorGUILayout.IntField("Regen Health Amount", _regenHealthAmount);
                            break;
                    }
                    break;
                case ItemType.Weapon:
                    break;
                case ItemType.Ammo:
                    AddLabel("Ammo Settings");
                    _ammoType = (AmmoType)EditorGUILayout.EnumPopup("Ammo Type", _ammoType);
                    _ammoDamageBonus = EditorGUILayout.IntField("Ammo Damage Bonus", _ammoDamageBonus);
                    _ammoSpeed = EditorGUILayout.IntField("Ammo Speed", _ammoSpeed);
                    break;
                case ItemType.Resources:
                    break;
            }


            if (GUILayout.Button("Create Ammo Data") && IsCreatable())
                Create(_itemType);

            #endregion

            EditorGUILayout.EndScrollView();
        }
        private bool IsCreatable()
        {
            if (ItemDatabase.Instance.ContainsItem(_dataId))
            {
                ThrowDebugMessage($"There is an item with the same ID={_dataId}");
                return false;
            }
            else if (string.IsNullOrEmpty(_displayName) || _displayName.Length < 1)
            {
                ThrowDebugMessage($"Building name is not be null and name lengt must be greater than zero");
                return false;
            }


            return true;
        }
        private void Create(ItemType itemType)
        {
            string fullPath = $"{_mainPath}/{itemType}";
            CheckEnsurePathExists(fullPath);

            CreateItemData(_itemType, fullPath);
            ResetAllVariables();
        }

        private void CheckEnsurePathExists(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                string parentPath = Path.GetDirectoryName(path);

                if (!AssetDatabase.IsValidFolder(parentPath))
                    CheckEnsurePathExists(parentPath);

                string folderName = Path.GetFileName(path);
                AssetDatabase.CreateFolder(parentPath, folderName);
            }
        }
        private void CreateItemData(ItemType itemType, string dataPath)
        {
            string finalDataPath = dataPath + "/" + _displayName + ".asset";

            if (File.Exists(finalDataPath))
            {
                ThrowDebugMessage($"Item with the name {_displayName} already exists in the specified path.");
                return;
            }

            switch (itemType)
            {
                case ItemType.Consumable:
                    ConsumableData consumableData = SetItemGeneralSettings(SetConsumableData(_consumableType)) as ConsumableData;
                    switch (consumableData.ConsumableType)
                    {
                        case ConsumableType.Medkit:
                            Medkit medkit = consumableData as Medkit;
                            medkit.Set_Item_MedKit_RegenHealthAmount(_regenHealthAmount);
                            break;
                    }

                    FinalCreateItemData(consumableData, finalDataPath);

                    break;
                case ItemType.Weapon:
                    break;
                case ItemType.Ammo:
                    AmmoData ammoData = SetItemGeneralSettings(SetAmmoData(_ammoType)) as AmmoData;
                    ammoData.Set_Item_Ammo_Damage_Bonus(_ammoDamageBonus);
                    ammoData.Set_Item_Ammo_Speed(_ammoSpeed);

                    FinalCreateItemData(ammoData, finalDataPath);

                    break;
                case ItemType.Resources:
                    break;
            }
        }
        private void FinalCreateItemData(ItemData data, string path)
        {
            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = data;
            ItemDatabase.Instance.AddItem(data);
        }
        private AmmoData SetAmmoData(AmmoType type)
        {
            AmmoData ammoData = type switch
            {
                AmmoType.RifleBullet => CreateInstance<RifleBullet>(),
                AmmoType.PistolBullet => CreateInstance<PistolBullet>(),
                AmmoType.BowBullet => CreateInstance<BowBullet>(),
                AmmoType.ShotgunBullet => CreateInstance<ShotgunBullet>(),
                _ => null,
            };

            return ammoData;
        }
        private ConsumableData SetConsumableData(ConsumableType type)
        {
            ConsumableData consumableData = type switch
            {
                ConsumableType.Medkit => CreateInstance<Medkit>(),
                _ => null,
            };

            return consumableData;
        }
        private ItemData SetItemGeneralSettings(ItemData itemData)
        {
            itemData.Set_Item_Id(_dataId);
            itemData.Set_Item_Display_Name(_displayName);
            itemData.Set_Item_Display_Description(_displayDescription);
            itemData.Set_Item_Icon(_displayIcon);
            itemData.Set_Item_Type(_itemType);
            itemData.Set_Item_IsStackable(_isStackable);
            itemData.Set_Item_Stack_Capacity(!_isStackable ? 1 : _stackCapacity);
            itemData.Set_Item_IsSrappable(_isScrappable);
            itemData.Set_Item_ObtainableType(_obtainableType);
            itemData.Set_Item_Weight(_weight);

            return itemData;
        }

        private void ResetAllVariables()
        {
            _displayName = string.Empty;
            _displayDescription = string.Empty;
            _displayIcon = null;

            _dataId = 0;
            _stackCapacity = 1;

            _isStackable = false;
            _isScrappable = false;

            _ammoDamageBonus = 0;
            _ammoSpeed = 0;
            _ammoType = AmmoType.RifleBullet;

            _regenHealthAmount = 0;
        }

        private void AddLabel(string labelName)
        {
            GUILayout.Space(5);
            EditorGUILayout.LabelField(labelName, EditorStyles.boldLabel);
        }

        private void ThrowDebugMessage(string message)
        {
            string info = $"<color=cyan>{message}</color>";
            Debug.LogWarning(info);
        }
    }
}