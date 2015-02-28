using System;

namespace Frogger
{
    public class Frog : Figure
    {
        public Frog(int x, int y)
            : base(x, y, new char[][] { new char[] { 'Ж' } }, ConsoleColor.Green, 0, 0)
        {
        }
    }
}
