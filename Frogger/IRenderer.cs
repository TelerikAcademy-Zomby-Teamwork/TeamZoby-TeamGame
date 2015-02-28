using System;

namespace Frogger
{
    public interface IRenderer
    {
        int Width
        {
            get;
            set;
        }
        int Height
        {
            get;
            set;
        }

        void DrawPixel(int x, int y, char symbol, ConsoleColor color);
        void DrawObject(int x, int y, char[][] body, ConsoleColor color);
        void ShowMessage(string message);
        void ShowMessage(int x, int y, string message);
        void Clear();
    }
}
