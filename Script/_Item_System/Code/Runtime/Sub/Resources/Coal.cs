using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using UnityEngine;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Sub.Resources
{
    [CreateAssetMenu(fileName = "New_Coal", menuName = "_Project_Plan_B/Create Item/Resources/Coal")]
    public sealed class Coal : ResourcesData
    {
        public override ResourcesType Resources_Type => ResourcesType.Coal;
    }
}