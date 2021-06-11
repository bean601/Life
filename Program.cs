﻿using System;
using System.Diagnostics;
using System.Threading;

namespace life
{
    public class Program
    {
        private static bool[,] _currentGeneration = new bool[Console.WindowWidth, Console.WindowHeight];
        private static bool[,] _nextGeneration = new bool[Console.WindowWidth, Console.WindowHeight];

        public class Cell
        {
            public bool IsValid { get; set; }
            public bool IsAlive { get; set; }
        }

        private class Neighborhood
        {
            public Neighborhood()
            {
                NW = new Cell();
                N = new Cell();
                NE = new Cell();
                E = new Cell();
                SE = new Cell();
                S = new Cell();
                SW = new Cell();
                W = new Cell();
                Current = new Cell();
            }

            public Cell NW { get; set; }
            public Cell N { get; set; }
            public Cell NE { get; set; }
            public Cell E { get; set; }
            public Cell SE { get; set; }
            public Cell S { get; set; }
            public Cell SW { get; set; }
            public Cell W { get; set; }
            public Cell Current { get; set; }

            public int NumberOfAliveNieghbords
            {
                get
                {
                    var aliveCount = 0;
                    if (NW.IsAlive) { aliveCount++; }
                    if (N.IsAlive) { aliveCount++; }
                    if (NE.IsAlive) { aliveCount++; }
                    if (E.IsAlive) { aliveCount++; }
                    if (SE.IsAlive) { aliveCount++; }
                    if (S.IsAlive) { aliveCount++; }
                    if (SW.IsAlive) { aliveCount++; }
                    if (W.IsAlive) { aliveCount++; }
                    return aliveCount;
                }
            }
        }

        static void Main(string[] args)
        {
            //Console.WriteLine(board.GetLength(0));
            //Console.WriteLine(board.GetLength(1));
            
            while (true)
            {
                DoLife();
                Render();
            }

            Console.Read();
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
                    var neighborhood = GetNeighborhood(row, column);
                    neighborhood.Current.IsAlive = neighborhood.NumberOfAliveNieghbords > (neighborhood.Current.IsAlive ? 1 : 2);

                    _nextGeneration[column, row] = neighborhood.Current.IsAlive;
                }
            }

            _currentGeneration = _nextGeneration;
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
                        Console.Write('#');
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
            if (row - 1 > 0 && column - 1 > 0 && row - 1 >= _currentGeneration.GetLength(1) && column - 1 >= _currentGeneration.GetLength(0))
            {
                neighborhood.NW.IsValid = true;
                neighborhood.NW.IsAlive = _currentGeneration[column - 1, row - 1];
            }
            // n
            if (row - 1 > 0 && row - 1 >= _currentGeneration.GetLength(1) && column >= _currentGeneration.GetLength(0))
            {
                neighborhood.N.IsValid = true;
                neighborhood.N.IsAlive = _currentGeneration[column, row - 1];
            }
            // ne
            if (row + 1 < _currentGeneration.GetLength(1) && column + 1 < _currentGeneration.GetLength(0))
            {
                neighborhood.NE.IsValid = true;
                neighborhood.NE.IsAlive = _currentGeneration[column + 1, row + 1];
            }
            // e
            if (row < _currentGeneration.GetLength(1) && column + 1 < _currentGeneration.GetLength(0))
            {
                neighborhood.E.IsValid = true;
                neighborhood.E.IsAlive = _currentGeneration[column + 1, row];
            }
            // se
            if (row + 1 < _currentGeneration.GetLength(1) && column + 1 < _currentGeneration.GetLength(0))
            {
                neighborhood.SE.IsValid = true;
                neighborhood.SE.IsAlive = _currentGeneration[column + 1, row + 1];
            }
            // s
            if (row <= _currentGeneration.GetLength(1) && column + 1 < _currentGeneration.GetLength(0))
            {
                neighborhood.S.IsValid = true;
                neighborhood.S.IsAlive = _currentGeneration[column + 1, row];
            }
            // sw
            if (column - 1 > 0 && row - 1 > 0 && row + 1 <= _currentGeneration.GetLength(1) && column - 1 <= _currentGeneration.GetLength(0))
            {
                neighborhood.SW.IsValid = true;
                neighborhood.SW.IsAlive = _currentGeneration[column - 1, row - 1];
            }
            // w
            if (column - 1 > 0 && row <= _currentGeneration.GetLength(1) && column - 1 <= _currentGeneration.GetLength(0))
            {
                neighborhood.W.IsValid = true;
                neighborhood.W.IsAlive = _currentGeneration[column - 1, row];
            }

            neighborhood.Current.IsValid = true;
            neighborhood.Current.IsAlive = _currentGeneration[column, row];

            return neighborhood;
        }
    }
}