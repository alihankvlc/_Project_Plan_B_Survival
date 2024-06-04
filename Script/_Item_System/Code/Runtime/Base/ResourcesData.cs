using Sirenix.OdinInspector;
using UnityEngine;

namespace _Item_System_.Runtime.Base
{
    public abstract class ResourcesData : ItemData
    {
        public override ItemType _itemType => ItemType.Resources;
        [SerializeField, ReadOnly] public virtual ResourcesType Resources_Type { get; protected set; }
    }
}