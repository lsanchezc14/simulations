using System;
using System.Collections.Generic;
using Ecs.Components;
using Ecs.Entities;

namespace Ecs.Main
{
    public class World
    {
        private readonly Dictionary<int, Entity> _entities = new Dictionary<int, Entity>();
        private readonly Dictionary<Type, Dictionary<int, IComponent>> _components = new Dictionary<Type, Dictionary<int, IComponent>>();

        public Entity CreateEntity()
        {
            var entity = new Entity();
            _entities[entity.Id] = entity;
            return entity;
        }

        public void DestroyEntity(Entity entity)
        {
            foreach (var componentList in _components.Values)
            {
                componentList.Remove(entity.Id);
            }
            _entities.Remove(entity.Id);
        }

        public void AddComponent(Entity entity, IComponent component)
        {
            var componentType = component.GetType();

            if(!_components.ContainsKey(componentType))
            {
                _components[componentType] = new Dictionary<int, IComponent>();
            }
            _components[componentType][entity.Id] = component;
        }

        public T GetComponent<T>(Entity entity) where T : IComponent
        {
            var componentType = typeof(T);
            if (_components.ContainsKey(componentType) && _components[componentType].ContainsKey(entity.Id))
            {
                return (T)_components[componentType][entity.Id];
            }
            
            return default;
        }

        public List<Entity> GetEntitiesWithComponents<T1, T2>() where T1 : IComponent where T2 : IComponent
        {
            var result = new List<Entity>();

            if (_components.ContainsKey(typeof(T1)) && _components.ContainsKey(typeof(T2)))
            {
                var components1 = _components[typeof(T1)];
                var components2 = _components[typeof(T2)];

                foreach (var entityId in components1.Keys)
                {
                    if (components2.ContainsKey(entityId))
                    {
                        result.Add(_entities[entityId]);
                    }
                }
            }
            
            return result;
        }
    }
}