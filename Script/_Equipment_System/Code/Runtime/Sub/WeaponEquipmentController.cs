using System;
using _Equipment_System_.Runtime.Base;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Item_System_.Runtime.Base;
using UnityEngine;


namespace _Equipment_System_.Runtime.Sub
{
    // TODO: Şuan AnimEvent ile equip oluyor bunu rigging sistemine uyarlamayı düşünüyorum...
    public sealed class WeaponEquipmentController : EquipmentControll
    {
        public static event Action<SlotItem> OnEquipmentWeapon;

        public override void ItemEquipmentHandler(SlotItem slotItem)
        {
            if (slotItem.Data is WeaponData weaponData && !IsEquipped)
            {
                EquipmentPlayAnimation(weaponData, true);
                OnEquipmentWeapon?.Invoke(slotItem);

                EquippedSlotItem = slotItem;
                IsEquipped = true;
            }
        }

        public override void ItemUnEquipmentHandler(SlotItem slotItem)
        {
            if (slotItem.Data is WeaponData weaponData && IsEquipped)
            {
                EquipmentPlayAnimation(weaponData, false);
                OnEquipmentWeapon?.Invoke(null);
                
                EquippedSlotItem = null;
                IsEquipped = false;
            }
        }

        private void EquipmentPlayAnimation(WeaponData data, bool isEquip)
        {
            PlayerAnimator.SetBool(data.EQUIP_HASH_ID, isEquip);
        }
    }
}