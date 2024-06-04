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

    public class StatManager : IStatSubject
    {
        private List<IStatObserver> _observers = new List<IStatObserver>();

        public void RegisterObserver(IStatObserver observer)
        {
            if (_observers.Contains(observer)) return;
            _observers.Add(observer);
        }

        public void RemoveObserver(IStatObserver observer)
        {
            if (!_observers.Contains(observer)) return;
            _observers.Remove(observer);
        }

        public void Notify(IStat stat)
        {
            _observers.ForEach(r => r.OnModifyStat(stat));
        }
    }
}
