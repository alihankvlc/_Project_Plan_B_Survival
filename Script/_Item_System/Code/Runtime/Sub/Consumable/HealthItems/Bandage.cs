using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Consumable.HealthItems
{
    [CreateAssetMenu(fileName = "New_HealthItem_Bandage", menuName = "_Project_Plan_B/Create Item/Consumable/HealthItems/Bandage")]
    public class Bandage : HealthItemsData
    {
        public override HealthItemType HealthItemType => HealthItemType.Bandage;

        public override void Consume()
        {
            base.Consume();
        }
    }
}

