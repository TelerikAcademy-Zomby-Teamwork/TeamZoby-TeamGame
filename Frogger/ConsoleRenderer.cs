using System;
using System.Text;

namespace Frogger
{
    class ConsoleRenderer : IRenderer
    {
        private int width;
        private int height;

        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.Initialize(value, this.Height);
                width = value;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.Initialize(this.Width, value);
                height = value;
            }
        }

        public ConsoleRenderer(int width = 30, int height = 18)
        {
            this.Initialize(width, height);
            Console.OutputEncoding = Encoding.Unicode;
        }

        public void DrawPixel(int x, int y, char symbol, ConsoleColor color)
        {
            if (x < 0 || x >= this.Width || y < 0 || y >= this.Height)
            {
                return;
            }

            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(symbol);
        }

        public void DrawObject(int x, int y, char[][] body, ConsoleColor color)
        {
            int startX = x < 0 ? -x : 0;
            int endX = (x + body[0].Length >= this.Width) ? this.Width - x : body[0].Length;
            int startY = y < 0 ? -y : 0;
            int endY = (y + body.Length >= this.Height) ? this.Height - y : body.Length;
            Console.ForegroundColor = color;
            for (int i = startY; i < endY; i++)
            {
                if (startX < endX)
                {
                    Console.SetCursorPosition(x + startX, y + startY + i);
                    Console.Write(new String(body[i]).Substring(startX, endX - startX));
                }
            }
        }

        public void ShowMessage(int x, int y, string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(x, y);
            Console.Write(message);
        }
        public void Clear()
        {
            Console.Clear();
        }

        private void Initialize(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentOutOfRangeException("The dimensions must be positive integers.");
            }
            this.width = Console.BufferWidth = Console.WindowWidth = width;
            this.height = Console.BufferHeight = Console.WindowHeight = height;
        }
    }
}
