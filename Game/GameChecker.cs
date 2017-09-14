using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slant
{
    public enum NumberStates
    {
        NONE, ERROR, COMPLETED
    }

    class GameChecker
    {
        protected readonly Game game;
        protected readonly NumberStates[,] numberState;
        protected readonly int[,] lineError;

        public bool this[int x, int y]
        {
            get { return lineError[x, y] > 0; }
        }

        private class NumberStateIndexer : IIndexer<NumberStates>
        {
            private readonly GameChecker checker;
            public NumberStateIndexer(GameChecker checker)
            {
                this.checker = checker;
            }
            public NumberStates this[int x, int y]
            {
                get { return checker.numberState[x, y]; }
            }
        }

        public readonly IIndexer<NumberStates> NumberState;

        public GameChecker(Game game)
        {
            this.game = game;
            this.lineError = new int[game.Width, game.Height];
            this.numberState = new NumberStates[game.Width + 1, game.Height + 1];

            NumberState = new NumberStateIndexer(this);
        }

        public bool isNoError()
        {
            foreach (NumberStates state in numberState)
            {
                if (state == NumberStates.ERROR) return false;
            }
            foreach (int le in lineError)
            {
                if (le > 0) return false;
            }
            return true;
        }

        public void clear()
        {
            for (int y = 0; y < game.Height; ++y)
            {
                for (int x = 0; x < game.Width; ++x)
                {
                    lineError[x, y] = 0;
                    numberState[x, y] = NumberStates.NONE;
                }
            }
            for (int y = 0; y <= game.Height; ++y)
            {
                numberState[game.Width, y] = NumberStates.NONE;
            }
            for (int x = 0; x < game.Width; ++x)
            {
                numberState[x, game.Height] = NumberStates.NONE;
            }
        }

        public void removeLineError(int x, int y)
        {
            if (lineError[x, y] > 0) new LineFollower(this, x, y, false).follow();
        }

        public void checkError(int x, int y)
        {
            checkNumberState(x, y);
            new LineFollower(this, x, y, true).follow();
        }

        public void checkAllNumbers()
        {
            int[,] free = new int[game.Width + 1, game.Height + 1],
                con = new int[game.Width + 1, game.Height + 1];
            for (int y = 0; y < game.Height; ++y)
            {
                for (int x = 0; x < game.Width; ++x)
                {
                    switch (game[x, y])
                    {
                        case Line.NONE:
                            ++free[x, y];
                            ++free[x + 1, y];
                            ++free[x, y + 1];
                            ++free[x + 1, y + 1];
                            break;
                        case Line.DOWN:
                            ++con[x, y];
                            ++con[x + 1, y + 1];
                            break;
                        case Line.UP:
                            ++con[x + 1, y];
                            ++con[x, y + 1];
                            break;
                    }
                }
            }
            for (int y = 0; y < game.Height + 1; ++y)
            {
                for (int x = 0; x < game.Width + 1; ++x)
                {
                    setNumberState(x, y, con[x, y], free[x, y]);
                }
            }
        }

        private void checkNumberState(int x, int y)
        {
            int[] free = new int[4], con = new int[4];

            switch (game[x - 1, y - 1])
            {
                case Line.NONE: ++free[0]; break;
                case Line.DOWN: ++con[0]; break;
            }
            switch (game[x + 1, y - 1])
            {
                case Line.NONE: ++free[1]; break;
                case Line.UP: ++con[1]; break;
            }
            switch (game[x - 1, y + 1])
            {
                case Line.NONE: ++free[2]; break;
                case Line.UP: ++con[2]; break;
            }
            switch (game[x + 1, y + 1])
            {
                case Line.NONE: ++free[3]; break;
                case Line.DOWN: ++con[3]; break;
            }

            switch (game[x, y - 1])
            {
                case Line.NONE: ++free[0]; ++free[1]; break;
                case Line.DOWN: ++con[1]; break;
                case Line.UP: ++con[0]; break;
            }
            switch (game[x - 1, y])
            {
                case Line.NONE: ++free[0]; ++free[2]; break;
                case Line.DOWN: ++con[2]; break;
                case Line.UP: ++con[0]; break;
            }
            switch (game[x + 1, y])
            {
                case Line.NONE: ++free[1]; ++free[3]; break;
                case Line.DOWN: ++con[1]; break;
                case Line.UP: ++con[3]; break;
            }
            switch (game[x, y + 1])
            {
                case Line.NONE: ++free[2]; ++free[3]; break;
                case Line.DOWN: ++con[2]; break;
                case Line.UP: ++con[3]; break;
            }

            switch (game[x, y])
            {
                case Line.NONE: ++free[0]; ++free[1]; ++free[2]; ++free[3]; break;
                case Line.DOWN: ++con[0]; ++con[3]; break;
                case Line.UP: ++con[1]; ++con[2]; break;
            }

            setNumberState(x, y, con[0], free[0]);
            setNumberState(x + 1, y, con[1], free[1]);
            setNumberState(x, y + 1, con[2], free[2]);
            setNumberState(x + 1, y + 1, con[3], free[3]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void setNumberState(int x, int y, int con, int free)
        {
            if (game.Number[x, y] < 0)
                numberState[x, y] = NumberStates.NONE;
            else if (game.Number[x, y] == con && free == 0)
                numberState[x, y] = NumberStates.COMPLETED;
            else if (con > game.Number[x, y] || game.Number[x, y] - con > free)
                numberState[x, y] = NumberStates.ERROR;
            else numberState[x, y] = NumberStates.NONE;
        }

        private class LineFollower
        {
            private readonly GameChecker checker;
            private readonly int sx, sy, diff;

            public LineFollower(GameChecker checker, int x, int y, bool add)
            {
                this.checker = checker;
                sx = x;
                sy = y;
                diff = add ? 1 : -1;
            }
            public bool follow()
            {
                return followRec(sx, sy, true, new HashSet<int>()) > 0;
            }
            private int followRec(int x, int y, bool r, HashSet<int> visited)
            {
                int pt = y << 16 | x;
                if (visited.Contains(pt))
                {
                    return (r && x == sx && y == sy) ? diff : 0;
                }
                HashSet<int> vis = new HashSet<int>(visited);
                vis.Add(pt);

                int cnt = 0;
                switch (checker.game[x, y])
                {
                    case Line.DOWN:
                        if (r)
                        {
                            cnt += checkNfollow(x, y + 1, Line.UP, false, vis);
                            cnt += checkNfollow(x + 1, y + 1, Line.DOWN, true, vis);
                            cnt += checkNfollow(x + 1, y, Line.UP, true, vis);
                        }
                        else
                        {
                            cnt += checkNfollow(x, y - 1, Line.UP, true, vis);
                            cnt += checkNfollow(x - 1, y - 1, Line.DOWN, false, vis);
                            cnt += checkNfollow(x - 1, y, Line.UP, false, vis);
                        }
                        break;
                    case Line.UP:
                        if (r)
                        {
                            cnt += checkNfollow(x + 1, y, Line.DOWN, true, vis);
                            cnt += checkNfollow(x + 1, y - 1, Line.UP, true, vis);
                            cnt += checkNfollow(x, y - 1, Line.DOWN, false, vis);
                        }
                        else
                        {
                            cnt += checkNfollow(x - 1, y, Line.DOWN, false, vis);
                            cnt += checkNfollow(x - 1, y + 1, Line.UP, false, vis);
                            cnt += checkNfollow(x, y + 1, Line.DOWN, true, vis);
                        }
                        break;
                }
                checker.lineError[x, y] += cnt;
                return cnt;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int checkNfollow(int x, int y, Line d, bool r, HashSet<int> vis)
            {
                return checker.game[x, y] == d ? followRec(x, y, r, vis) : 0;
            }
        }
    }
}
