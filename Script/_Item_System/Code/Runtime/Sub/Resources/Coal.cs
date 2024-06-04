using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Consumable.Resources
{
    [CreateAssetMenu(fileName = "New_Coal", menuName = "_Project_Plan_B/Create Item/Resources/Coal")]
    public sealed class Coal : ResourcesData
    {
        public override ResourcesType Resources_Type => ResourcesType.Coal;
    }
}