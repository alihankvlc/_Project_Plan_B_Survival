using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Base
{
    [CreateAssetMenu(fileName = "New_Firearm", menuName = "_Project_Plan_B/Create Item/Weapon/Firearm")]
    public abstract class FirearmData : WeaponData
    {
        [Header("Firearm Settings")]
        [SerializeField] private AmmoType _ammoCategory;
        [SerializeField, Range(2f, 25f)] private float _reloadTime;
        public override WeaponType WeaponType => WeaponType.Firearm;
        [SerializeField, ReadOnly] public virtual FirearmType FirearmType { get; protected set; }
    }
}