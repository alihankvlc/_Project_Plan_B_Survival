using _Stat_System.Runtime.Base;
using _Stat_System.Runtime.Common;
using UnityEngine;

namespace _Stat_System.Runtime.Sub
{
    public class Stamina : Stat
    {
        public override StatType Type => StatType.Stamina;
        public Stamina(StatManager subject, StatGroup statGroup) : base(subject, statGroup) { }
    }
}

