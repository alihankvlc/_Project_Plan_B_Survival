using _Inventory_System_.Code.Runtime.Common;
using _Inventory_System_.Code.Runtime.SlotManagment;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Equipment_System_.Runtime.Base
{
    public abstract class EquipmentControll : SerializedMonoBehaviour
    {
        [SerializeField, ReadOnly] protected bool IsEquipped;
        [SerializeField, ReadOnly, InlineEditor] protected SlotItem EquippedSlotItem;
        protected Animator PlayerAnimator;//TODO: Animator componentini inject edicem.

        public virtual void Start()
        {
            PlayerAnimator = GetComponent<Animator>();

            ToolBelt.OnItemEquipped += ItemEquipmentHandler;
            ToolBelt.OnItemUnequipped += ItemUnEquipmentHandler;
        }

        public virtual void ItemEquipmentHandler(SlotItem data) { }
        public virtual void ItemUnEquipmentHandler(SlotItem data) { }
        public virtual void OnDestroy() => ToolBelt.OnItemEquipped -= ItemEquipmentHandler;
    }
}
