using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecs.Main
{
    public class EventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            var eventType = typeof(T);
            if (!_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] = new List<Delegate>();
            }
            _subscribers[eventType].Add(handler);
        }

        public void Publish<T>(T eventData) where T : IEvent
        {
            var eventType = typeof(T);
            if (_subscribers.ContainsKey(eventType))
            {
                var handlers = _subscribers[eventType];

                try
                {
                    foreach (var handler in handlers)
                    {
                        ((Action<T>)handler).Invoke(eventData);
                    }               
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while publishing event {eventType.Name}: {ex.Message}");
                }
            }
        }
    }
}