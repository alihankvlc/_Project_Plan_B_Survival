using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Item_System_.Runtime.Sub.Consumable.Resources
{
    [CreateAssetMenu(fileName = "New_Wood", menuName = "_Project_Plan_B/Create Item/Resources/Wood")]
    public sealed class Wood : ResourcesData
    {
        public override ResourcesType Resources_Type => ResourcesType.Wood;
    }
}