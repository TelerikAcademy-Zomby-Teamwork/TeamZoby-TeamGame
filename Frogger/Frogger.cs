using System;

namespace Frogger
{
    class Frogger
    {
        static void Main(string[] args)
        {
            Engine engine = new Engine(new ConsoleRenderer());
            engine.Start();

            Console.SetCursorPosition(0, 17);
        }
    }
}
