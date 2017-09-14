using System.Collections.Generic;

namespace Slant
{
    public class FastLineFollower
    {
        private readonly Game game;
        private readonly int sx, sy;
        private readonly HashSet<int> visited = new HashSet<int>();
        private Line sd;

        public FastLineFollower(Game game, int x, int y)
        {
            this.game = game;
            sx = x;
            sy = y;
        }

        public bool follow(Line d)
        {
            sd = d;
            visited.Clear();
            visited.Add(sy << 16 | sx);
            return followRec(sx, sy, d, true);
        }

        private bool followRec(int x, int y, Line d, bool r)
        {
            switch (d)
            {
                case Line.DOWN:
                    if (r)
                    {
                        return checkNfollow(x, y + 1, Line.UP, false)
                        || checkNfollow(x + 1, y + 1, Line.DOWN, true)
                        || checkNfollow(x + 1, y, Line.UP, true);
                    }
                    else
                    {
                        return checkNfollow(x, y - 1, Line.UP, true)
                        || checkNfollow(x - 1, y - 1, Line.DOWN, false)
                        || checkNfollow(x - 1, y, Line.UP, false);
                    }
                case Line.UP:
                    if (r)
                    {
                        return checkNfollow(x + 1, y, Line.DOWN, true)
                        || checkNfollow(x + 1, y - 1, Line.UP, true)
                        || checkNfollow(x, y - 1, Line.DOWN, false);
                    }
                    else
                    {
                        return checkNfollow(x - 1, y, Line.DOWN, false)
                        || checkNfollow(x - 1, y + 1, Line.UP, false)
                        || checkNfollow(x, y + 1, Line.DOWN, true);
                    }
                default:
                    return false;
            }
        }

        private bool checkNfollow(int x, int y, Line d, bool r)
        {
            int pt = y << 16 | x;
            if (visited.Contains(pt))
            {
                return r && x == sx && y == sy && d == sd;
            }
            if (game[x, y] == d)
            {
                visited.Add(pt);
                return followRec(x, y, d, r);
            }
            return false;
        }
    }
}
