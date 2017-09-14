using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SlantWPF
{
    class GameRenderer : INotifyPropertyChanged
    {
        const double FieldSize = 50d, NumRingSize = 20d;

        static readonly Brush BorderBrush = Brushes.Black, GridBrush = Brushes.Gray, LineBrush = Brushes.Black,
            NumBrush = Brushes.Black, CompleteNumBrush = Brushes.DarkGray, ErrorBrush = Brushes.Red,
            BackgroundBrush = Brushes.White, CompleteBrush = Brushes.LightGreen;

        const double BorderThickness = 1d, GridThickness = 1d,
            LineThickness = 2d, NumRingThickness = 1.5d, NumFontSize = 15d;

        //static readonly Font NumFont = new Font("Arial", 13f, FontStyle.Bold);
        
        protected readonly Canvas canvas;

        public GameRenderer(Canvas gamecanvas)
        {
            canvas = gamecanvas;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void notifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected Slant.Game game;
        protected Slant.GameChecker checker;

        protected bool isCompleted = false;
        protected RendererFieldProperty[,] prp;

        public Brush Border
        {
            get
            {
                return isCompleted ? CompleteBrush : BorderBrush;
            }
        }

        public class RendererFieldProperty : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private Slant.Line _line;
            private bool _lineerror;
            private Slant.NumberStates _number;

            public void setLine(Slant.Line line)
            {
                if (line != _line)
                {
                    _line = line;
                    notifyPropertyChanged("LineUp");
                    notifyPropertyChanged("LineDown");
                }
            }

            public void setLineError(bool lineerror)
            {
                if (lineerror != _lineerror)
                {
                    _lineerror = lineerror;
                    notifyPropertyChanged("LineError");
                }
            }

            public void setNumber(Slant.NumberStates number)
            {
                if (number != _number)
                {
                    _number = number;
                    notifyPropertyChanged("Number");
                }
            }
            private void notifyPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            public Visibility LineUp
            {
                get { return _line == Slant.Line.UP ? Visibility.Visible : Visibility.Hidden; }
            }
            public Visibility LineDown
            {
                get { return _line == Slant.Line.DOWN ? Visibility.Visible : Visibility.Hidden; }
            }

            public Brush LineError
            {
                get { return _lineerror ? ErrorBrush : LineBrush; }
            }

            public Brush Number
            {
                get
                {
                    switch (_number)
                    {
                        case Slant.NumberStates.ERROR:
                            return ErrorBrush;
                        case Slant.NumberStates.COMPLETED:
                            return CompleteNumBrush;
                        default:
                            return NumBrush;
                    }
                }
            }
        }

        public void changed(int x, int y)
        {
            prp[x, y].setLine(game[x, y]);
            changedGlobal();
        }

        public void changedAll()
        {
            for (int y = 0; y < game.Height; ++y)
            {
                for (int x = 0; x < game.Width; ++x)
                {
                    prp[x, y].setLine(game[x, y]);
                }
            }
            changedGlobal();
        }

        private void changedGlobal()
        {
            bool completed = game.CountFree == 0 && checker.isNoError();
            if(completed != isCompleted)
            {
                isCompleted = completed;
                notifyPropertyChanged("Border");
            }
            for (int y = 0; y < game.Height; ++y)
            {
                for (int x = 0; x < game.Width; ++x)
                {
                    prp[x, y].setLineError(checker[x, y]);
                }
            }
            for (int y = 0; y < game.Height + 1; ++y)
            {
                for (int x = 0; x < game.Width + 1; ++x)
                {
                    prp[x, y].setNumber(checker.NumberState[x, y]);
                }
            }
        }

        public void setGame(Slant.Game game, Slant.GameChecker checker)
        {
            this.game = game;
            this.checker = checker;

            canvas.Children.Clear();
            canvas.Width = FieldSize * game.Width + NumRingSize;
            canvas.Height = FieldSize * game.Height + NumRingSize;

            prp = new RendererFieldProperty[game.Width + 1, game.Height + 1];
            for (int y = 0; y < game.Height + 1; ++y)
            {
                for (int x = 0; x < game.Width + 1; ++x)
                {
                    prp[x, y] = new RendererFieldProperty();
                }
            }
            changedAll();

            Rectangle rect = new Rectangle()
            {
                Width = FieldSize * game.Width,
                Height = FieldSize * game.Height,
                Fill = BackgroundBrush,
                StrokeThickness = BorderThickness,
                DataContext = this
            };
            rect.SetBinding(Rectangle.StrokeProperty, "Border");
            Canvas.SetLeft(rect, NumRingSize / 2d);
            Canvas.SetTop(rect, NumRingSize / 2d);
            Canvas.SetZIndex(rect, 0);
            canvas.Children.Add(rect);

            Path grid = new Path()
            {
                Width = rect.Width,
                Height = rect.Height,
                Stroke = GridBrush,
                StrokeThickness = GridThickness
            };
            Canvas.SetLeft(grid, Canvas.GetLeft(rect));
            Canvas.SetTop(grid, Canvas.GetTop(rect));
            Canvas.SetZIndex(grid, 1);
            {
                GeometryGroup group = new GeometryGroup();
                for (int i = 1; i < game.Width; ++i)
                {
                    group.Children.Add(new LineGeometry() 
                    { 
                        StartPoint = new Point(i * FieldSize, BorderThickness), 
                        EndPoint = new Point(i * FieldSize, grid.Height-BorderThickness)
                    });
                }
                for (int i = 1; i < game.Height; ++i)
                {
                    group.Children.Add(new LineGeometry()
                    {
                        StartPoint = new Point(BorderThickness, i * FieldSize),
                        EndPoint = new Point(grid.Width-BorderThickness, i * FieldSize)
                    });
                }
                grid.Data = group;
            }
            canvas.Children.Add(grid);

            for(int y = 0; y < game.Height; ++y)
            {
                for(int x = 0; x < game.Width; ++x)
                {
                    Line down = new Line()
                    {
                        StrokeThickness = LineThickness,
                        DataContext = prp[x,y],
                        X1 = GridThickness,
                        Y1 = GridThickness,
                        X2 = FieldSize - GridThickness,
                        Y2 = FieldSize - GridThickness
                    };
                    down.SetBinding(Line.StrokeProperty, "LineError");
                    down.SetBinding(Line.VisibilityProperty, "LineDown");
                    Canvas.SetLeft(down, Canvas.GetLeft(rect) + x * FieldSize);
                    Canvas.SetTop(down, Canvas.GetTop(rect) + y * FieldSize);
                    Canvas.SetZIndex(down, 2);
                    canvas.Children.Add(down);

                    Line up = new Line()
                    {
                        StrokeThickness = LineThickness,
                        DataContext = prp[x, y],
                        X1 = FieldSize - GridThickness,
                        Y1 = GridThickness,
                        X2 = GridThickness,
                        Y2 = FieldSize - GridThickness
                    };
                    up.SetBinding(Line.StrokeProperty, "LineError");
                    up.SetBinding(Line.VisibilityProperty, "LineUp");
                    Canvas.SetLeft(up, Canvas.GetLeft(rect) + x * FieldSize);
                    Canvas.SetTop(up, Canvas.GetTop(rect) + y * FieldSize);
                    Canvas.SetZIndex(up, 2);
                    canvas.Children.Add(up);
                }
            }

            for (int y = 0; y < game.Height+1; ++y)
            {
                for (int x = 0; x < game.Width+1; ++x)
                {
                    if(game.Number[x, y] >= 0)
                    {
                        Ellipse el = new Ellipse()
                        {
                            Fill = BackgroundBrush,
                            StrokeThickness = NumRingThickness,
                            Width = NumRingSize,
                            Height = NumRingSize,
                            DataContext = prp[x, y]
                        };
                        el.SetBinding(Ellipse.StrokeProperty, "Number");
                        Canvas.SetLeft(el, Canvas.GetLeft(rect) + x * FieldSize - NumRingSize / 2d);
                        Canvas.SetTop(el, Canvas.GetTop(rect) + y * FieldSize - NumRingSize / 2d);
                        Canvas.SetZIndex(el, 3);
                        canvas.Children.Add(el);

                        TextBlock tb = new TextBlock()
                        {
                            Text = game.Number[x, y].ToString(),
                            Width = NumRingSize,
                            Height = NumRingSize,
                            FontSize = NumFontSize,
                            FontWeight = FontWeights.Bold,
                            TextAlignment = TextAlignment.Center,
                            DataContext = prp[x, y]
                        };
                        tb.SetBinding(TextBlock.ForegroundProperty, "Number");
                        Canvas.SetLeft(tb, Canvas.GetLeft(el));
                        Canvas.SetTop(tb, Canvas.GetTop(el));
                        Canvas.SetZIndex(tb, 3);
                        canvas.Children.Add(tb);
                    }
                }
            }
        }
    }
}
