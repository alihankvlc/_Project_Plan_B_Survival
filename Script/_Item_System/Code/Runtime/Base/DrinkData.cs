using UnityEngine;

namespace _Item_System_.Runtime.Base
{
    public abstract class DrinkData : ConsumableData
    {
        public override ConsumableType Consumable_Type => ConsumableType.Drink;
    }

}
