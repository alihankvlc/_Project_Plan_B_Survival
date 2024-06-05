using _Inventory_System_.Code.Runtime.SlotManagment;
using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Consumable.HealthItems
{
    [CreateAssetMenu(fileName = "New_HealthItem_Painkiller", menuName = "_Project_Plan_B/Create Item/Consumable/HealthItems/Painkiller")]
    public class Painkiller : HealthItemsData
    {
        public override HealthItemType HealthItemType => HealthItemType.Painkillers;


        public override void Consume()
        {
            base.Consume();
        }
    }
}

