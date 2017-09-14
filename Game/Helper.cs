using System;

namespace Slant
{
    public class EasyHelper : EasyPatternSolver
    {
        public EasyHelper(Game game) : base(game) { }
        public override int solve()
        {
            return solve(1);
        }
    }

    public class MediumHelper : MediumPatternSolver
    {
        public MediumHelper(Game game) : base(game) { }
        public override int solve()
        {
            return solve(1);
        }
    }

    public class HardHelper : HardPatternSolver
    {
        public HardHelper(Game game) : base(game) { }
        public override int solve()
        {
            return solve(1);
        }
    }

    public class ExtremeHelper : ExtremePatternSolver
    {
        public ExtremeHelper(Game game) : base(game) { }
        public override int solve()
        {
            return solve(1);
        }
    }

    public class HintHelper : PatternSolver
    {
        protected readonly Random rng;
        public HintHelper(Game game, Random rng) : base(game)
        {
            this.rng = rng;
        }
        public override int solve()
        {
            int r = 0;
            lock (this)
            {
                int[,] numbercopy = (int[,])number.Clone();
                calcNumbers();
                foreach (FieldSolver fs in fieldSolvers.Shuffle(rng))
                {
                    if (field[fs.x, fs.y] == Line.NONE)
                    {
                        --r;
                        Line d = fs.solve(Difficulty.EXTREME);
                        if(d != Line.NONE)
                        {
                            field[fs.x, fs.y] = d;
                            r = -r;
                            break;
                        }
                    }
                }
                Array.Copy(numbercopy, number, number.Length);
            }
            return r;
        }
    }
}
