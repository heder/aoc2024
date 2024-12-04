class Program
{
    private static char[,] grid;

    private static int xmax;
    private static int ymax;

    static void Main()
    {
        var lines = File.ReadLines("in.txt").ToList();

        xmax = lines[0].Length;
        ymax = lines.Count();

        grid = new char[xmax, ymax];

        for (int y = 0; y < ymax; y++)
        {
            for (int x = 0; x < xmax; x++)
            {
                grid[x, y] = lines[y][x];
            }
        }

        DumpGrid();

        for (int y = 1; y < ymax -1; y++)
        {
            for (int x = 1; x < xmax -1; x++)
            {
                Search(x, y);
            }
        }

        Console.WriteLine(sum);
        Console.ReadKey();
    }

    private static int sum = 0;

    private static void Search(int x, int y)
    {
        if (grid[x, y] == 'A')
        {
            if (((grid[x + 1, y + 1] == 'M' && grid[x - 1, y - 1] == 'S') || (grid[x + 1, y + 1] == 'S' && grid[x - 1, y - 1] == 'M')) &&
                    ((grid[x - 1, y + 1] == 'M' && grid[x + 1, y - 1] == 'S') || (grid[x - 1, y + 1] == 'S') && (grid[x + 1, y - 1] == 'M')))
                {
                { sum++; Console.WriteLine("Found XMAS"); }
            }
        }
    }

    private static void DumpGrid()
    {
        for (int y = 0; y < ymax; y++)
        {
            for (int x = 0; x < xmax; x++)
            {
                Console.Write(grid[x, y]);
            }

            Console.WriteLine();
        }
    }
}
