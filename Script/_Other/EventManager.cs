using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project_Plan_B_Survival.Script._Other
{
    public interface EventContainer
    {
    }

    public static class EventManager
    {
        private static readonly Dictionary<Type, Action<EventContainer>> eventTypeToListener = new();
        private static readonly Dictionary<Delegate, Action<EventContainer>> listenerToAction = new();

        public static void AddListener<T>(Action<T> listener) where T : EventContainer
        {
            if (!listenerToAction.ContainsKey(listener))
            {
                Action<EventContainer> action = (e) => listener((T)e);
                listenerToAction[listener] = action;

                if (eventTypeToListener.TryGetValue(typeof(T), out Action<EventContainer> existingAction))
                {
                    eventTypeToListener[typeof(T)] += action;
                }
                else
                {
                    eventTypeToListener[typeof(T)] = action;
                }
            }
        }

        public static void RemoveListener<T>(Action<T> listener) where T : EventContainer
        {
            if (listenerToAction.TryGetValue(listener, out var action))
            {
                if (eventTypeToListener.TryGetValue(typeof(T), out var existingAction))
                {
                    existingAction -= action;
                    if (existingAction == null)
                    {
                        eventTypeToListener.Remove(typeof(T));
                    }
                    else
                    {
                        eventTypeToListener[typeof(T)] = existingAction;
                    }
                }

                listenerToAction.Remove(listener);
            }
        }

        public static void Trigger(EventContainer eventContainer)
        {
            if (eventTypeToListener.TryGetValue(eventContainer.GetType(), out var action))
            {
                action.Invoke(eventContainer);
            }
        }

        public static void Clear()
        {
            eventTypeToListener.Clear();
            listenerToAction.Clear();
        }
    }
}