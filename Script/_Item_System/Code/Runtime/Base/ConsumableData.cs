using Sirenix.OdinInspector;
using UnityEngine;

namespace _Item_System_.Runtime.Base
{
    public abstract class ConsumableData : ItemData
    {
        [SerializeField, ReadOnly] public virtual ConsumableType Consumable_Type { get; protected set; }
        public override ItemType _itemType => ItemType.Consumable;
        public ConsumableType ConsumableType => Consumable_Type;

        public virtual void Consume()
        {

        }
    }
}