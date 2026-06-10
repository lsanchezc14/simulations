namespace Ecs.Components
{
    public class VelocityComponent : IComponent
    {
        public float X { get; set; }
        public float Y { get; set; }

        public VelocityComponent(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}