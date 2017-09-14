using System;
using System.Collections.Generic;
using System.Linq;

namespace Slant
{
    public static class Extensions
    {
        // by Jon Skeet: http://stackoverflow.com/a/1287572
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            T[] elements = source.ToArray();
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                // Swap element "i" with a random earlier element it (or itself)
                // ... except we don't really need to swap it fully, as we can
                // return it immediately, and afterwards it's irrelevant.
                int swapIndex = rng.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }
    }

    class Creator : PatternSolver
    {
        public Creator(int width, int height)
            : base(new Game(width, height))
        { }

        public int generate(Random rng, int maxtries)
        {
            lock (this)
            {
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        field[x, y] = Line.UP;
                    }
                }

                int t = 0;
                while (t > -maxtries)
                {
                    --t;
                    if (randomChanges(field.Length / 2, rng))
                    {
                        t = -t;
                        break;
                    }
                }

                calcNumbers();

                return t;
            }
        }

        protected bool randomChanges(int changes, Random rng)
        {
            foreach (FieldSolver fs in fieldSolvers.Shuffle(rng))
            {
                Line d = (field[fs.x, fs.y] == Line.DOWN) ? Line.UP : Line.DOWN;
                if (!fs.flf.follow(d))
                {
                    field[fs.x, fs.y] = d;
                    if (--changes == 0) return true;
                }
            }
            return false;
        }

        protected new void calcNumbers()
        {
            for (int y = 0; y < height + 1; ++y)
            {
                for (int x = 0; x < width + 1; ++x)
                {
                    number[x, y] = 0;
                }
            }
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    switch (field[x, y])
                    {
                        case Line.DOWN:
                            ++number[x, y];
                            ++number[x + 1, y + 1];
                            break;
                        case Line.UP:
                            ++number[x, y + 1];
                            ++number[x + 1, y];
                            break;
                    }
                }
            }
        }

        public int eliminate(Random rng, Difficulty dif, int maxsolverounds)
        {
            lock (this)
            {
                if (CountFree > 0) throw new InvalidOperationException("first call generate()");

                int rnd = 0;
                foreach (int pt in Enumerable.Range(0,number.Length).Shuffle(rng))
                {
                    int x = pt % (width + 1), y = pt / (width + 1);
                    int num = number[x, y];
                    number[x, y] = -1;
                    clear();
                    int r = solve(dif, maxsolverounds);
                    if (r < 0) number[x, y] = num;
                    else rnd = r;
                }

                return rnd;
            }
        }

    }
}
