using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusDAL
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();

        public static void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
                _subscribers[type] = new List<Delegate>();
            _subscribers[type].Add(handler);
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (_subscribers.ContainsKey(type))
                _subscribers[type].Remove(handler);
        }

        public static void Publish<T>(T evt)
        {
            var type = typeof(T);
            if (_subscribers.ContainsKey(type))
            {
                foreach (var handler in _subscribers[type])
                    ((Action<T>)handler)?.Invoke(evt);
            }
        }
        public static void Clear()
        {
            _subscribers.Clear();
        }
    }

}
