using System;

namespace Frogger
{
    public abstract class Figure : DrawableObject
    {
        public int Speed
        {
            get;
            private set;
        }
        public Direction Direction
        {
            get;
            private set;
        }

        public Figure(int x, int y, char[][] body, ConsoleColor color, int speed, Direction direction)
            : base(x, y, body, color)
        {
            this.Speed = speed;
            this.Direction = direction;
        }

        public void Update()
        {
            switch (this.Direction)
            {
                case Direction.RIGHT:
                    {
                        this.X += this.Speed;
                        break;
                    }
                case Direction.LEFT:
                    {
                        this.X -= this.Speed;
                        break;
                    }
                case Direction.TOP:
                    {
                        this.Y -= this.Speed;
                        break;
                    }
                case Direction.BOTTOM:
                    {
                        this.Y += this.Speed;
                        break;
                    }
            }
        }

        virtual public bool IsObsticle()
        {
            return true;
        }
    }
}
