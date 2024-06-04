using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Consumable.Resources
{
    [CreateAssetMenu(fileName = "New_Iron", menuName = "_Project_Plan_B/Create Item/Resources/Iron")]
    public sealed class Iron : ResourcesData
    {
        public override ResourcesType Resources_Type => ResourcesType.Iron;
    }
}