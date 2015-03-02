using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frogger
{
    public class Menu
    {
        private static int EXIT_CHOICE_NUMBER = 3;

        private IRenderer Renderer;
        private Player Player;
        private Highscore Highscore;

        public Menu(IRenderer renderer, string fileName)
        {
            this.Renderer = renderer;
            this.Player = new Player(this.RetrievePlayerName());
            this.Highscore = new Highscore(fileName);
        }

        public void Open()
        {
            this.ReadInput();
        }

        private string RetrievePlayerName()
        {
            this.Renderer.ShowMessage(0, 0, "Enter your name: ");
            return Console.ReadLine();
        }

        private void ReadInput()
        {
            int choice = 0;
            while (choice != EXIT_CHOICE_NUMBER)
            {
                this.ShowMenu();
                bool parsed = int.TryParse(Console.ReadLine(), out choice);
                if (!parsed)
                {
                    this.ShowMenu();
                    continue;
                }
                switch (choice)
                {
                    case 1:
                        {
                            Engine engine = new Engine(this.Renderer, this.Highscore, this.Player);
                            engine.Start();
                            Console.ReadKey();
                            break;
                        }
                    case 2:
                        {
                            this.Renderer.Clear();
                            for (int i = 0; i < this.Highscore.HighscoreEntries.Count; i++ )
                            {
                                Player highscoreEntry = this.Highscore.HighscoreEntries[i];
                                Console.WriteLine("{0}. {1, -15}{2, 10}", i + 1, highscoreEntry.Name, highscoreEntry.Score);
                            }

                            Console.ReadKey();
                            break;
                        }
                    case 3:
                        {
                            return;
                        }
                    default:
                        {
                            this.ShowMenu();
                            break;
                        }
                }
            }
        }

        private void ShowMenu()
        {
            this.Renderer.Clear();
            this.Renderer.ShowMessage(0, 0,
@"1. Start game
2. Highscore
3. Exit
");
        }
    }
}
