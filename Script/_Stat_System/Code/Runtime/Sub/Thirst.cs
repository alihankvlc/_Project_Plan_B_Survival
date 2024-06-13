using _Stat_System.Runtime.Base;
using _Stat_System.Runtime.Common;
using UnityEngine;

namespace _Stat_System.Runtime.Sub
{
    public class Thirst : Stat
    {
        public override StatType Type => StatType.Thirst;
        public Thirst(StatObserverManager subject, StatGroup statGroup) : base(subject, statGroup) { }
    }
}

