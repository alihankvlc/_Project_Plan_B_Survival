using _Inventory_System_.Code.Runtime.SlotManagment;
using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Consumable.HealthItems
{
    [CreateAssetMenu(fileName = "New_HealthItem_Vitamin", menuName = "_Project_Plan_B/Create Item/Consumable/HealthItems/Vitamin")]
    public class Vitamin : HealthItemsData
    {
        public override HealthItemType HealthItemType => HealthItemType.Vitamins;

        public override void Consume()
        {
            base.Consume();
        }
    }
}

