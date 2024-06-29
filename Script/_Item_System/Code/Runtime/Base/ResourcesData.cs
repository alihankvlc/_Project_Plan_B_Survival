using Sirenix.OdinInspector;
using UnityEngine;

namespace _Item_System_.Runtime.Base
{
    [CreateAssetMenu(fileName = "New_Resources", menuName = "_Project_Plan_B/Create Item/Resources")]
    public class ResourcesData : ItemData
    {
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (!_smeltable) _smeltResult = SmeltResult.None;
            if (!_combustible) _burningTime = 0.0f;
        }
#endif

        public override ItemType _itemType => ItemType.Resources;

        [Header("Resources Settings")] [SerializeField]
        private bool _smeltable;

        [SerializeField, ShowIf("@_smeltable")]
        private SmeltResult _smeltResult;

        [SerializeField] private bool _combustible;

        [SerializeField, ShowIf("@_combustible")]
        private float _burningTime;
    }
}