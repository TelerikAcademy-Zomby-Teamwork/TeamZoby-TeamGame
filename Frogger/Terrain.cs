using System;
using System.Linq;

namespace Frogger
{
    public abstract class Terrain : DrawableObject
    {
        public Terrain(int x, int y, int width, int height, char symbol, ConsoleColor color) :
            base(x, y, Enumerable.Repeat(Enumerable.Repeat(symbol, width).ToArray(), height).ToArray(), color)
        {
        }

        public virtual bool Passable()
        {
            return true;
        }
    }
}
