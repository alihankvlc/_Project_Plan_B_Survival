using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Base
{
    public abstract class ConsumableData : ItemData
    {
        [SerializeField, ReadOnly] public virtual ConsumableType Consumable_Type { get; protected set; }
        public override ItemType _itemType => ItemType.Consumable;

        public ConsumableType ConsumableType => Consumable_Type;
    }
}