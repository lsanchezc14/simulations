using Microsoft.Xna.Framework;

namespace Ecs.Systems
{
    public interface ISystem
    {
        void Update(GameTime gameTime);
    }
}