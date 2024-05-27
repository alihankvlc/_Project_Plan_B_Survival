using UnityEngine;

namespace _Project_Plan_B_Item_System.Code.Base
{
    [CreateAssetMenu(fileName = "New_Medkit", menuName = "_Project_Plan_B/Create Item/Consumable/Medkit")]
    public class Medkit : Consumable_Item
    {
        [SerializeField] private int _regenHealthAmount;
        public override ConsumableType Consumable_Type => ConsumableType.Medkit;

    }
}