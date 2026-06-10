using System.Drawing;

namespace Ecs.Components
{
    public class SpriteComponent : IComponent
    {
        public string TextureName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Color Color { get; set; }

        public SpriteComponent(string textureName, int width, int height, Color color)
        {
            TextureName = textureName;
            Width = width;
            Height = height;
            Color = color;
        }
    }
}