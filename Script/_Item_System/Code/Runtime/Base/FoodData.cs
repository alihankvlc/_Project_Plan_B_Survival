using UnityEngine;

namespace _Item_System_.Runtime.Base
{
    public abstract class FoodData : ConsumableData
    {
        public override ConsumableType Consumable_Type => ConsumableType.Food;
    }

}
