using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using UnityEngine;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Sub.Consumable
{
    [CreateAssetMenu(fileName = "New_Medkit", menuName = "_Project_Plan_B/Create Item/Consumable/Medkit")]
    public sealed class Medkit : ConsumableData
    {
        [SerializeField] private int _regenHealthAmount;
        public override ConsumableType Consumable_Type => ConsumableType.Medkit;
        public int RegenHealthAmount => _regenHealthAmount;

        public void Set_Item_MedKit_RegenHealthAmount(int amount)
        {
            _regenHealthAmount = amount;
        }
    }
}