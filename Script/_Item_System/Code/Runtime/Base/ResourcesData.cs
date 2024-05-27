using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Base
{
    public abstract class ResourcesData : ItemData
    {
        public override ItemType _itemType => ItemType.Resources;
        [SerializeField, ReadOnly] public virtual ResourcesType Resources_Type { get; protected set; }
    }
}