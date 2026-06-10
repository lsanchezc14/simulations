using System;
using System.Collections.Generic;
using Ecs.Components;

namespace Ecs.Entities
{
    public class Entity
    {
        private static int _nextId = 0;
        public int Id { get; }
        private readonly Dictionary<Type, IComponent> _components = new Dictionary<Type, IComponent>();

        public Entity()
        {
            Id = _nextId++;
        }

        public void AddComponent(IComponent component)
        {
            _components[component.GetType()] = component;
        }

        public T GetComponent<T>() where T: IComponent
        {
            _components.TryGetValue(typeof(T), out var component);
            return (T)component;
        }

        public bool HasComponent<T>() where T: IComponent
        {
            return _components.ContainsKey(typeof(T));
        }

        public void RemoveComponent<T>() where T: IComponent
        {
            _components.Remove(typeof(T));
        }
    }
}