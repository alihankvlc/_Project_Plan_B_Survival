using _Project_Plan_B_Common;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Project_Plan_B_Survival_Database.Code
{
    public interface IData
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public Sprite Icon { get; }
    }

    public abstract class Database<T1, T2> : DeletableScriptableObject where T2 : IData where T1 : Database<T1, T2>
    {
        private const string _dataFolderPath = "Assets/_Project_Plan_B_Survival_Main/ScriptableObjects/";

        [SerializeField, InlineEditor, Searchable] protected List<T2> Datas = new List<T2>();
        [SerializeField, ReadOnly] protected Dictionary<int, T2> Cache = new Dictionary<int, T2>();

        public List<T2> Get_Data_List => Datas;
        public bool ContainsItem(int id) => Cache.ContainsKey(id);

        public virtual void Init()
        {
            Cache = Datas.ToDictionary(r => r.Id);
        }

#if UNITY_EDITOR

        [Button("Populate Data")]
        public void Load()
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T2)}", new[] { _dataFolderPath });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(T2));

                if (obj is T2 data)
                    AddItem(data);
            }
        }

        [Button("Clear Data")]
        public void Clear()
        {
            if (Cache != null && Datas != null)
            {
                Cache.Clear();
                Datas.Clear();
            }
        }

        public void AddItem(T2 item)
        {
            if (!Datas.Contains(item) && !Cache.ContainsKey(item.Id))
            {
                Datas.Add(item);
                Cache[item.Id] = item;
                EditorUtility.SetDirty(this);
            }
        }

        public void RemoveItem(int id)
        {
            if (Cache.TryGetValue(id, out T2 item))
            {
                Datas.Remove(item);
                Cache.Remove(id);
                EditorUtility.SetDirty(this);
            }
        }
#endif
        public T2 GetData(int id)
        {
            if (Cache.TryGetValue(id, out T2 existingData))
                return existingData;

            Debug.Log($"Database in id={id} null ? ?? ");
            return default(T2);
        }
    }
}