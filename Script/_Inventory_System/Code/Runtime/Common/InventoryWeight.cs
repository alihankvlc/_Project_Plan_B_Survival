using System;
using UnityEngine;

namespace _Inventory_System_.Code.Runtime.Common
{
    public sealed class InventoryWeight
    {
        private float _maxWeight;
        private float _currentWeight;

        public float MaxWeight
        {
            get => _maxWeight;
            private set => _maxWeight = value;
        }

        public float CurrentWeight
        {
            get => _currentWeight;
            private set
            {
                float roundedValue = (float)Math.Round(value, 1);
                _currentWeight = roundedValue;
                
                OnChangeInventoryWeight?.Invoke(_currentWeight, _maxWeight);

                if (_currentWeight > _maxWeight)
                    OnMaxCapacityReached?.Invoke();
            }
        }

        public static event Action<float, float> OnChangeInventoryWeight;
        public static event Action OnMaxCapacityReached;

        public void SetMaxWeight(float maxWeight) => _maxWeight = maxWeight;

        public void DecreaseWeight(int amount, ref float weight)
        {
            CurrentWeight -= amount;
            weight = _currentWeight;
        }

        public void IncreaseWeight(float amount, ref float weight)
        {
            CurrentWeight += amount;
            weight = _currentWeight;
        }
    }
}
