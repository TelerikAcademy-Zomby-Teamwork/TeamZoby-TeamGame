namespace Frogger
{
    public class Player
    {
        public string Name;
        public int Score;
        public int LivesCount;

        public Player(string name)
        {
            this.Name = name;
            this.Score = 0;
            this.LivesCount = 5;
        }
    }
}
