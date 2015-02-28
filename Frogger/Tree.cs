using System;
using System.Linq;

namespace Frogger
{
    class Tree : Figure
    {
        public Tree(int x, int y, int lenght, ConsoleColor color, int speed, Direction direction)
            : base(x, y, new char[][] { Enumerable.Repeat('0', lenght).ToArray() }, color, speed, direction)
        {
        }

        public override bool IsObsticle()
        {
            return false;
        }
    }
}
