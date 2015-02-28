using System;

namespace Frogger
{
    public class PassableLandTerrain : Terrain
    {
        public PassableLandTerrain(int x, int y, int width, int height, ConsoleColor color)
            : base(x, y, width, height, '=', color)
        {
        }
    }
}
