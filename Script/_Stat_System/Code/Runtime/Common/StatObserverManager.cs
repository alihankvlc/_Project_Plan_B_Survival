using System.Collections.Generic;
using _Stat_System.Runtime.Base;
using UnityEngine;

namespace _Stat_System.Runtime.Common
{
    public interface IStatObserver
    {
        public void OnModifyStat(IStat stat);
    }
    public interface IStatSubject
    {
        public void RegisterObserver(IStatObserver observer);
        public void RemoveObserver(IStatObserver observer);
        public void Notify(IStat stat);
    }

    public class StatObserverManager : IStatSubject
    {
        private List<IStatObserver> _statObservers = new();

        public void RegisterObserver(IStatObserver observer)
        {
            if (_statObservers.Contains(observer)) return;
            _statObservers.Add(observer);
        }

        public void RemoveObserver(IStatObserver observer)
        {
            if (!_statObservers.Contains(observer)) return;
            _statObservers.Remove(observer);
        }

        public void Notify(IStat stat)
        {
            _statObservers.ForEach(r => r.OnModifyStat(stat));
        }
    }
}
