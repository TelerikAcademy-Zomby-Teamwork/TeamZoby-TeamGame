﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using System.Threading;

class CurrentVersionFrog
{
    // ???? ?? ? ????????? ????? ?? ?????? ? ?????????????
    public struct Object
    {
        public int x;
        public int y;
        public string c;
        public ConsoleColor color;
        public Object(int x, int y, string c, ConsoleColor color)// constructor
        {
            this.x = x;
            this.y = y;
            this.c = c;
            this.color = color;
        }
    }// the objects

    public static void FillGrass(char[,] grass)
    {

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

    }//  fill the grass array 
    public static void DrawTheGrass(char[,] grass)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        for (int i = 0; i < grass.GetLength(0); i++)
        {
            for (int j = 0; j < grass.GetLength(1); j++)
            {
                Console.Write(grass[i, j]);
            }
        }

    } //draw grass

    public static void DrawSafetyZone(int scoreWindowBuffer)
    {
        Console.ResetColor();
        Console.SetCursorPosition(0, Console.WindowHeight - scoreWindowBuffer);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(new string('=', Console.WindowWidth));

        Console.SetCursorPosition(0, Console.WindowHeight / 2 - scoreWindowBuffer + 2);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(new string('=', Console.WindowWidth));
    }// draw safety zone 
    public static int[] Moves1(Object frog, int[] data, int scoreWindowBuffer)
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
                    data[0] = 0;
                }
                if (pressedKey.Key == ConsoleKey.Q)
                {
                    Console.WriteLine();
                    data[0] = 1;


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

        data[1] = frog.x;
        data[2] = frog.y;
        return data;
    }//move frog and chek for over 

    public static void PrintOnPosition(int x, int y, string c, ConsoleColor color)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(c);
    }

    public static void FillListOfCars(List<Object> cars, int scoreWindowBuffer)
    {
        int rowCounter = Console.WindowHeight - scoreWindowBuffer - 1;
        byte carCount = 0;

        string smallCars = ">>";
        string middleCars = ">>>";

        byte counter = 0;
        byte col = 0;

        for (int i = 0; i < 18; i++)
        {
            if (counter == 0)
            {
                cars.Add(new Object(col, rowCounter, smallCars, ConsoleColor.Red));
            }
            if (counter == 1)
            {
                cars.Add(new Object(col, rowCounter, middleCars, ConsoleColor.Yellow));
            }
            if (counter == 2)
            {
                cars.Add(new Object(col, rowCounter, smallCars, ConsoleColor.White));
            }
            col += 8;

            carCount++;
            if (carCount % 3 == 0)
            {
                rowCounter--;
                col = 0;
                switch (carCount / 3)
                {
                    case 0: col = 5;
                        break;
                    case 1: col = 5;

                        break;
                    case 2: col = 4;

                        break;
                    case 3: col = 0;
                        break;
                    case 4:
                        col = 6;
                        break;
                    case 5: col = 0;
                        break;
                    case 6: col = 7;
                        break;
                    default: col = 0;
                        break;
                }


                counter++;
                if (counter > 2)
                {
                    counter = 0;
                }

            }


        }
    }

    static void Main()
    {
        Console.BufferHeight = Console.WindowHeight = 18;
        Console.BufferWidth = Console.WindowWidth = 30;
        int n = Console.WindowWidth;

        char[,] grass = new char[2, n];// grass array 
        FillGrass(grass);// 


        int scoreWindowBuffer = 4;
        // ???? ??????? ? ????? ?? ?? ???????? ???????? ?? ??????

        Object frog = new Object();
        frog.x = Console.WindowWidth / 2;
        frog.y = Console.WindowHeight - scoreWindowBuffer;
        frog.c = "X";
        frog.color = ConsoleColor.Yellow;
        int[] data = new int[3] { 0, frog.x, frog.y };
        List<Object> cars = new List<Object>(18);
        FillListOfCars(cars, scoreWindowBuffer);
        int x;
        int y;
        string c;
        ConsoleColor color;










        while (true)
        {

            Moves1(frog, data, scoreWindowBuffer);
            if (data[0] == 1)//chek for exit 0=exit
            {
                return;

            }

            // Move our frog
            // Move obstacles
            // Collision detection
            Console.Clear();

            DrawTheGrass(grass);// print grass
            DrawSafetyZone(scoreWindowBuffer);// print save zone 

            PrintOnPosition(frog.x = data[1], frog.y = data[2], frog.c, frog.color);// print the  frog
            for (int i = 0; i < cars.Count; i++)
            {
                PrintOnPosition(cars[i].x, cars[i].y, cars[i].c, cars[i].color);
                x = cars[i].x;
                x++;
                y = cars[i].y;
                c = cars[i].c;
                color = cars[i].color;


                cars.Remove(cars[i]);
                cars.Insert(i, new Object(x, y, c, color));

            }

            // check if the car is in the end of the console window and print it in the begging of the console. remove car and add other with new cordinates//
            for (int i = 0; i < cars.Count; i++)
            {
                if (cars[i].x == Console.WindowWidth - 1)
                {
                    x = 0;
                    y = cars[i].y;
                    c = cars[i].c;
                    color = cars[i].color;
                    cars.Remove(cars[i]);
                    cars.Insert(i, new Object(x, y, c, color));

                }


            }
            // colusion detection //////////////////////////////////
            foreach (var car in cars)
            {
                if (car.x == frog.x && car.y == frog.y)
                {

                    Console.Clear();
                    Console.WriteLine("you are dead ");
                    Thread.Sleep(1000);
                    frog.x = Console.WindowWidth / 2;
                    frog.y = Console.WindowHeight - scoreWindowBuffer;
                }
            }

            Thread.Sleep(400);
        }
    }
}
