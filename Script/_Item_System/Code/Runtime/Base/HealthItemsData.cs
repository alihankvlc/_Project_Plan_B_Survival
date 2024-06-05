using _Inventory_System_.Code.Runtime.SlotManagment;
using UnityEngine;

namespace _Item_System_.Runtime.Base
{
    //TODO: sUB sınıfların Tedavi edebileceği hastalıklar olcak.
    public abstract class HealthItemsData : ConsumableData
    {
        [Header("Health Items Settings")]
        [SerializeField] private int _regenHealthAmount;
        [SerializeField] private int _applyDuration;

        public override ConsumableType Consumable_Type => ConsumableType.HealthItems;
        public virtual HealthItemType HealthItemType { get; protected set; }

        public override void Consume()
        {
        }
    }
}