using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Crafting_System_.Runtime.Common
{
    [System.Serializable]
    public class CraftingRequirement
    {
        [SerializeField] private ItemData _itemData;
        [SerializeField] private int _quantity;
        [SerializeField] private int _requirementPlayerLevel;

        public ItemData Data => _itemData;
        public int Quantity => _quantity;
        public int RequirementPlayerLevel => _requirementPlayerLevel;

        public bool CheckPlayerLevel(int playerLevel) => _requirementPlayerLevel >= playerLevel;
    }
}

