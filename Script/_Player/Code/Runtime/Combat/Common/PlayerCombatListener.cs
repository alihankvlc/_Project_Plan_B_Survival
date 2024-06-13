using System;
using System.Collections;
using System.Collections.Generic;
using _Item_System_.Runtime.Base;
using _Player_System_.Runtime.Combat.Base;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Player_System_.Runtime.Combat.Common
{
    [Serializable]
    public class MeleeEvent : UnityEvent{ }
    public interface IMeleeTriggerListener
    {
        public MeleeEvent OnMeleeEnableTrigger { get; }
        public MeleeEvent OnMeleeIgnoreTrigger { get; }
    }

    public class PlayerCombatListener : MonoBehaviour, IMeleeTriggerListener
    {
        [SerializeField] private MeleeEvent _onMeleeEnableTrigger = new();
        [SerializeField] private MeleeEvent _onMeleeIgnoreTrigger = new();

        public MeleeEvent OnMeleeEnableTrigger => _onMeleeEnableTrigger;
        public MeleeEvent OnMeleeIgnoreTrigger => _onMeleeIgnoreTrigger;

        public void EnableMeleeTrigger() => _onMeleeEnableTrigger?.Invoke();
        public void DisableMeleeTrigger() => _onMeleeIgnoreTrigger?.Invoke();
    }
}