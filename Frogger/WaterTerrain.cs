using System;

namespace Frogger
{
    public class WaterTerrain : Terrain
    {
        public WaterTerrain(int x, int y, int width, int height)
            : base(x, y, width, height, '^', ConsoleColor.Cyan)
        {
        }

        public override bool Passable()
        {
            return false;
        }
    }
}
