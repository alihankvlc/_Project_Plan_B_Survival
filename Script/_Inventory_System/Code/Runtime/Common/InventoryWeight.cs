using System;
using UnityEngine;
namespace _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Common
{
    public class InventoryWeight
    {
        private int _maxWeight;
        private int _currentWeight;

        public int MaxWeight
        {
            get => _maxWeight;
            private set => _maxWeight = value;
        }

        public int CurrentWeight
        {
            get => _currentWeight;
            private set
            {
                _currentWeight = value;
                OnChangeInventoryWeight?.Invoke(_currentWeight);

                if (_currentWeight > _maxWeight)
                    OnMaxCapacityReached?.Invoke();
            }
        }

        public static event Action<int> OnChangeInventoryWeight;
        public static event Action OnMaxCapacityReached;

        public InventoryWeight(int maxWeight) => SetMaxWeight(maxWeight);

        public void SetMaxWeight(int maxWeight) => _maxWeight = maxWeight;

        public void DecreaseWeight(int amount, ref int weight)
        {
            CurrentWeight -= amount;
            weight = _currentWeight;
        }

        public void IncreaseWeight(int amount, ref int weight)
        {
            CurrentWeight += amount;
            weight = _currentWeight;
        }
    }
}
