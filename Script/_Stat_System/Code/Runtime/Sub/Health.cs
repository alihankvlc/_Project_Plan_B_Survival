using _Stat_System.Runtime.Base;
using _Stat_System.Runtime.Common;
using UnityEngine;
using Zenject;

namespace _Stat_System.Runtime.Sub
{
    public class Health : Stat
    {
        public override StatType Type => StatType.Health;
        public Health(StatObserverManager subject, StatGroup statGroup) : base(subject, statGroup) { }
        
        public override int Modify
        {
            get => base.Modify;
            set => base.Modify = value;
        }
        public (bool isDead, int health) GetStatus()
        {
            bool isDead = Value <= 0;
            int health = Value;
            
            return (isDead, health);
        }
    }
}