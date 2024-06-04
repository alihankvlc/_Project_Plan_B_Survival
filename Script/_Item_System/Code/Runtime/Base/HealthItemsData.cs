using UnityEngine;

namespace _Item_System_.Runtime.Base
{
    public abstract class HealthItemsData : ConsumableData
    {
        [Header("Health Items Settings")]
        [SerializeField] private int _regenHealthAmount;
        [SerializeField] private int _applyDuration;
        //Tedavi edebilecek hasatlıklar olcak.
        public override ConsumableType Consumable_Type => ConsumableType.HealthItems;
        public virtual HealthItemType HealthItemType { get; protected set; }
    }
}