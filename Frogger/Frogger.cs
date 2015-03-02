using System;

namespace Frogger
{
    class Frogger
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu(new ConsoleRenderer(), "highscore.txt");
            menu.Open();
            Console.SetCursorPosition(0, 17);
        }
    }
}
