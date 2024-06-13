using System;
using _Stat_System.Runtime.Base;
using _Stat_System.Runtime.Common;
using UnityEngine;
using Zenject;

namespace _Stat_System.Runtime.Sub
{
    public class Experience : Stat
    {
        private int _level = 1;
        private int _currentExperience;
        private int _requirementExperience;

        private ExperienceEventHandler _experienceEventHandler;

        public static event EventHandler<ExperienceEventHandler> OnChangeExperience;
        public static event Action<int> OnChangeLevel;
        
        public override StatType Type => StatType.Health;

        public Experience(StatObserverManager subject, StatGroup statGroup) : base(subject, statGroup)
        {
            _experienceEventHandler = new(_level, _currentExperience, _requirementExperience);
        }

        public int CurrentExp => _currentExperience;

        public override int Modify
        {
            get => _currentExperience;
            set
            {
                _currentExperience = value;
                
                while (_currentExperience >= _requirementExperience)
                {
                    _level++;
                    OnChangeLevel?.Invoke(_level);

                    _currentExperience -= _requirementExperience;
                    _requirementExperience = _level * 45 + 150;
                }

                NotifyExperienceChanged();
            }
        }

        public void InitializeRequirementExp(int exp)
        {
            _requirementExperience = exp;
        }

        public int RequirementExp
        {
            get => _requirementExperience;
            private set => _requirementExperience = value;
        }

        public int Level
        {
            get => _level;
            private set => _level = value;
        }

        private void NotifyExperienceChanged()
        {
            _experienceEventHandler.Update(_level, _currentExperience, _requirementExperience);
            OnChangeExperience?.Invoke(this, _experienceEventHandler);
        }
    }
}