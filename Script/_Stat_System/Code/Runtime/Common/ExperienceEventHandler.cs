using System;
using UnityEngine;

namespace _Stat_System.Runtime.Common
{
    public class ExperienceEventHandler : EventArgs
    {
        public int CurrentExperience { get; private set; }
        public int RequirementExperience { get; private set; }
        public int Level { get; private set; }
        
        public ExperienceEventHandler(int level, int currentExperience, int requirementExperience)
        {
            Update(level, currentExperience, requirementExperience);
        }

        public void Update(int level, int currentExperience, int requirementExperience)
        {
            Level = level;
            CurrentExperience = currentExperience;
            RequirementExperience = requirementExperience;
        }
    }
}