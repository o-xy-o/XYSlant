using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace SlantWPF.UI
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int FieldSize = 50, NumRingSize = 20, SolverRounds = 50;

        private Slant.Game _game;
        private Slant.GameChecker checker;
        private readonly Random rand;

        public Slant.Game Game
        {
            get { return _game; }
            set
            {
                _game = value;
                checker = new Slant.GameChecker(value);
                checker.checkAllNumbers();
                renderer.setGame(value, checker);
            }
        }

        BackgroundWorker solveWorker, createWorker;
        GameRenderer renderer;

        public MainWindow()
        {
            InitializeComponent();
            renderer = new GameRenderer(gamePanel);
            solveWorker = new BackgroundWorker();
            solveWorker.DoWork += solveWorker_DoWork;
            solveWorker.RunWorkerCompleted += solveWorker_RunWorkerCompleted;
            createWorker = new BackgroundWorker();
            createWorker.DoWork += createWorker_DoWork;
            createWorker.RunWorkerCompleted += createWorker_RunWorkerCompleted;

            rand = new Random();
            Game = new Slant.Game(8, 8);
        }

        private void menuNew_Click(object sender, RoutedEventArgs e)
        {
            Game.clear();
            checker.clear();
            renderer.changedAll();
        }

        private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "Slant Dateien|*.slant"
            };
            if ((bool)dlg.ShowDialog(this) && dlg.FileName != "")
            {
                try
                {
                    string[] lines = File.ReadAllLines(dlg.FileName);

                    if (lines.Length == 0 || lines[0].Length == 0)
                        throw new ArgumentNullException();

                    int width = lines[0].Length, height = 1;
                    while (height < lines.Length && lines[height].Length != 0) ++height;
                    if (width > 21 || height > 21)
                        throw new ArgumentOutOfRangeException();

                    Slant.Line[,] field = null;
                    int[,] numbers = new int[width, height];
                    
                    for (int y = 0; y < height; ++y)
                    {
                        if (lines[y].Length != width)
                            throw new ArgumentException();
                        for (int x = 0; x < width; ++x)
                        {
                            char c = lines[y][x];
                            numbers[x, y] = (c >= '0' && c <= '4') ? c - '0' : -1;
                        }
                    }

                    if(height < lines.Length)
                    {
                        if (lines.Length != 2 * height)
                            throw new ArgumentException();
                        --width; --height;
                        field = new Slant.Line[width, height];
                        int ys = lines.Length - height;
                        for (int y = 0; y < height; ++y)
                        {
                            if (lines[ys + y].Length != width)
                                throw new ArgumentException();
                            for (int x = 0; x < width; ++x)
                            {
                                switch (lines[ys + y][x])
                                {
                                    case '\\':
                                        field[x, y] = Slant.Line.DOWN;
                                        break;
                                    case '/':
                                        field[x, y] = Slant.Line.UP;
                                        break;
                                    default:
                                        field[x, y] = Slant.Line.NONE;
                                        break;
                                }
                            }
                        }
                    }

                    Game = new Slant.Game(numbers, field);
                }
                catch (ArgumentNullException)
                {
                    MessageBox.Show("Die Datei oder die erste Zeile ist leer!", Title,
                        MessageBoxButton.OK, MessageBoxImage.None);
                }
                catch (ArgumentOutOfRangeException)
                {
                    MessageBox.Show("Die Datei ist zu groß (max. 20x20)!", Title,
                        MessageBoxButton.OK, MessageBoxImage.None);
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Die Datei hat nicht das richtige Format!", Title,
                        MessageBoxButton.OK, MessageBoxImage.None);
                }
                catch (Exception)
                {
                    MessageBox.Show("Die Datei konnte nicht gelesen werden!", Title,
                        MessageBoxButton.OK, MessageBoxImage.None);
                }
            }
        }

        private void menuSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog()
            {
                Filter = "Slant Dateien|*.slant",
                OverwritePrompt = true
            };
            if ((bool)dlg.ShowDialog(this) && dlg.FileName != "")
            {
                try
                {
                    using (StreamWriter file = new StreamWriter(dlg.FileName))
                    {
                        for (int y = 0; y < Game.Height + 1; ++y)
                        {
                            for (int x = 0; x < Game.Width + 1; ++x)
                            {
                                if (Game.Number[x, y] < 0) file.Write('-');
                                else file.Write(Game.Number[x, y]);
                            }
                            file.WriteLine();
                        }
                        for (int y = 0; y < Game.Height; ++y)
                        {
                            file.WriteLine();
                            for (int x = 0; x < Game.Width; ++x)
                            {
                                switch (Game[x, y])
                                {
                                    case Slant.Line.DOWN:
                                        file.Write('\\');
                                        break;
                                    case Slant.Line.UP:
                                        file.Write('/');
                                        break;
                                    default:
                                        file.Write('-');
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Die Datei konnte nicht gespeichert werden!", Title,
                        MessageBoxButton.OK, MessageBoxImage.None);
                }
            }
        }

        private void menuSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsDialog dlg = new SettingsDialog()
            {
                NumericWidth = Game.Width,
                NumericHeight = Game.Height,
                Owner = Window.GetWindow(this)
            };

            if ((bool)dlg.ShowDialog())
            {
                Game = new Slant.Game(dlg.NumericWidth, dlg.NumericHeight);
            }
        }

        private void menuQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void menuSolveEasy_Click(object sender, RoutedEventArgs e)
        {
            if (!solveWorker.IsBusy)
                solveWorker.RunWorkerAsync(new Slant.EasyPatternSolver(Game));
        }

        private void menuSolveMedium_Click(object sender, RoutedEventArgs e)
        {
            if (!solveWorker.IsBusy)
                solveWorker.RunWorkerAsync(new Slant.MediumPatternSolver(Game));
        }

        private void menuSolveHard_Click(object sender, RoutedEventArgs e)
        {
            if (!solveWorker.IsBusy)
                solveWorker.RunWorkerAsync(new Slant.HardPatternSolver(Game));
        }

        private void menuSolveExtreme_Click(object sender, RoutedEventArgs e)
        {
            if (!solveWorker.IsBusy)
                solveWorker.RunWorkerAsync(new Slant.ExtremePatternSolver(Game));
        }

        private void menuSolveBacktracking_Click(object sender, RoutedEventArgs e)
        {
            if (!solveWorker.IsBusy)
                solveWorker.RunWorkerAsync(new Slant.BacktrackingSolver(Game));
        }

        private void menuHelpEasy_Click(object sender, RoutedEventArgs e)
        {
            if (!solveWorker.IsBusy)
                solveWorker.RunWorkerAsync(new Slant.EasyHelper(Game));
        }

        private void menuHelpMedium_Click(object sender, RoutedEventArgs e)
        {
            if (!solveWorker.IsBusy)
                solveWorker.RunWorkerAsync(new Slant.MediumHelper(Game));
        }

        private void menuHelpHard_Click(object sender, RoutedEventArgs e)
        {
            if (!solveWorker.IsBusy)
                solveWorker.RunWorkerAsync(new Slant.HardHelper(Game));
        }

        private void menuHelpExtreme_Click(object sender, RoutedEventArgs e)
        {
            if (!solveWorker.IsBusy)
                solveWorker.RunWorkerAsync(new Slant.ExtremeHelper(Game));
        }

        private void menuHelpHint_Click(object sender, RoutedEventArgs e)
        {
            if (!solveWorker.IsBusy)
                solveWorker.RunWorkerAsync(new Slant.HintHelper(Game, rand));
        }

        private void solveWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Slant.Solver solver = (Slant.Solver)e.Argument;
            int rnd = solver.solve();
            sw.Stop();
            Console.WriteLine("Solve: " + rnd + " (" + sw.Elapsed + ")");
            e.Result = solver;
        }

        private void solveWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Game = (Slant.Game)e.Result;
        }

        private void menuCreateEasy_Click(object sender, RoutedEventArgs e)
        {
            if (!createWorker.IsBusy)
                createWorker.RunWorkerAsync(Slant.Difficulty.EASY);
        }

        private void menuCreateMedium_Click(object sender, RoutedEventArgs e)
        {
            if (!createWorker.IsBusy)
                createWorker.RunWorkerAsync(Slant.Difficulty.MEDIUM);
        }

        private void menuCreateHard_Click(object sender, RoutedEventArgs e)
        {
            if (!createWorker.IsBusy)
                createWorker.RunWorkerAsync(Slant.Difficulty.HARD);
        }

        private void menuCreateExtreme_Click(object sender, RoutedEventArgs e)
        {
            if (!createWorker.IsBusy)
                createWorker.RunWorkerAsync(Slant.Difficulty.EXTREME);
        }

        private void createWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Slant.Creator creator = new Slant.Creator(Game.Width, Game.Height);
            int trs = creator.generate(rand, 10);
            int rnd = creator.eliminate(rand, (Slant.Difficulty)e.Argument, SolverRounds);
            creator.clear();
            sw.Stop();
            Console.WriteLine("Create: " + trs + " | " + rnd + " (" + sw.Elapsed + ")");
            e.Result = creator;
        }

        private void menuHelp_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new HelpWindow()
            {
                Owner = Window.GetWindow(this)
            };
            dlg.Show();
        }

        private void menuAbout_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AboutWindow()
            {
                Owner = Window.GetWindow(this)
            };
            dlg.ShowDialog();
        }

        private void createWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Game = (Slant.Game)e.Result;
        }

        private void gamePanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int cnt;
            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    cnt = 1;
                    break;
                case MouseButton.Right:
                    cnt = -1;
                    break;
                default:
                    return;
            }
            Point pt = e.GetPosition(gamePanel);
            pt.Offset(-NumRingSize / 2d, -NumRingSize / 2d);
            if (pt.X <= 0 || 
                pt.Y <= 0 || 
                pt.X >= Game.Width * FieldSize || 
                pt.Y >= Game.Height * FieldSize) return;
            int x = (int)pt.X / FieldSize, y = (int)pt.Y / FieldSize;
            checker.removeLineError(x, y);
            Game.toggle(x, y, cnt);
            checker.checkError(x, y);
            renderer.changed(x, y);

            if(Game.CountFree == 0 && checker.isNoError())
            {
                gamePanel.IsEnabled = false;
                Storyboard sbd = (Storyboard)FindResource("winAnimation");
                sbd.Begin(this);
            }
        }

        private void winAnimation_Completed(object sender, EventArgs e)
        {
            gamePanel.IsEnabled = true;
        }
    }
}
