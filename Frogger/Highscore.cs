using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Frogger
{
    public class Highscore
    {
        private const int MAX_NUMBER_OF_ENTRIES = 5;

        public string FilePath
        {
            get;
            private set;
        }

        private List<Player> highscoreEntries;
        public List<Player> HighscoreEntries
        {
            get
            {
                if (this.highscoreEntries != null)
                {
                    return this.highscoreEntries;
                }

                this.highscoreEntries = new List<Player>();
                if(!File.Exists(this.FilePath))
                {
                    File.Create(this.FilePath).Close();
                    
                }
                using (StreamReader reader = new StreamReader(this.FilePath))
                {
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Player player = new Player(line.Substring(0, 15).Trim(), int.Parse(line.Substring(15, 10).Trim()));
                        this.highscoreEntries.Add(player);
                    }
                    return this.highscoreEntries;
                }
            }
            set
            {
                this.highscoreEntries = value;
            }
        }

        public Highscore(string filePath)
        {
            this.FilePath = filePath;
        }

        public void AddHighscoreEntry(Player player)
        {
            this.HighscoreEntries.Add(player);
            this.HighscoreEntries.Sort((firstPlayer, secondPlayer) => (firstPlayer.Score > secondPlayer.Score) ? -1 : 1);
            this.HighscoreEntries = this.HighscoreEntries.Take(MAX_NUMBER_OF_ENTRIES).ToList();
        }

        public void Persist()
        {
            using (StreamWriter writer = new StreamWriter(this.FilePath))
            {
                foreach (Player entry in this.HighscoreEntries)
                {
                    writer.WriteLine("{0, -15}{1, 10}", entry.Name, entry.Score);
                }
            }
        }
    }
}
