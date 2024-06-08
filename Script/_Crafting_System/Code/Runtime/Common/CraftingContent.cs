using UnityEngine;
using UnityEngine.UI;

namespace _Crafting_System_.Runtime.Common
{
    public class CraftingContent : MonoBehaviour
    {
        [SerializeField] private Transform _slotContainer;
        [SerializeField] private Transform _pageContainer;

        public Transform SlotContainer => _slotContainer;
        public Transform PageContainer => _pageContainer;
    }
}

