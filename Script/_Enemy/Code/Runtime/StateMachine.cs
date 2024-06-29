using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using UnityEngine;

namespace _Zombie_System_.Runtime.Common
{
    public interface IState
    {
        public void OnStateEnter();
        public void OnStateTick();
        public void OnStateExit();

        public StateMachine StateMachine { get; }
    }

    public class StateMachine
    {
        private readonly Dictionary<Type, IState> _states = new Dictionary<Type, IState>();
        private IState _currentState;

        public IState ActiveState => _currentState;

        public void AddState<T>(T state) where T : IState
        {
            Type stateType = typeof(T);
            if (_states.ContainsKey(stateType))
            {
                Debug.LogError($"State {stateType.Name} already exists");
                return;
            }

            _states[stateType] = state;
        }

        public void SetState<T>() where T : IState
        {
            Type stateType = typeof(T);
            if (!_states.TryGetValue(stateType, out var targetState))
            {
                Debug.LogError($"State {stateType.Name} not found");
                return;
            }

            _currentState?.OnStateExit();
            _currentState = targetState;
            _currentState.OnStateEnter();
        }

        public void Tick()
        {
            _currentState?.OnStateTick();
        }

        public void Dispose()
        {
            _currentState?.OnStateExit();
        }
    }
}