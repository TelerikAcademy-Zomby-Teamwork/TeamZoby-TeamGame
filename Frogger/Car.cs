using System;
using System.Linq;

namespace Frogger
{
    class Car : Figure
    {
        public Car(int x, int y, int lenght, ConsoleColor color, int speed, Direction direction)
            : base(x, y, new char[][] { Enumerable.Repeat(direction == Direction.LEFT ? '<' : '>', lenght).ToArray() }, color, speed, direction)
        {
        }
    }
}
