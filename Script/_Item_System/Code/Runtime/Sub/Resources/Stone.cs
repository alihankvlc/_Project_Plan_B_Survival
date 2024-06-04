using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Consumable.Resources
{
    [CreateAssetMenu(fileName = "New_Stone", menuName = "_Project_Plan_B/Create Item/Resources/Stone")]
    public sealed class Stone : ResourcesData
    {
        public override ResourcesType Resources_Type => ResourcesType.Stone;
    }
}