using _Inventory_System_.Code.Runtime.Common;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Player_System_.Runtime.Common
{   // TODO: Şuan AnimEvent ile equip oluyor bunu rigging sistemine uyarlamayı düşünüyorum...
    public sealed class WeaponEquipmentController : EquipmentControll
    {
        public override void ItemEquipmentHandler(SlotItem slotItem)
        {
            if (slotItem.Data is WeaponData weaponData && !IsEquipped)
                weaponData.EquipWeapon(PlayerAnimator, ref IsEquipped);
        }

        public override void ItemUnEquipmentHandler(SlotItem slotItem)
        {
            if (slotItem.Data is WeaponData weaponData && IsEquipped)
                weaponData.UnequipWeapon(PlayerAnimator, ref IsEquipped);
        }
    }
}

