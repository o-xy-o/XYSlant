
namespace Slant
{
    partial class FieldSolver
    {
        public bool tryDiagonalOneTwo(ref Line solution)
        {
            if (isNotAtBorder(x, y))
            {
                if (game.Number[x, y] == 1 && followDiagonal(x, y, x + 1, y + 1))
                {
                    solution = Line.UP;
                    return true;
                }
                if (game.Number[x + 1, y] == 1 && followDiagonal(x, y, x, y + 1))
                {
                    solution = Line.DOWN;
                    return true;
                }
                if (game.Number[x, y + 1] == 1 && followDiagonal(x, y, x + 1, y))
                {
                    solution = Line.DOWN;
                    return true;
                }
                if (game.Number[x + 1, y + 1] == 1 && followDiagonal(x, y, x, y))
                {
                    solution = Line.UP;
                    return true;
                }
            }
            return false;
        }

        private bool followDiagonal(int x, int y, int nx, int ny)
        {
            int dx, dy;
            for (int i = (game.Width - 2) * (game.Height - 2); i > 0; --i)
            {
                switch (game.Number[nx, ny])
                {
                    case 1: return true;
                    case 2: break;
                    default: return false;
                }
                if (isNotAtBorder(dx = nx, dy = ny)
                    && (dx != x || dy != y) && game[dx, dy] == Line.DOWN)
                { x = dx; y = dy; ++nx; ++ny; }
                else if (isNotAtBorder(dx = nx - 1, dy = ny)
                    && (dx != x || dy != y) && game[dx, dy] == Line.UP)
                { x = dx; y = dy; --nx; ++ny; }
                else if (isNotAtBorder(dx = nx, dy = ny - 1)
                    && (dx != x || dy != y) && game[dx, dy] == Line.UP)
                { x = dx; y = dy; ++nx; --ny; }
                else if (isNotAtBorder(dx = nx - 1, dy = ny - 1)
                    && (dx != x || dy != y) && game[dx, dy] == Line.DOWN)
                { x = dx; y = dy; --nx; --ny; }
                else return false;
            }
            return false;
        }



        public bool tryDiagonalThreeOnes(ref Line solution)
        {
            if (isNotAtBorder(x, y))
            {
                if ((x - 1 > 0 && y - 1 > 0 && game.Number[x - 1, y - 1] == 1
                    && game.Number[x - 1, y + 1] == 1 && game.Number[x + 1, y - 1] == 1)
                    || (x + 2 < game.Width && y + 2 < game.Height && game.Number[x + 2, y + 2] == 1
                    && game.Number[x, y + 2] == 1 && game.Number[x + 2, y] == 1))
                {
                    solution = Line.DOWN;
                    return true;
                }
                if ((x - 1 > 0 && y + 2 < game.Height && game.Number[x - 1, y + 2] == 1
                    && game.Number[x - 1, y] == 1 && game.Number[x + 1, y + 2] == 1)
                    || (x + 2 < game.Width && y - 1 > 0 && game.Number[x + 2, y - 1] == 1
                    && game.Number[x, y - 1] == 1 && game.Number[x + 2, y + 1] == 1))
                {
                    solution = Line.UP;
                    return true;
                }
            }
            return false;
        }

        public bool tryDiagonalTunnel(ref Line solution)
        {
            if (x - 1 > 0 && y - 1 > 0 && game.Number[x - 1, y - 1] == 1
                && game[x, y - 1] == Line.DOWN && game[x - 1, y] == Line.DOWN)
            {
                solution = Line.DOWN;
                return true;
            }
            if (x + 2 < game.Width && y - 1 > 0 && game.Number[x + 2, y - 1] == 1
                && game[x, y - 1] == Line.UP && game[x + 1, y] == Line.UP)
            {
                solution = Line.UP;
                return true;
            }
            if (x - 1 > 0 && y + 2 < game.Height && game.Number[x - 1, y + 2] == 1
                && game[x, y + 1] == Line.UP && game[x - 1, y] == Line.UP)
            {
                solution = Line.UP;
                return true;
            }
            if (x + 2 < game.Width && y + 2 < game.Height && game.Number[x + 2, y + 2] == 1
                && game[x, y + 1] == Line.DOWN && game[x + 1, y] == Line.DOWN)
            {
                solution = Line.DOWN;
                return true;
            }

            if (x - 1 > 0 && y > 0 && game.Number[x - 1, y] == 1
                && game[x, y + 1] == Line.UP && game[x - 1, y + 1] == Line.DOWN)
            {
                solution = Line.UP;
                return true;
            }
            if (x + 1 < game.Width && y - 1 > 0 && game.Number[x + 1, y - 1] == 1
                && game[x - 1, y] == Line.DOWN && game[x - 1, y - 1] == Line.UP)
            {
                solution = Line.DOWN;
                return true;
            }
            if (x > 0 && y + 2 < game.Height && game.Number[x, y + 2] == 1
                && game[x + 1, y] == Line.DOWN && game[x + 1, y + 1] == Line.UP)
            {
                solution = Line.DOWN;
                return true;
            }
            if (x + 2 < game.Width && y + 1 < game.Height && game.Number[x + 2, y + 1] == 1
                && game[x, y - 1] == Line.UP && game[x + 1, y - 1] == Line.DOWN)
            {
                solution = Line.UP;
                return true;
            }

            if (x > 0 && y - 1 > 0 && game.Number[x, y - 1] == 1
                && game[x + 1, y] == Line.UP && game[x + 1, y - 1] == Line.DOWN)
            {
                solution = Line.UP;
                return true;
            }
            if (x + 2 < game.Width && y > 0 && game.Number[x + 2, y] == 1
                && game[x, y + 1] == Line.DOWN && game[x + 1, y + 1] == Line.UP)
            {
                solution = Line.DOWN;
                return true;
            }
            if (x - 1 > 0 && y + 1 < game.Height && game.Number[x - 1, y + 1] == 1
                && game[x, y - 1] == Line.DOWN && game[x - 1, y - 1] == Line.UP)
            {
                solution = Line.DOWN;
                return true;
            }
            if (x + 1 < game.Width && y + 2 < game.Height && game.Number[x + 1, y + 2] == 1
                && game[x - 1, y] == Line.UP && game[x - 1, y + 1] == Line.DOWN)
            {
                solution = Line.UP;
                return true;
            }
            return false;
        }
    }
}
