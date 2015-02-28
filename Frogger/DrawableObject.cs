using System;

namespace Frogger
{
    public abstract class DrawableObject : IDrawable
    {
        public int X
        {
            get;
            protected set;
        }
        public int Y
        {
            get;
            protected set;
        }
        public char[][] Body
        {
            get;
            private set;
        }
        public ConsoleColor Color
        {
            get;
            private set;
        }

        public DrawableObject(int x, int y, char[][] body, ConsoleColor color)
        {
            this.X = x;
            this.Y = y;
            this.Body = body;
            this.Color = color;
        }

        public void PlaceAt(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public void Draw(IRenderer renderer)
        {
            renderer.DrawObject(this.X, this.Y, this.Body, this.Color);
        }
    }
}
