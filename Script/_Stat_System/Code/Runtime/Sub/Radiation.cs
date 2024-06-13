using _Stat_System.Runtime.Base;
using _Stat_System.Runtime.Common;
using UnityEngine;

namespace _Stat_System.Runtime.Sub
{
    public class Radiation : Stat
    {
        public override StatType Type => StatType.Radiation;
        public Radiation(StatObserverManager subject, StatGroup statGroup) : base(subject, statGroup) { }
    }
}

