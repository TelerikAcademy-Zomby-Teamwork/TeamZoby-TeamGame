using System;
using System.Collections.Generic;
using System.Linq;

namespace Frogger
{
    public class CollisionDispater
    {
        private static Frog frog;

        public bool DetectCollisions(Frog frog, List<Figure> figures, List<Terrain> terrains)
        {
            CollisionDispater.frog = frog;
            Func<DrawableObject, bool> containsFrog = CollisionDispater.containsFrog;
            Terrain currentTerrain = (Terrain)terrains.Single(containsFrog);
            if (!currentTerrain.Passable())
            {
                if (!figures.Any(containsFrog))
                {
                    return true;
                }
            }
            // This check works only for figures consisted on one row
            bool hasCollision = figures.Any(figure => figure.IsObsticle() && frog.Y == figure.Y && frog.X + frog.Body[0].Length > figure.X && frog.X < figure.X + figure.Body[0].Length);
            if (hasCollision)
            {
                return true;
            }

            return false;
        }

        private static bool containsFrog(DrawableObject drawableObject)
        {
            return drawableObject.X <= frog.X &&
                frog.X + frog.Body[0].Length <= drawableObject.X + drawableObject.Body[0].Length &&
                drawableObject.Y <= frog.Y &&
                frog.Y + frog.Body.Length <= drawableObject.Y + drawableObject.Body.Length;
        }
    }
}
