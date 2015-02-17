using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using System.Threading;

class Program
{
    // ???? ?? ? ????????? ????? ?? ?????? ? ?????????????
    private struct Object
    {
        public int x;
        public int y;
        public string c;
        public ConsoleColor color;
    }

    public static void PrintOnPosition(int x, int y, string c, ConsoleColor color)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(c);
    }

    static void Main()
    {
        Console.BufferHeight = Console.WindowHeight = 18;
        Console.BufferWidth = Console.WindowWidth = 30;
        int n = Console.WindowWidth;
        char [,] grass = new char [2, n];
        
        for (int i = 0; i < grass.GetLength(0); i++)
        {
            for (int j = 0; j < grass.GetLength(1); j++)
            {
                grass[i, j] = '#';
                grass[1, 0] = ' ';
                grass[1, 7] = ' ';
                grass[1, 15] = ' ';
                grass[1, 22] = ' ';
                grass[1, 29] = ' ';
            }
        }
        
        int scoreWindowBuffer = 4;
        // ???? ??????? ? ????? ?? ?? ???????? ???????? ?? ??????

        Object frog = new Object();
        frog.x = Console.WindowWidth / 2;
        frog.y = Console.WindowHeight - scoreWindowBuffer;
        frog.c = "X";
        frog.color = ConsoleColor.Yellow;
        List<Object> cars = new List<Object>();

        Object firstLineCar = new Object();
        firstLineCar.x = 0;
        firstLineCar.y = Console.WindowHeight - scoreWindowBuffer - 1;
        firstLineCar.c = ">>>>";
        firstLineCar.color = ConsoleColor.Cyan;

        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
                if (pressedKey.Key == ConsoleKey.Escape)
                {
                    PrintOnPosition(10, 7, @"
    Paused
    Any key ---> Continue.
    Q ---> Quit.", ConsoleColor.Red);
                    pressedKey = Console.ReadKey();
                    if (pressedKey.Key == ConsoleKey.C)
                    {
                        continue;
                    }
                    if (pressedKey.Key == ConsoleKey.Q)
                    {
                        Console.WriteLine();
                        break;
                    }
                }
                if (pressedKey.Key == ConsoleKey.LeftArrow)
                {
                    if (frog.x >= 1)
                    {
                        frog.x--;
                    }
                }
                else if (pressedKey.Key == ConsoleKey.RightArrow)
                {
                    if (frog.x < Console.WindowWidth - 1)
                    {
                        frog.x++;
                    }
                }
                else if (pressedKey.Key == ConsoleKey.DownArrow)
                {
                    if (frog.y < Console.WindowHeight - scoreWindowBuffer)
                    {
                        frog.y++;
                    }
                }
                else if (pressedKey.Key == ConsoleKey.UpArrow)
                {
                    if (frog.y >= 1)
                    {
                        frog.y--;
                    }
                }
            }

            // Move our frog
            // Move obstacles
            // Collision detection
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < grass.GetLength(0); i++)
            {
                for (int j = 0; j < grass.GetLength(1); j++)
                {
                    Console.Write(grass[i, j]);
                }
            }
            Console.ResetColor();
            Console.SetCursorPosition(0, Console.WindowHeight - scoreWindowBuffer);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(new string('=', Console.WindowWidth));

            Console.SetCursorPosition(0, Console.WindowHeight / 2 - scoreWindowBuffer + 2);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(new string('=', Console.WindowWidth));
            // Redraw playfield 
            PrintOnPosition(frog.x, frog.y, frog.c, frog.color);
            PrintOnPosition(firstLineCar.x, firstLineCar.y, firstLineCar.c, firstLineCar.color);
            firstLineCar.x++;

            if (firstLineCar.x - 1 == Console.WindowWidth - firstLineCar.c.Length)
            {
                firstLineCar.x = 0;
                firstLineCar.y = Console.WindowHeight - scoreWindowBuffer - 1;
            }

            // Draw info 
            Thread.Sleep(100);
        }
    }
}

