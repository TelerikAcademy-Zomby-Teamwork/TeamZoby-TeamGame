using System;

namespace Frogger
{
    public class UnpassableLandTerrain : PassableLandTerrain
    {
        public UnpassableLandTerrain(int x, int y, int width, int height, ConsoleColor color)
            : base(x, y, width, height, color)
        {
        }

        public override bool Passable()
        {
            return false;
        }
    }
}
