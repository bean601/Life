using System;
using System.Diagnostics;

namespace life
{
    public class Program
    {
        private readonly static int _width = Console.WindowWidth;
        private readonly static int _height = Console.WindowHeight;
        private static bool[,] _currentGeneration = new bool[_width, _height];
        private static bool[,] _nextGeneration = new bool[_width, _height];

        private class Neighborhood
        {
            public bool NW { get; set; }
            public bool N { get; set; }
            public bool NE { get; set; }
            public bool E { get; set; }
            public bool SE { get; set; }
            public bool S { get; set; }
            public bool SW { get; set; }
            public bool W { get; set; }

            public int NumberOfAliveNeighbors
            {
                get
                {
                    var aliveCount = 0;
                    if (NW) { aliveCount++; }
                    if (N) { aliveCount++; }
                    if (NE) { aliveCount++; }
                    if (E) { aliveCount++; }
                    if (SE) { aliveCount++; }
                    if (S) { aliveCount++; }
                    if (SW) { aliveCount++; }
                    if (W) { aliveCount++; }
                    return aliveCount;
                }
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("         ====== LIFE! =======");
            Console.WriteLine("         Use the arrow keys to move around");
            Console.WriteLine("         Use the space bar to toggle a live cell");
            Console.WriteLine("         Press Q to quit and being the simulation");
            Console.WriteLine("         Press any key now to begin placing live cells");

            Console.ReadLine();

            SetupCells();

            while (true)
            {
                DoLife();
                Render();
            }

            Console.Read();
        }

        private static void SetupCells()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            var keyPressed = Console.ReadKey();

            while (keyPressed.Key != ConsoleKey.Q)
            {
                keyPressed = Console.ReadKey();

                switch (keyPressed.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (Console.CursorTop > 1)
                        {
                            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (Console.CursorTop < 29)
                        {
                            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (Console.CursorLeft > 1)
                        {
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (Console.CursorLeft < 119)
                        {
                            Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                        }
                        break;
                    case ConsoleKey.Spacebar:
                        Console.CursorLeft = Console.CursorLeft - 1;
                        _currentGeneration[Console.CursorLeft, Console.CursorTop] = !_currentGeneration[Console.CursorLeft, Console.CursorTop];
                        Console.Write(_currentGeneration[Console.CursorLeft, Console.CursorTop] ? '#' : ' ');
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        break;
                    default:
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);                        
                        Console.Write(_currentGeneration[Console.CursorLeft, Console.CursorTop] ? '#' : ' ');
                        break;
                }
            }
        }

        private static void DoLife()
        {
            // Any live cell with two or three live neighbours survives.
            // Any dead cell with three live neighbours becomes a live cell.
            // All other live cells die in the next generation. Similarly, all other dead cells stay dead.

            for (int row = 0; row < _currentGeneration.GetLength(1); row++)
            {
                for (int column = 0; column < _currentGeneration.GetLength(0); column++)
                {
                    var current = _currentGeneration[column, row];
                    var neighborhood = GetNeighborhood(row, column);

                    _nextGeneration[column, row] =
                        current ? neighborhood.NumberOfAliveNeighbors == 2 || neighborhood.NumberOfAliveNeighbors == 3
                        : neighborhood.NumberOfAliveNeighbors == 3;
                }
            }

            _currentGeneration = _nextGeneration; // play god of life and death
            _nextGeneration = new bool[_width, _height];
        }

        private static void Render()
        {
            var sw = new Stopwatch();
            sw.Start();

            for (int row = 0; row < _currentGeneration.GetLength(1); row++)
            {
                for (int column = 0; column < _currentGeneration.GetLength(0); column++)
                {
                    if (!(row == 0 && column < 7))
                    {
                        Console.SetCursorPosition(column, row);
                        Console.Write(_currentGeneration[column, row] ? '#' : ' ');
                    }
                }
            }
            sw.Stop();

            var fps = (int)(((double)sw.ElapsedMilliseconds / (double)1000) * 60);
            Console.SetCursorPosition(0,0); 
            Console.Write($"FPS:{fps}");
        }

        private static Neighborhood GetNeighborhood(int row, int column)
        {
            var neighborhood = new Neighborhood();

            // logic
            // nw
            if (row - 1 > 0 && column - 1 > 0 && row - 1 < _currentGeneration.GetLength(1) && column - 1 < _currentGeneration.GetLength(0))
            {
                neighborhood.NW = _currentGeneration[column - 1, row - 1];
            }
            // n
            if (row - 1 > 0 && row - 1 < _currentGeneration.GetLength(1) && column < _currentGeneration.GetLength(0))
            {
                neighborhood.N = _currentGeneration[column, row - 1];
            }
            // ne
            if (row - 1 > 0 && row - 1 < _currentGeneration.GetLength(1) && column + 1 < _currentGeneration.GetLength(0))
            {
                neighborhood.NE = _currentGeneration[column + 1, row - 1];
            }
            // e
            if (row < _currentGeneration.GetLength(1) && column + 1 < _currentGeneration.GetLength(0))
            {
                neighborhood.E = _currentGeneration[column + 1, row];
            }
            // se
            if (row + 1 < _currentGeneration.GetLength(1) && column + 1 < _currentGeneration.GetLength(0))
            {
                neighborhood.SE = _currentGeneration[column + 1, row + 1];
            }
            // s
            if (row + 1 < _currentGeneration.GetLength(1) && column < _currentGeneration.GetLength(0))
            {
                neighborhood.S = _currentGeneration[column, row + 1];
            }
            // sw
            if (column - 1 > 0 && row + 1 < _currentGeneration.GetLength(1) && column - 1 < _currentGeneration.GetLength(0))
            {
                neighborhood.SW = _currentGeneration[column - 1, row + 1];
            }
            // w
            if (column - 1 > 0 && row < _currentGeneration.GetLength(1) && column - 1 < _currentGeneration.GetLength(0))
            {
                neighborhood.W = _currentGeneration[column - 1, row];
            }

            return neighborhood;
        }
    }
}
