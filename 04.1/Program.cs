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

        for (int y = 0; y < ymax; y++)
        {
            for (int x = 0; x < xmax; x++)
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
        if (x < xmax - 3 && grid[x, y] == 'X' && grid[x + 1, y] == 'M' && grid[x + 2, y] == 'A' && grid[x + 3, y] == 'S') { sum++; Console.WriteLine("Found XMAS"); }
        if (x > 2 && grid[x, y] == 'X' && grid[x - 1, y] == 'M' && grid[x - 2, y] == 'A' && grid[x - 3, y] == 'S') { sum++; Console.WriteLine("Found XMAS"); }
        if (y < ymax - 3 && grid[x, y] == 'X' && grid[x, y + 1] == 'M' && grid[x, y + 2] == 'A' && grid[x, y + 3] == 'S') { sum++; Console.WriteLine("Found XMAS"); }
        if (y > 2 && grid[x, y] == 'X' && grid[x, y - 1] == 'M' && grid[x, y - 2] == 'A' && grid[x, y - 3] == 'S') { sum++; Console.WriteLine("Found XMAS"); }

        if (x < xmax - 3 && y < ymax - 3 && grid[x, y] == 'X' && grid[x + 1, y + 1] == 'M' && grid[x + 2, y + 2] == 'A' && grid[x + 3, y + 3] == 'S') { sum++; Console.WriteLine("Found XMAS"); }
        if (x > 2 && y > 2 && grid[x, y] == 'X' && grid[x - 1, y - 1] == 'M' && grid[x - 2, y - 2] == 'A' && grid[x - 3, y - 3] == 'S') { sum++; Console.WriteLine("Found XMAS"); }
        if (x < xmax - 3 && y > 2 && grid[x, y] == 'X' && grid[x + 1, y - 1] == 'M' && grid[x + 2, y - 2] == 'A' && grid[x + 3, y - 3] == 'S') { sum++; Console.WriteLine("Found XMAS"); }
        if (x > 2 && y < ymax - 3 && grid[x, y] == 'X' && grid[x - 1, y + 1] == 'M' && grid[x - 2, y + 2] == 'A' && grid[x - 3, y + 3] == 'S') { sum++; Console.WriteLine("Found XMAS"); }
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
