using _Inventory_System_.Code.Runtime.SlotManagment;
using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Consumable.HealthItems
{
    [CreateAssetMenu(fileName = "New_HealthItem_Antibiotics", menuName = "_Project_Plan_B/Create Item/Consumable/HealthItems/Antibiotics")]
    public class Antibiotics : HealthItemsData
    {
        public override HealthItemType HealthItemType => HealthItemType.Antibiotics;

        public override void Consume()
        {
            base.Consume();
        }
    }
}

