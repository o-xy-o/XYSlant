using System;
using System.Collections.Generic;
using System.Threading;

namespace Slant
{
    public class BacktrackingSolver : Solver
    {
        public BacktrackingSolver(Game game) : base(game)
        { }

        protected CountdownEvent cde;
        protected readonly List<Line[,]> solutions = new List<Line[,]>();

        public override int solve()
        {
            lock (this)
            {
                solutions.Clear();
                using (cde = new CountdownEvent(1))
                {
                    ThreadPool.QueueUserWorkItem(
                                new BTSolver(this, this).solve, 0);
                    cde.Wait();
                }
                if (solutions.Count > 0)
                    Array.Copy(solutions[0], field, field.Length);
                return solutions.Count;
            }
        }

        protected class BTSolver : Game
        {
            protected readonly BacktrackingSolver sync;

            public BTSolver(Game game, BacktrackingSolver sync) : base(game)
            {
                this.sync = sync;
            }

            public void solve(object pt)
            {
                int xs = ((int)pt) % width;
                for (int y = ((int)pt) / width; y < height; ++y)
                {
                    for (int x = xs; x < width; ++x)
                    {
                        xs = 0;
                        if (field[x, y] != Line.NONE) continue;
                        var nsc = new NumberStateChecker(this, x, y);
                        var flf = new FastLineFollower(this, x, y);
                        bool down = nsc.isError(Line.DOWN) || flf.follow(Line.DOWN);
                        bool up = nsc.isError(Line.UP) || flf.follow(Line.UP);
                        if (up && down)
                        {
                            sync.cde.Signal();
                            return;
                        }
                        else if (up) field[x, y] = Line.DOWN;
                        else if (down) field[x, y] = Line.UP;
                        else
                        {
                            field[x, y] = Line.UP;
                            var bts = new BTSolver(this, sync);
                            sync.cde.AddCount();
                            ThreadPool.QueueUserWorkItem(
                                bts.solve, y * width + x + 1);
                            field[x, y] = Line.DOWN;
                        }
                    }
                }
                lock (sync.solutions) sync.solutions.Add(field);
                sync.cde.Signal();
            }

            protected class NumberStateChecker
            {
                protected int[] need, free;
                public NumberStateChecker(Game game, int x, int y)
                {
                    need = new int[] { game.Number[x, y], game.Number[x + 1, y],
                        game.Number[x, y + 1], game.Number[x + 1, y + 1] };
                    free = new int[4];

                    switch (game[x - 1, y - 1])
                    {
                        case Line.NONE: ++free[0]; break;
                        case Line.DOWN: --need[0]; break;
                    }
                    switch (game[x + 1, y - 1])
                    {
                        case Line.NONE: ++free[1]; break;
                        case Line.UP: --need[1]; break;
                    }
                    switch (game[x - 1, y + 1])
                    {
                        case Line.NONE: ++free[2]; break;
                        case Line.UP: --need[2]; break;
                    }
                    switch (game[x + 1, y + 1])
                    {
                        case Line.NONE: ++free[3]; break;
                        case Line.DOWN: --need[3]; break;
                    }

                    switch (game[x, y - 1])
                    {
                        case Line.NONE: ++free[0]; ++free[1]; break;
                        case Line.DOWN: --need[1]; break;
                        case Line.UP: --need[0]; break;
                    }
                    switch (game[x - 1, y])
                    {
                        case Line.NONE: ++free[0]; ++free[2]; break;
                        case Line.DOWN: --need[2]; break;
                        case Line.UP: --need[0]; break;
                    }
                    switch (game[x + 1, y])
                    {
                        case Line.NONE: ++free[1]; ++free[3]; break;
                        case Line.DOWN: --need[1]; break;
                        case Line.UP: --need[3]; break;
                    }
                    switch (game[x, y + 1])
                    {
                        case Line.NONE: ++free[2]; ++free[3]; break;
                        case Line.DOWN: --need[2]; break;
                        case Line.UP: --need[3]; break;
                    }
                }
                public bool isError(Line d)
                {
                    switch (d)
                    {
                        case Line.DOWN:
                            return need[0] == 0 || need[3] == 0
                                || need[1] > free[1] || need[2] > free[2];
                        case Line.UP:
                            return need[1] == 0 || need[2] == 0
                                || need[0] > free[0] || need[3] > free[3];
                        default:
                            return false;
                    }
                }
            }
        }
    }
}
