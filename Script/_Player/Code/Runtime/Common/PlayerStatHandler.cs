using System;
using _Other_.Runtime.Code;
using _Stat_System.Runtime.Base;
using _Stat_System.Runtime.Sub;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Player_System_.Runtime.Common
{
    public interface IPlayerExperienceHandler
    {
        public int CurrentExperience { get; }
        public int RequirementExperience { get; }
        public int Level { get; }
        public int GrantExperiencePoint { get; set; }
    }

    public class PlayerStatHandler : MonoBehaviour, IPlayerExperienceHandler
    {
#if UNITY_EDITOR
        [SerializeField] private int _currentExperience;
        [SerializeField] private int _requirementExperience;
        [SerializeField] private int _level;
#endif
        private const int START_EXPERIENCE_VALUE = 45;
        private const int START_PLAYER_BASE_VALUE = 100;

        [Inject(Id = StatGroup.Player)] private Health _health;
        [Inject(Id = StatGroup.Player)] private Experience _experience;

        public static event Action<int> OnExperienceGainedEvent;

        public int CurrentExperience => _experience.CurrentExp;
        public int RequirementExperience => _experience.RequirementExp;
        public int Level => _experience.Level;

        public int GrantExperiencePoint
        {
            get => _experience.CurrentExp;
            set { GrantExperiencePoints(value); }
        }

        private void Start()
        {
            _health.ModifyBaseValue(START_PLAYER_BASE_VALUE);
            _experience.InitializeRequirementExp(START_EXPERIENCE_VALUE);
        }

        private void Update()
        {
#if UNITY_EDITOR
            _currentExperience = _experience.CurrentExp;
            _requirementExperience = _experience.RequirementExp;
            _level = _experience.Level;
#endif
        }

        public void TakeDamage(int amount)
        {
            _health.Modify -= amount;
            if (_health.GetStatus().isDead) Dead();
        }

        public void GrantExperiencePoints(int amount)
        {
            _experience.Modify += amount;
            OnExperienceGainedEvent?.Invoke(amount);
        }

        private void Dead()
        {
            //Dolduracam...
        }
    }
}