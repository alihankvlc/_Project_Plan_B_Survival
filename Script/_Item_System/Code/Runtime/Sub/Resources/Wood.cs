using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using UnityEngine;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Sub.Resources
{
    [CreateAssetMenu(fileName = "New_Wood", menuName = "_Project_Plan_B/Create Item/Resources/Wood")]
    public sealed class Wood : ResourcesData
    {
        public override ResourcesType Resources_Type => ResourcesType.Wood;
    }
}