using System;
using System.Threading;

namespace Slant
{
    public enum Difficulty
    {
        EASY, MEDIUM, HARD, EXTREME
    }

    public class EasyPatternSolver : PatternSolver
    {
        public EasyPatternSolver(Game game) : base(game) { }
        public override int solve()
        {
            return solve(Difficulty.EASY, MAXROUNDS);
        }
        public int solve(int maxRounds)
        {
            return solve(Difficulty.EASY, maxRounds);
        }
    }
    public class MediumPatternSolver : PatternSolver
    {
        public MediumPatternSolver(Game game) : base(game) { }
        public override int solve()
        {
            return solve(Difficulty.MEDIUM, MAXROUNDS);
        }
        public int solve(int maxRounds)
        {
            return solve(Difficulty.MEDIUM, maxRounds);
        }
    }
    public class HardPatternSolver : PatternSolver
    {
        public HardPatternSolver(Game game) : base(game) { }
        public override int solve()
        {
            return solve(Difficulty.HARD, MAXROUNDS);
        }
        public int solve(int maxRounds)
        {
            return solve(Difficulty.HARD, maxRounds);
        }
    }

    public class ExtremePatternSolver : PatternSolver
    {
        public ExtremePatternSolver(Game game) : base(game) { }
        public override int solve()
        {
            return solve(Difficulty.EXTREME, MAXROUNDS);
        }
        public int solve(int maxRounds)
        {
            return solve(Difficulty.EXTREME, maxRounds);
        }
    }

    public class PatternSolver : Solver
    {
        public const int MAXROUNDS = 50;
        protected readonly FieldSolver[] fieldSolvers;

        public PatternSolver(Game game) : base(game)
        {
            fieldSolvers = new FieldSolver[field.Length];
            for (int i = 0; i < fieldSolvers.Length; ++i)
            {
                fieldSolvers[i] = new FieldSolver(this, i % width, i / width);
            }
        }

        public override int solve()
        {
            return solve(Difficulty.HARD, MAXROUNDS);
        }

        public int solve(Difficulty dif, int maxRounds)
        {
            int cnt, round = 0;
            lock (this)
            {
                int[,] numbercopy = null;
                if (dif == Difficulty.EXTREME) numbercopy = (int[,])number.Clone();
                using (var cde = new CountdownEvent(fieldSolvers.Length))
                {
                    cnt = CountFree;
                    while (cnt > 0 && round < maxRounds)
                    {
                        if (dif == Difficulty.EXTREME) calcNumbers();
                        cde.Reset();
                        foreach (FieldSolver fs in fieldSolvers)
                        {
                            ThreadPool.QueueUserWorkItem(obj =>
                            {
                                field[fs.x, fs.y] = fs.solve(dif);
                                cde.Signal();
                            });
                        }
                        cde.Wait();
                        ++round;
                        int old = cnt;
                        cnt = CountFree;
                        if (cnt == old) break;
                    }
                }
                if (dif == Difficulty.EXTREME) Array.Copy(numbercopy, number, number.Length);
            }
            return cnt == 0 ? round : -round;
        }

        protected void calcNumbers()
        {
            for (int y = 0; y < height+1; ++y)
            {
                for (int x = 0; x < width+1; ++x)
                {
                    if (number[x, y] >= 0) continue;
                    int n=0, f=0;
                    switch (this[x, y])
                    {
                        case Line.NONE: ++f; break;
                        case Line.DOWN: ++n; break;
                    }
                    switch (this[x-1, y])
                    {
                        case Line.NONE: ++f; break;
                        case Line.UP: ++n; break;
                    }
                    switch (this[x, y-1])
                    {
                        case Line.NONE: ++f; break;
                        case Line.UP: ++n; break;
                    }
                    switch (this[x-1, y-1])
                    {
                        case Line.NONE: ++f; break;
                        case Line.DOWN: ++n; break;
                    }
                    if (f == 0) number[x, y] = n;
                }
            }
        }
    }
}
