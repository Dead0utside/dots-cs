using System;

namespace Dots.Core
{
    [Serializable]
    public class Dot
    {
        public Color Color { get; }

        public Dot(Color color)
        {
            Color = color;
        }
    }
}