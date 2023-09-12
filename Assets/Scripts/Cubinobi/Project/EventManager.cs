using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cubinobi.Project
{
    public class EventManager
    {
        public delegate void EventDelegate(IEvent e);

        private readonly Dictionary<Type, EventDelegate> _delegates = new();


        public void AddListener<T>(EventDelegate e) where T : IEvent
        {
            if (!_delegates.ContainsKey(typeof(T)))
            {
                _delegates[typeof(T)] = e;
            }
            else
            {
                _delegates[typeof(T)] += e;
            }
        }

        public void RemoveListener<T>(EventDelegate e) where T : IEvent
        {
            if (_delegates[typeof(T)].GetInvocationList().Length > 1)
            {
                _delegates[typeof(T)] -= e;
            }
            else
            {
                _delegates.Remove(typeof(T));
            }
        }

        public void SendEvent(IEvent e)
        {
            var type = e.GetType();
            if (_delegates.ContainsKey(type))
            {
                _delegates[type].Invoke(e);
            }
            else
            {
                Debug.LogWarning($"Sending event but no listeners of type {type} found.");
            }
        }
    }
}