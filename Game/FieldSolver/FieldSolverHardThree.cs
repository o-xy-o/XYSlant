
namespace Slant
{
    partial class FieldSolver
    {
        public bool tryThreeOther(ref Line solution)
        {
            if (game.Number[x, y] == 3)
            {
                for (int i = x - 1; i >= 0; --i)
                {
                    switch (game.Number[i, y])
                    {
                        case 1:
                            if (game[i - 1, y] == Line.DOWN && game[i - 1, y - 1] == Line.UP)
                            {
                                solution = Line.DOWN;
                                return true;
                            }
                            break;
                        case 2:
                            if (game[i - 1, y] == Line.DOWN || game[i - 1, y - 1] == Line.UP)
                            {
                                solution = Line.DOWN;
                                return true;
                            }
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
                        case 1:
                            if (game[x, i - 1] == Line.DOWN && game[x - 1, i - 1] == Line.UP)
                            {
                                solution = Line.DOWN;
                                return true;
                            }
                            break;
                        case 2:
                            if (game[x, i - 1] == Line.DOWN || game[x - 1, i - 1] == Line.UP)
                            {
                                solution = Line.DOWN;
                                return true;
                            }
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
                        case 1:
                            if (game[i, y] == Line.UP && game[i, y - 1] == Line.DOWN)
                            {
                                solution = Line.UP;
                                return true;
                            }
                            break;
                        case 2:
                            if (game[i, y] == Line.UP || game[i, y - 1] == Line.DOWN)
                            {
                                solution = Line.UP;
                                return true;
                            }
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
                        case 1:
                            if (game[x, i - 1] == Line.UP && game[x + 1, i - 1] == Line.DOWN)
                            {
                                solution = Line.UP;
                                return true;
                            }
                            break;
                        case 2:
                            if (game[x, i - 1] == Line.UP || game[x + 1, i - 1] == Line.DOWN)
                            {
                                solution = Line.UP;
                                return true;
                            }
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
                        case 1:
                            if (game[i - 1, y] == Line.UP && game[i - 1, y + 1] == Line.DOWN)
                            {
                                solution = Line.UP;
                                return true;
                            }
                            break;
                        case 2:
                            if (game[i - 1, y] == Line.UP || game[i - 1, y + 1] == Line.DOWN)
                            {
                                solution = Line.UP;
                                return true;
                            }
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
                        case 1:
                            if (game[x, i] == Line.UP && game[x - 1, i] == Line.DOWN)
                            {
                                solution = Line.UP;
                                return true;
                            }
                            break;
                        case 2:
                            if (game[x, i] == Line.UP || game[x - 1, i] == Line.DOWN)
                            {
                                solution = Line.UP;
                                return true;
                            }
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
                        case 1:
                            if (game[i, y] == Line.DOWN && game[i, y + 1] == Line.UP)
                            {
                                solution = Line.DOWN;
                                return true;
                            }
                            break;
                        case 2:
                            if (game[i, y] == Line.DOWN || game[i, y + 1] == Line.UP)
                            {
                                solution = Line.DOWN;
                                return true;
                            }
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
                        case 1:
                            if (game[x, i] == Line.DOWN && game[x + 1, i] == Line.UP)
                            {
                                solution = Line.DOWN;
                                return true;
                            }
                            break;
                        case 2:
                            if (game[x, i] == Line.DOWN || game[x + 1, i] == Line.UP)
                            {
                                solution = Line.DOWN;
                                return true;
                            }
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

        public bool tryTwoThree(ref Line solution)
        {
            if (game.Number[x, y] == 2)
            {
                if (game[x, y - 1] == Line.DOWN)
                {
                    for (int i = x - 1; i >= 0; --i)
                    {
                        switch (game.Number[i, y])
                        {
                            case 1:
                                if (game[i - 1, y] == Line.DOWN && game[i - 1, y - 1] == Line.UP)
                                {
                                    solution = Line.DOWN;
                                    return true;
                                }
                                break;
                            case 2:
                                if (game[i - 1, y] == Line.DOWN || game[i - 1, y - 1] == Line.UP)
                                {
                                    solution = Line.DOWN;
                                    return true;
                                }
                                continue;
                            case 3:
                                solution = Line.DOWN;
                                return true;
                        }
                        break;
                    }
                }
                if (game[x - 1, y] == Line.DOWN)
                {
                    for (int i = y - 1; i >= 0; --i)
                    {
                        switch (game.Number[x, i])
                        {
                            case 1:
                                if (game[x, i - 1] == Line.DOWN && game[x - 1, i - 1] == Line.UP)
                                {
                                    solution = Line.DOWN;
                                    return true;
                                }
                                break;
                            case 2:
                                if (game[x, i - 1] == Line.DOWN || game[x - 1, i - 1] == Line.UP)
                                {
                                    solution = Line.DOWN;
                                    return true;
                                }
                                continue;
                            case 3:
                                solution = Line.DOWN;
                                return true;
                        }
                        break;
                    }
                }
            }
            if (game.Number[x + 1, y] == 2)
            {
                if (game[x, y - 1] == Line.UP)
                {
                    for (int i = x + 2; i <= game.Width; ++i)
                    {
                        switch (game.Number[i, y])
                        {
                            case 1:
                                if (game[i, y] == Line.UP && game[i, y - 1] == Line.DOWN)
                                {
                                    solution = Line.UP;
                                    return true;
                                }
                                break;
                            case 2:
                                if (game[i, y] == Line.UP || game[i, y - 1] == Line.DOWN)
                                {
                                    solution = Line.UP;
                                    return true;
                                }
                                continue;
                            case 3:
                                solution = Line.UP;
                                return true;
                        }
                        break;
                    }
                }
                if (game[x + 1, y] == Line.UP)
                {
                    for (int i = y - 1; i >= 0; --i)
                    {
                        switch (game.Number[x + 1, i])
                        {
                            case 1:
                                if (game[x, i - 1] == Line.UP && game[x + 1, i - 1] == Line.DOWN)
                                {
                                    solution = Line.UP;
                                    return true;
                                }
                                break;
                            case 2:
                                if (game[x, i - 1] == Line.UP || game[x + 1, i - 1] == Line.DOWN)
                                {
                                    solution = Line.UP;
                                    return true;
                                }
                                continue;
                            case 3:
                                solution = Line.UP;
                                return true;
                        }
                        break;
                    }
                }
            }
            if (game.Number[x, y + 1] == 2)
            {
                if (game[x, y + 1] == Line.UP)
                {
                    for (int i = x - 1; i >= 0; --i)
                    {
                        switch (game.Number[i, y + 1])
                        {
                            case 1:
                                if (game[i - 1, y] == Line.UP && game[i - 1, y + 1] == Line.DOWN)
                                {
                                    solution = Line.UP;
                                    return true;
                                }
                                break;
                            case 2:
                                if (game[i - 1, y] == Line.UP || game[i - 1, y + 1] == Line.DOWN)
                                {
                                    solution = Line.UP;
                                    return true;
                                }
                                continue;
                            case 3:
                                solution = Line.UP;
                                return true;
                        }
                        break;
                    }
                }
                if (game[x - 1, y] == Line.UP)
                {
                    for (int i = y + 2; i <= game.Height; ++i)
                    {
                        switch (game.Number[x, i])
                        {
                            case 1:
                                if (game[x, i] == Line.UP && game[x - 1, i] == Line.DOWN)
                                {
                                    solution = Line.UP;
                                    return true;
                                }
                                break;
                            case 2:
                                if (game[x, i] == Line.UP || game[x - 1, i] == Line.DOWN)
                                {
                                    solution = Line.UP;
                                    return true;
                                }
                                continue;
                            case 3:
                                solution = Line.UP;
                                return true;
                        }
                        break;
                    }
                }
            }
            if (game.Number[x + 1, y + 1] == 2)
            {
                if (game[x, y + 1] == Line.DOWN)
                {
                    for (int i = x + 2; i <= game.Width; ++i)
                    {
                        switch (game.Number[i, y + 1])
                        {
                            case 1:
                                if (game[i, y] == Line.DOWN && game[i, y + 1] == Line.UP)
                                {
                                    solution = Line.DOWN;
                                    return true;
                                }
                                break;
                            case 2:
                                if (game[i, y] == Line.DOWN || game[i, y + 1] == Line.UP)
                                {
                                    solution = Line.DOWN;
                                    return true;
                                }
                                continue;
                            case 3:
                                solution = Line.DOWN;
                                return true;
                        }
                        break;
                    }
                }
                if (game[x + 1, y] == Line.DOWN)
                {
                    for (int i = y + 2; i <= game.Height; ++i)
                    {
                        switch (game.Number[x + 1, i])
                        {
                            case 1:
                                if (game[x, i] == Line.DOWN && game[x + 1, i] == Line.UP)
                                {
                                    solution = Line.DOWN;
                                    return true;
                                }
                                break;
                            case 2:
                                if (game[x, i] == Line.DOWN || game[x + 1, i] == Line.UP)
                                {
                                    solution = Line.DOWN;
                                    return true;
                                }
                                continue;
                            case 3:
                                solution = Line.DOWN;
                                return true;
                        }
                        break;
                    }
                }
            }
            return false;
        }
    }
}
