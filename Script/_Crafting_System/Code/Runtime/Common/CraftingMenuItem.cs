using TMPro;
using UnityEngine;

namespace _Crafting_System_.Runtime.Common
{
    [RequireComponent(typeof(CraftingContent))]
    public class CraftingMenuItem : MonoBehaviour
    {
        [SerializeField] private CraftingType _craftingType;
        [SerializeField] private CraftingContent _craftingContent;
        [SerializeField] private TextMeshProUGUI _headerTextMesh;

        public CraftingType CraftingType => _craftingType;
        public CraftingContent CraftingContent => _craftingContent;


        public void SetCraftingType(CraftingType type)
        {
            _craftingType = type;
            _headerTextMesh?.SetText(_craftingType.ToString());
        }
    }
}
