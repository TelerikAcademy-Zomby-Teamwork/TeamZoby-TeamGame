namespace Frogger
{
    public class Player
    {
        public string Name;
        public int Score;
        public int LivesCount;

        public Player(string name, int score = 0, int livesCount = 5)
        {
            this.Name = name;
            this.Score = score;
            this.LivesCount = livesCount;
        }
    }
}
