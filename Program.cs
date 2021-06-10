using System;
using System.Diagnostics;
using System.Threading;

namespace life
{
    public class Program
    {
        private static bool[,] board = new bool[Console.WindowWidth, Console.WindowHeight];

        static void Main(string[] args)
        {
           // Console.WriteLine("Hello World!");
            Console.WriteLine(board.Length);

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


  

        }

        private static void Render()
        {
            var sw = new Stopwatch();
            sw.Start();

            for (int j = 0; j < Console.WindowHeight; j++)
            {
                for (int i = 0; i < Console.WindowWidth; i++)
                {
                    if (!(j == 0 ))//&& i < 7))
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write('#');
                    }
                }
            }
            sw.Stop();

            var fps = ((decimal)sw.ElapsedMilliseconds / (decimal)1000)*60;
            Console.SetCursorPosition(0,0); 
            Console.Write($"FPS:{fps}");
        }
    }
}
