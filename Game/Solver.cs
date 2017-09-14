
namespace Slant
{
    public abstract class Solver : Game
    {
        public Solver(Game game) : base(game) { }

        public abstract int solve();
    }
}
