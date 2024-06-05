using _Inventory_System_.Code.Runtime.Common;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Item_System_.Runtime.Base;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Equipment_System_.Runtime.Base
{
    public abstract class EquipmentControll : MonoBehaviour
    {
        [SerializeField, ReadOnly] protected bool IsEquipped;
        protected Animator PlayerAnimator;//TODO: Animator componentini bind edecem.

        public virtual void Start()
        {
            PlayerAnimator = GetComponent<Animator>();

            ToolBelt.OnItemEquipped += ItemEquipmentHandler;
            ToolBelt.OnItemUnequipped += ItemUnEquipmentHandler;
        }

        public virtual void ItemEquipmentHandler(SlotItem data)
        {
        }
        public virtual void ItemUnEquipmentHandler(SlotItem data)
        {

        }

        public virtual void OnDestroy()
        {
            ToolBelt.OnItemEquipped -= ItemEquipmentHandler;
        }
    }
}
