using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
        private const int REFRESH_TIME = 500;

        private const string BACKGROUND_MUSIC_FILE_PATH = "music/Background.wav";
        private const string WIN_MUSIC_FILE_PATH = "music/Win.wav";
        private const string LOSE_MUSIC_FILE_PATH = "music/Lose.wav";

        private bool TryIsOver;
        private bool FrogPositioned;
        private IRenderer Renderer;
        private Player Player;
        private Frog Frog;
        private List<Figure> Figures;
        private List<Terrain> Terrains;
        private PressedKeysProvider PressedKeysProvider;
        private CollisionDispater CollisionDispater;
        private Highscore Highscore;

        public Engine(IRenderer renderer, Highscore highscore, Player player)
        {
            this.TryIsOver = false;
            this.FrogPositioned = false;
            this.Renderer = renderer;
            this.Player = player;
            this.PressedKeysProvider = new PressedKeysProvider();
            this.CollisionDispater = new CollisionDispater();
            this.InitializeFrog();
            this.InitializeFigures();
            this.InitializeTerrains();
            this.Highscore = highscore;
        }

        public void Start()
        {
            this.PlaySound(BACKGROUND_MUSIC_FILE_PATH);
            while (true)
            {
                this.TryIsOver = false;
                Renderer.Clear();
                this.Update();
                this.Draw();
                // not already determined while moving the frog by the player
                if (!this.TryIsOver)
                {
                    this.CheckTryOver();
                }
                if (this.TryIsOver)
                {
                    this.PressedKeysProvider.ClearInput();
                    if (this.FrogPositioned)
                    {
                        this.FrogPositioned = false;
                        this.Player.Score += 100;
                        this.Figures.Add(this.Frog);
                        if (this.HasPlayerWon())
                        {
                            this.GameOver(true);
                            return;
                        }
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

                this.CheckTryOver();
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

        private void CheckTryOver()
        {
            WinArea reachedWinArea = this.Figures.OfType<WinArea>().SingleOrDefault(winArea => winArea.X == this.Frog.X && winArea.Y == this.Frog.Y);
            if (reachedWinArea != null)
            {
                this.TryIsOver = true;
                this.FrogPositioned = true;
                this.Figures.Remove(reachedWinArea);
                return;
            }

            bool frogDied = this.IsFrogOutOfRange() || this.CollisionDispater.DetectCollisions(this.Frog, this.Figures, this.Terrains);
            if (frogDied)
            {
                this.TryIsOver = true;
                this.FrogPositioned = false;
            }
        }

        private bool HasPlayerWon()
        {
            return this.Figures.OfType<WinArea>().Count() == 0;
        }

        private bool IsFrogOutOfRange()
        {
            return this.Frog.X < 0 || this.Frog.Y < 0 || this.Frog.X >= this.Renderer.Width || this.Frog.Y >= PLAYGROUND_HEIGHT;
        }

        private void GameOver(bool playerWins)
        {
            this.Renderer.Clear();
            if (playerWins)
            {
                this.Player.Score += this.Player.LivesCount * 100;
                this.ShowCenteredMessage("You Win!", this.Renderer.Height / 2 - 1);
                this.PlaySound(WIN_MUSIC_FILE_PATH);
            }
            else
            {
                this.ShowCenteredMessage("Game Over!", this.Renderer.Height / 2 - 1);
                this.PlaySound(LOSE_MUSIC_FILE_PATH);
            }
            this.ShowCenteredMessage(String.Format("Final score: {0}", this.Player.Score), this.Renderer.Height / 2 + 1);
            this.Highscore.AddHighscoreEntry(this.Player);
            this.Highscore.Persist();
        }

        private void ShowCenteredMessage(string message, int row)
        {
            this.Renderer.ShowMessage(Math.Max(this.Renderer.Width / 2 - message.Length / 2, 0), row, message);
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

        private void PlaySound(string filePath)
        {
            SoundPlayer player = new SoundPlayer(filePath);
            player.Play();
        }
    }
}