using _Stat_System.Runtime.Sub;
using UnityEngine;
using Zenject;
namespace _Player_System_.Runtime.Common
{
    public interface IDamageable
    {
        public void TakeDamage(int amount);
    }

    public class PlayerStatHandler : MonoBehaviour, IDamageable
    {
        [Header("Stat/Health Settings")]
        [SerializeField] private int _baseValue;
        [SerializeField] private bool _isAlive = true;

        [Inject(Id = "PlayerHealth")] private Health _health;

        public int tempTakeDamage;

        private void Start()
        {
            _health.ModifyBaseValue(_baseValue);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(tempTakeDamage);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                _health.Modify += tempTakeDamage;
            }
        }

        public void TakeDamage(int amount)
        {
            _health.Modify -= amount;
            _isAlive = _health.Value > 0;
        }
    }
}

