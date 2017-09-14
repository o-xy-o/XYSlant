using System;

namespace Slant
{
    public enum Line
    {
        NONE = 0, DOWN = 1, UP = 2, OUTSIDE = -1
    }

    public interface IIndexer<T>
    {
        T this[int x, int y] { get; }
    }

    public class Game
    {
        protected readonly int width, height;
        protected readonly Line[,] field;
        protected readonly int[,] number;

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public Line this[int x, int y]
        {
            get { return (x < 0 || y < 0 || x >= width || y >= height) ? Line.OUTSIDE : field[x, y]; }
        }

        public int CountSet
        {
            get
            {
                int c = 0;
                foreach(Line ln in field)
                {
                    if (ln != Line.NONE) ++c;
                }
                return c;
            }
        }
        public int CountFree
        {
            get
            {
                int c = 0;
                foreach (Line ln in field)
                {
                    if (ln == Line.NONE) ++c;
                }
                return c;
            }
        }


        private class NumberIndexer : IIndexer<int>
        {
            private readonly Game game;
            public NumberIndexer(Game game)
            {
                this.game = game;
            }
            public int this[int x, int y]
            {
                get { return game.number[x, y]; }
            }
        }

        public readonly IIndexer<int> Number;

        public Game(int[,] numbers, Line[,] lines = null)
        {
            this.number = numbers;
            this.width = numbers.GetLength(0) - 1;
            this.height = numbers.GetLength(1) - 1;

            if (lines != null)
            {
                if (lines.GetLength(0) == width && lines.GetLength(1) == height)
                    this.field = lines;
                else throw new ArgumentException("Line-Field size invalid.");
            }
            else this.field = new Line[width, height];

            Number = new NumberIndexer(this);
        }

        public Game(int width, int height) : this(new int[width + 1, height + 1])
        {
            for (int y = 0; y < height + 1; ++y)
            {
                for (int x = 0; x < width + 1; ++x)
                {
                    number[x, y] = -1;
                }
            }
        }

        public Game(Game other) : this((int[,])other.number.Clone(), (Line[,])other.field.Clone())
        { }

        public void toggle(int x, int y, int cnt)
        {
            field[x, y] = (Line)(((((int)field[x, y]) + cnt) % 3 + 3) % 3);
        }

        public void clear()
        {
            for(int y = 0; y < height; ++y)
            {
                for(int x = 0; x < width; ++x)
                {
                    field[x, y] = Line.NONE;
                }
            }
        }

    }
}
