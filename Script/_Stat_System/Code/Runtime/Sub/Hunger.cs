using _Stat_System.Runtime.Base;
using _Stat_System.Runtime.Common;
using UnityEngine;

namespace _Stat_System.Runtime.Sub
{
    public class Hunger : Stat
    {
        public override StatType Type => StatType.Hunger;
        public Hunger(StatObserverManager subject, StatGroup statGroup) : base(subject, statGroup) { }
    }
}

