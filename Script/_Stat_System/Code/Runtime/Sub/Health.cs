using _Stat_System.Runtime.Base;
using _Stat_System.Runtime.Common;
using UnityEngine;
using Zenject;

namespace _Stat_System.Runtime.Sub
{
    public class Health : Stat
    {
        public override StatType Type => StatType.Health;
        public Health(StatManager subject, StatGroup statGroup) : base(subject, statGroup) { }
        public override int Modify
        {
            get => base.Modify;
            set => base.Modify = value;
        }
    }
}

