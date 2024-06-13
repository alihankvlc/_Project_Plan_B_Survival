using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Equipment_System_.Runtime.Sub;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Item_System_.Runtime.Base;
using UnityEngine;

namespace _Equipment_System_.Runtime.Common
{
    [System.Serializable]
    public class EquipmentMelee
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private MeleeType _meleeType;

        public GameObject Prefab => _prefab;
        public MeleeType MeleeType => _meleeType;
    }


    public class EquipmentWeaponHandler : MonoBehaviour
    {
        [SerializeField] private Transform _containerTransform;
        [SerializeField] private List<EquipmentMelee> _equipmentMelees = new();

        private EquipmentMelee _activeMelee;

        private void Start()
        {
            WeaponEquipmentController.OnEquipmentWeapon += EquipmentWeapon;
        }

        private void EquipmentWeapon(SlotItem slotItem)
        {
            if (slotItem == null && _activeMelee != null)
            {
                _activeMelee.Prefab.SetActive(false); //TODO : TEST Amaçlı bu şekilde düzelticem...
                return;
            }

            if (slotItem.Data is MeleeData meleeData)
            {
                EquipmentMelee melee = _equipmentMelees.FirstOrDefault(r => r.MeleeType == meleeData.MeleeType);

                _activeMelee = melee;
                _activeMelee.Prefab.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            WeaponEquipmentController.OnEquipmentWeapon -= EquipmentWeapon;
        }
    }
}