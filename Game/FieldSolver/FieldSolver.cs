using System.Runtime.CompilerServices;

namespace Slant
{
    public partial class FieldSolver
    {
        protected readonly Game game;
        public readonly int x, y;

        public readonly FastLineFollower flf;

        public FieldSolver(Game game, int x, int y)
        {
            this.game = game;
            this.x = x;
            this.y = y;
            flf = new FastLineFollower(game, x, y);
        }

        public virtual Line solve(Difficulty dif)
        {
            if (game[x, y] != Line.NONE)
                return game[x, y];
            Line solution = Line.NONE;
            bool done = false;
            switch ((Difficulty)dif)
            {
                default:
                    done = done
                        || tryOneOther(ref solution) // includes tryOneOne
                        || tryThreeOther(ref solution) // includes tryThreeThree
                        || tryTwoOne(ref solution)
                        || tryTwoThree(ref solution)
                        || tryDiagonalOneTwo(ref solution) // includes tryDiagonalOnes
                        || tryDiagonalThreeOnes(ref solution)
                        || tryDiagonalTunnel(ref solution);
                    goto case Difficulty.EASY; // skip medium, because hard includes all
                case Difficulty.MEDIUM:
                    done = done
                        || tryDiagonalOnes(ref solution)
                        || tryOneOne(ref solution)
                        || tryThreeThree(ref solution);
                    goto case Difficulty.EASY;
                case Difficulty.EASY:
                    done = done
                        || tryNumberStates(ref solution)
                        || tryLineRings(ref solution);
                    break;
            }
            return solution;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool isNotAtBorder(int x, int y)
        {
            return x > 0 && y > 0 && x + 1 < game.Width && y + 1 < game.Height;
        }
    }
}
