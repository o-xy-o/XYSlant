
namespace Slant
{
    partial class FieldSolver
    {
        public bool tryOneOne(ref Line solution)
        {
            if (game.Number[x, y] == 1 && x > 0 && y > 0)
            {
                for (int i = x - 1; i >= 0; --i)
                {
                    switch (game.Number[i, y])
                    {
                        case 2:
                            continue;
                        case 1:
                            solution = Line.UP;
                            return true;
                    }
                    break;
                }
                for (int i = y - 1; i >= 0; --i)
                {
                    switch (game.Number[x, i])
                    {
                        case 2:
                            continue;
                        case 1:
                            solution = Line.UP;
                            return true;
                    }
                    break;
                }
            }
            if (game.Number[x + 1, y] == 1 && y > 0 && x + 1 < game.Width)
            {
                for (int i = x + 2; i <= game.Width; ++i)
                {
                    switch (game.Number[i, y])
                    {
                        case 2:
                            continue;
                        case 1:
                            solution = Line.DOWN;
                            return true;
                    }
                    break;
                }
                for (int i = y - 1; i >= 0; --i)
                {
                    switch (game.Number[x + 1, i])
                    {
                        case 2:
                            continue;
                        case 1:
                            solution = Line.DOWN;
                            return true;
                    }
                    break;
                }
            }
            if (game.Number[x, y + 1] == 1 && x > 0 && y + 1 < game.Height)
            {
                for (int i = x - 1; i >= 0; --i)
                {
                    switch (game.Number[i, y + 1])
                    {
                        case 2:
                            continue;
                        case 1:
                            solution = Line.DOWN;
                            return true;
                    }
                    break;
                }
                for (int i = y + 2; i <= game.Height; ++i)
                {
                    switch (game.Number[x, i])
                    {
                        case 2:
                            continue;
                        case 1:
                            solution = Line.DOWN;
                            return true;
                    }
                    break;
                }
            }
            if (game.Number[x + 1, y + 1] == 1 && x + 1 < game.Width && y + 1 < game.Height)
            {
                for (int i = x + 2; i <= game.Width; ++i)
                {
                    switch (game.Number[i, y + 1])
                    {
                        case 2:
                            continue;
                        case 1:
                            solution = Line.UP;
                            return true;
                    }
                    break;
                }
                for (int i = y + 2; i <= game.Height; ++i)
                {
                    switch (game.Number[x + 1, i])
                    {
                        case 2:
                            continue;
                        case 1:
                            solution = Line.UP;
                            return true;
                    }
                    break;
                }
            }
            return false;
        }

        public bool tryThreeThree(ref Line solution)
        {
            if (game.Number[x, y] == 3)
            {
                for (int i = x - 1; i >= 0; --i)
                {
                    switch (game.Number[i, y])
                    {
                        case 2:
                            continue;
                        case 3:
                            solution = Line.DOWN;
                            return true;
                    }
                    break;
                }
                for (int i = y - 1; i >= 0; --i)
                {
                    switch (game.Number[x, i])
                    {
                        case 2:
                            continue;
                        case 3:
                            solution = Line.DOWN;
                            return true;
                    }
                    break;
                }
            }
            if (game.Number[x + 1, y] == 3)
            {
                for (int i = x + 2; i <= game.Width; ++i)
                {
                    switch (game.Number[i, y])
                    {
                        case 2:
                            continue;
                        case 3:
                            solution = Line.UP;
                            return true;
                    }
                    break;
                }
                for (int i = y - 1; i >= 0; --i)
                {
                    switch (game.Number[x + 1, i])
                    {
                        case 2:
                            continue;
                        case 3:
                            solution = Line.UP;
                            return true;
                    }
                    break;
                }
            }
            if (game.Number[x, y + 1] == 3)
            {
                for (int i = x - 1; i >= 0; --i)
                {
                    switch (game.Number[i, y + 1])
                    {
                        case 2:
                            continue;
                        case 3:
                            solution = Line.UP;
                            return true;
                    }
                    break;
                }
                for (int i = y + 2; i <= game.Height; ++i)
                {
                    switch (game.Number[x, i])
                    {
                        case 2:
                            continue;
                        case 3:
                            solution = Line.UP;
                            return true;
                    }
                    break;
                }
            }
            if (game.Number[x + 1, y + 1] == 3)
            {
                for (int i = x + 2; i <= game.Width; ++i)
                {
                    switch (game.Number[i, y + 1])
                    {
                        case 2:
                            continue;
                        case 3:
                            solution = Line.DOWN;
                            return true;
                    }
                    break;
                }
                for (int i = y + 2; i <= game.Height; ++i)
                {
                    switch (game.Number[x + 1, i])
                    {
                        case 2:
                            continue;
                        case 3:
                            solution = Line.DOWN;
                            return true;
                    }
                    break;
                }
            }
            return false;
        }

        public bool tryDiagonalOnes(ref Line solution)
        {
            if (isNotAtBorder(x, y))
            {
                if (game.Number[x, y] == 1 && game.Number[x + 1, y + 1] == 1)
                {
                    solution = Line.UP;
                    return true;
                }
                else if (game.Number[x + 1, y] == 1 && game.Number[x, y + 1] == 1)
                {
                    solution = Line.DOWN;
                    return true;
                }
            }
            return false;
        }
    }
}
