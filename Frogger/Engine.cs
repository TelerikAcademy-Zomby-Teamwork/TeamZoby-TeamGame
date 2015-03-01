using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Frogger
{
    class Engine
    {
        private const int TOTAL_HEIGHT = 18;
        private const int TOTAL_WIDTH = 30;
        private const int ROWS_WITH_CARS_COUNT = 6;
        private const int ROWS_WITH_WATER_COUNT = 5;
        private const int PLAYER_INFO_FIELD_HEIGHT = 3;
        private const int PLAYGROUND_HEIGHT = ROWS_WITH_CARS_COUNT + ROWS_WITH_WATER_COUNT + 4;
        private const int CARS_PER_ROW_COUNT = 3;
        private const int TREES_PER_ROW_COUNT = 4;
        private const int REFRESH_TIME = 1000;

        private bool TryIsOver;
        private bool FrogPositioned;
        private IRenderer Renderer;
        private Player Player;
        private Frog Frog;
        private List<Figure> Figures;
        private List<Terrain> Terrains;
        private PressedKeysProvider PressedKeysProvider;
        private CollisionDispater CollisionDispater;

        public Engine(IRenderer renderer)
        {
            this.TryIsOver = false;
            this.FrogPositioned = false;
            this.Renderer = renderer;
            this.Player = new Player("Tea");
            this.PressedKeysProvider = new PressedKeysProvider();
            this.CollisionDispater = new CollisionDispater();
            this.InitializeFrog();
            this.InitializeFigures();
            this.InitializeTerrains();
        }

        public void Start()
        {
            while (true)
            {
                this.TryIsOver = false;
                Renderer.Clear();
                this.Update();
                this.Draw();
                this.CheckGameOver();
                if (this.TryIsOver)
                {
                    this.PressedKeysProvider.ClearInput();
                    if (this.FrogPositioned)
                    {
                        this.FrogPositioned = false;
                        this.Figures.Add(this.Frog);
                        this.InitializeFrog();
                    }
                    else
                    {
                        this.Player.LivesCount--;
                        if (this.Player.LivesCount == 0)
                        {
                            this.GameOver(false);
                            return;
                        }
                        this.InitializeFrog();
                    }
                }
                else
                {
                    Thread.Sleep(REFRESH_TIME);
                }
            }
        }

        public void Draw()
        {
            foreach (var terrain in this.Terrains)
            {
                terrain.Draw(this.Renderer);
            }
            foreach (var figure in this.Figures)
            {
                figure.Draw(this.Renderer);
            }
            this.Frog.Draw(this.Renderer);
            this.ShowPlayerInfo();
        }

        public void Update()
        {
            this.UpdateFrog();
            for (int i = 0; i < Figures.Count; i++)
            {

                Figures[i].Update();
                if (Figures[i].Direction == Direction.RIGHT && Figures[i].X > Renderer.Width + 10)
                {
                    Figures[i].PlaceAt(0 - Figures[i].Body.Length, Figures[i].Y);
                }

                if (Figures[i].Direction == Direction.LEFT && Figures[i].X < -10)
                {
                    Figures[i].PlaceAt(Renderer.Width + Figures[i].Body.Length, Figures[i].Y);
                }

            }
        }

        public void UpdateFrog()
        {
            foreach (var pressedKey in this.PressedKeysProvider.getPressedKeys())
            {
                switch (pressedKey.Key)
                {
                    case ConsoleKey.LeftArrow:
                        {
                            if (this.Frog.X > 0)
                            {
                                this.Frog.PlaceAt(this.Frog.X - 1, this.Frog.Y);
                            }
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            if (this.Frog.X + this.Frog.Body.Length < this.Renderer.Width)
                            {
                                this.Frog.PlaceAt(this.Frog.X + 1, this.Frog.Y);
                            }
                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            if (this.Frog.Y > 0)
                            {
                                this.Frog.PlaceAt(this.Frog.X, this.Frog.Y - 1);
                            }
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            if (this.Frog.Y < PLAYGROUND_HEIGHT - 1)
                            {
                                this.Frog.PlaceAt(this.Frog.X, this.Frog.Y + 1);
                            }
                            break;
                        }

                }

                this.CheckGameOver();
                if (this.TryIsOver)
                {
                    return;
                }
            }

            Tree treeWithFrog = this.Figures.OfType<Tree>().SingleOrDefault(tree =>
                    tree.Y == this.Frog.Y &&
                    tree.X <= this.Frog.X &&
                    tree.X + tree.Body[0].Length >= this.Frog.X + this.Frog.Body[0].Length);
            if (treeWithFrog != null)
            {
                switch (treeWithFrog.Direction)
                {
                    case Direction.LEFT:
                        {
                            this.Frog.PlaceAt(this.Frog.X - treeWithFrog.Speed, this.Frog.Y);
                            break;
                        }
                    case Direction.RIGHT:
                        {
                            this.Frog.PlaceAt(this.Frog.X + treeWithFrog.Speed, this.Frog.Y);
                            break;
                        }
                    default:
                        {
                            throw new NotSupportedException("Tree direction not supported.");
                        }
                }
            }
        }

        private void CheckGameOver()
        {
            bool reachedWinArea = this.Figures.OfType<WinArea>().Any(winArea => winArea.X == this.Frog.X && winArea.Y == this.Frog.Y);
            if (reachedWinArea)
            {
                this.TryIsOver = true;
                this.FrogPositioned = true;
                return;
            }

            bool frogDied = this.IsFrogOutOfRange() || this.CollisionDispater.DetectCollisions(this.Frog, this.Figures, this.Terrains);
            if (frogDied)
            {
                this.TryIsOver = true;
                this.FrogPositioned = false;
            }
        }

        private bool IsFrogOutOfRange()
        {
            return this.Frog.X < 0 || this.Frog.Y < 0 || this.Frog.X >= this.Renderer.Width || this.Frog.Y >= PLAYGROUND_HEIGHT;
        }

        private void GameOver(bool playerWins)
        {
            if (playerWins)
            {
                this.Renderer.ShowMessage("You Win!");
            }
            else
            {
                this.Renderer.ShowMessage("Game Over!");
            }
        }

        private void InitializeFrog()
        {
            this.Frog = new Frog(this.Renderer.Width / 2, PLAYGROUND_HEIGHT - 1);
        }

        private void InitializeFigures()
        {
            this.Figures = new List<Figure>();

            for (int i = 0; i < ROWS_WITH_WATER_COUNT; i++)
            {
                for (int j = 0; j < TREES_PER_ROW_COUNT; j++)
                {
                    Direction direction = i % 2 == 0 ? Direction.LEFT : Direction.RIGHT;
                    int length = i % 3 + 4;
                    ConsoleColor color = (i % 2 == 0) ? ConsoleColor.White : ConsoleColor.Gray;
                    Figures.Add(new Tree((this.Renderer.Width * j) / TREES_PER_ROW_COUNT + length, 2 + i, length, color, i % 3 + 1, direction));
                }
            }

            for (int i = 0; i < ROWS_WITH_CARS_COUNT; i++)
            {
                for (int j = 0; j < CARS_PER_ROW_COUNT; j++)
                {
                    Direction direction = i % 2 == 0 ? Direction.LEFT : Direction.RIGHT;
                    int length = i % 3 + 2;
                    ConsoleColor color = (i % 2 == 0) ? ConsoleColor.Red : ConsoleColor.DarkRed;
                    Figures.Add(new Car((this.Renderer.Width * j) / CARS_PER_ROW_COUNT + length, PLAYGROUND_HEIGHT - 2 - i, length, color, i % 3 + 1, direction));
                }
            }

            for (int i = 4; i < 30; i += 7)
            {
                this.Figures.Add(new WinArea(i, 1));
            }
        }

        private void InitializeTerrains()
        {
            this.Terrains = new List<Terrain>();
            this.Terrains.Add(new UnpassableLandTerrain(0, 0, this.Renderer.Width, 2, ConsoleColor.DarkYellow));
            this.Terrains.Add(new WaterTerrain(0, 2, this.Renderer.Width, ROWS_WITH_WATER_COUNT));
            this.Terrains.Add(new PassableLandTerrain(0, PLAYGROUND_HEIGHT - 1, this.Renderer.Width, 1, ConsoleColor.Yellow));
            this.Terrains.Add(new PassableLandTerrain(0, PLAYGROUND_HEIGHT - ROWS_WITH_CARS_COUNT - 2, this.Renderer.Width, 1, ConsoleColor.Yellow));
            this.Terrains.Add(new StreetTerrain(0, ROWS_WITH_WATER_COUNT + 3, this.Renderer.Width, ROWS_WITH_CARS_COUNT));
        }

        private void ShowPlayerInfo()
        {
            this.Renderer.ShowMessage(0, TOTAL_HEIGHT - PLAYER_INFO_FIELD_HEIGHT + 0, String.Format("Player: {0}", this.Player.Name));
            this.Renderer.ShowMessage(0, TOTAL_HEIGHT - PLAYER_INFO_FIELD_HEIGHT + 1, String.Format("Score: {0}", this.Player.Score));
            this.Renderer.ShowMessage(0, TOTAL_HEIGHT - PLAYER_INFO_FIELD_HEIGHT + 2, String.Format("Lives: {0}", this.Player.LivesCount));
        }
    }
}