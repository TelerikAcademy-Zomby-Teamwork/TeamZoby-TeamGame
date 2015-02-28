using System;

namespace Frogger
{
    public class ConsolePixel
    {
        public ConsoleColor Color;
        public char Symbol;

        public ConsolePixel(ConsoleColor color = 0, char symbol = ' ')
        {
            this.Color = color;
            this.Symbol = symbol;
        }
    }
}
