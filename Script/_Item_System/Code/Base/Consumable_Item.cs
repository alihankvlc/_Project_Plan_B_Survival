using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project_Plan_B_Item_System.Code.Base
{
    public class Consumable_Item : Item
    {
        [SerializeField, ReadOnly]
        public virtual ConsumableType Consumable_Type { get; protected set; }
        public override ItemType _itemType => ItemType.Consumable;
    }
}