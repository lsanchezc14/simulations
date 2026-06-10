using System;
using Ecs.Components;
using Ecs.Main;
using Microsoft.Xna.Framework;

namespace Ecs.Systems
{
    public class MovementSystem : ISystem
    {
        private readonly World _world;

        public MovementSystem(World world)
        {
            _world = world;
        }
        public void Update(GameTime gameTime)
        {
            var movingEntities = _world.GetEntitiesWithComponents<PositionComponent, VelocityComponent>();

            foreach (var entity in movingEntities)
            {
                var position = _world.GetComponent<PositionComponent>(entity);
                var velocity = _world.GetComponent<VelocityComponent>(entity);

                position.X += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                position.Y += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}