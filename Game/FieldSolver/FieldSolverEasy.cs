
namespace Slant
{
    partial class FieldSolver
    {
        public bool tryNumberStates(ref Line solution)
        {
            int[] free = new int[] { 1, 1, 1, 1 },
                need = new int[] { game.Number[x, y], game.Number[x + 1, y],
                        game.Number[x + 1, y + 1], game.Number[x, y + 1] };

            if (need[0] == 0 || need[2] == 0)
            {
                solution = Line.UP;
                return true;
            }
            if (need[1] == 0 || need[3] == 0)
            {
                solution = Line.DOWN;
                return true;
            }

            switch (game[x - 1, y])
            {
                case Line.NONE: ++free[0]; ++free[3]; break;
                case Line.UP: --need[0]; break;
                case Line.DOWN: --need[3]; break;
            }
            switch (game[x, y - 1])
            {
                case Line.NONE: ++free[0]; ++free[1]; break;
                case Line.UP: --need[0]; break;
                case Line.DOWN: --need[1]; break;
            }
            if (need[0] > 0)
            {
                switch (game[x - 1, y - 1])
                {
                    case Line.NONE: ++free[0]; break;
                    case Line.DOWN: --need[0]; break;
                }
                if (need[0] == free[0])
                {
                    solution = Line.DOWN;
                    return true;
                }
            }
            if (need[0] == 0)
            {
                solution = Line.UP;
                return true;
            }

            switch (game[x + 1, y])
            {
                case Line.NONE: ++free[1]; ++free[2]; break;
                case Line.UP: --need[2]; break;
                case Line.DOWN: --need[1]; break;
            }
            if (need[1] > 0)
            {
                switch (game[x + 1, y - 1])
                {
                    case Line.NONE: ++free[1]; break;
                    case Line.UP: --need[1]; break;
                }
                if (need[1] == free[1])
                {
                    solution = Line.UP;
                    return true;
                }
            }
            if (need[1] == 0)
            {
                solution = Line.DOWN;
                return true;
            }

            switch (game[x, y + 1])
            {
                case Line.NONE: ++free[2]; ++free[3]; break;
                case Line.UP: --need[2]; break;
                case Line.DOWN: --need[3]; break;
            }
            if (need[2] > 0)
            {
                switch (game[x + 1, y + 1])
                {
                    case Line.NONE: ++free[2]; break;
                    case Line.DOWN: --need[2]; break;
                }
                if (need[2] == free[2])
                {
                    solution = Line.DOWN;
                    return true;
                }
            }
            if (need[2] == 0)
            {
                solution = Line.UP;
                return true;
            }


            if (need[3] > 0)
            {
                switch (game[x - 1, y + 1])
                {
                    case Line.NONE: ++free[3]; break;
                    case Line.UP: --need[3]; break;
                }
                if (need[3] == free[3])
                {
                    solution = Line.UP;
                    return true;
                }
            }
            if (need[3] == 0)
            {
                solution = Line.DOWN;
                return true;
            }

            return false;
        }

        public bool tryLineRings(ref Line solution)
        {
            if (flf.follow(Line.UP))
            {
                solution = Line.DOWN;
                return true;
            }
            if (flf.follow(Line.DOWN))
            {
                solution = Line.UP;
                return true;
            }
            return false;
        }
    }
}
