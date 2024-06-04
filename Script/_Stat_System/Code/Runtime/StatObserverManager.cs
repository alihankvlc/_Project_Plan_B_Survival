using _Stat_System.Runtime.Common;
using _Stat_System.Runtime.Base;
using System;
using Zenject;
using UnityEngine;


public class StatObserverManager : MonoBehaviour, IStatObserver
{
    public static event Action<IStat> OnStatChanged;

    private StatManager _subject;

    [Inject]
    private void Constructor(StatManager subject)
    {
        _subject = subject;
        _subject.RegisterObserver(this);

    }

    public void OnModifyStat(IStat stat)
    {
        OnStatChanged?.Invoke(stat);
    }
}
