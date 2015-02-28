namespace Frogger
{
    public class WinArea : Figure
    {
        public WinArea(int x, int y)
            : base(x, y, new char[][] { new char[] { ' ' } }, 0, 0, 0)
        {
        }

        public override bool IsObsticle()
        {
            return false;
        }
    }
}
