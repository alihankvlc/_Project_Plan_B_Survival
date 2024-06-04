using _Database_System_.Code.Runtime;
using _Item_System_.Runtime.Base;
using UnityEditor;
using UnityEngine;

namespace _Item_System_.Runtime.Database
{
    public interface ItemDatabaseProvider
    {
        public ItemData GetItemData(int id);
    }

    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "_Project_Plan_B/Database/Handle Create ItemDatabase")]
    public sealed class ItemDatabase : Database<ItemDatabase, ItemData>, ItemDatabaseProvider
    {
#if UNITY_EDITOR

#endif
        private static ItemDatabase _instance;
        public static ItemDatabase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<ItemDatabase>("ItemDatabase");
                    if (_instance == null)
                    {
                        string path = $"Assets/_Project_Plan_B_Survival_Main/ScriptableObjects/Database/ItemDatabase.asset";

                        _instance = CreateInstance<ItemDatabase>();
                        AssetDatabase.CreateAsset(_instance, path);
                    }
                }
                return _instance;
            }
        }

        public ItemData GetItemData(int id)
        {
            ItemData itemData = GetData(id);
            return itemData;
        }
    }
}