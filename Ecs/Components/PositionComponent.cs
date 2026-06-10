namespace Ecs.Components
{
    public class PositionComponent : IComponent
    {
        public float X { get; set; }
        public float Y { get; set; }

        public PositionComponent(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}