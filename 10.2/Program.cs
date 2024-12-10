class Program
{
    class Coord
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    class Tile
    {
        public bool Visited { get; set; }
        public int? Height { get; set; }
        public Coord Position { get; set; }
        public int Score { get; set; }
    }

    private static Tile[,] world;

    private static int xmax;
    private static int ymax;

    private static List<Tile> trailHeads = [];

    static void Main()
    {
        var lines = File.ReadLines("in.txt").ToList();

        xmax = lines[0].Length;
        ymax = lines.Count();

        world = new Tile[xmax, ymax];

        for (int y = 0; y < ymax; y++)
        {
            for (int x = 0; x < xmax; x++)
            {
                Tile t;
                if (lines[y][x].ToString() != ".")
                {
                    t = new Tile() { Visited = false, Position = new Coord() { X = x, Y = y }, Height = Convert.ToInt32(lines[y][x].ToString()) };
                }
                else
                {
                    t = new Tile() { Visited = false, Position = new Coord() { X = x, Y = y }, Height = null };
                }

                if (t.Height == 0)
                {
                    trailHeads.Add(t);
                }

                world[x, y] = t;
            }
        }

        Dumpworld();

        foreach (var item in trailHeads)
        {
            for (int y = 0; y < ymax; y++)
            {
                for (int x = 0; x < xmax; x++)
                {
                    world[x, y].Visited = false;
                }
            }

            DfsTile(item);
        }

        // Find all tiles who has a score > 0
        var result = world.Cast<Tile>().Where(x => x.Score > 0).ToList();
        var sum = result.Sum(f => f.Score);

        Console.WriteLine(sum);
        Console.ReadKey();


        void DfsTile(Tile item)
        {
            item.Visited = true;

            // Dumpworld();

            if (item.Height == 9)
            {
                world[item.Position.X, item.Position.Y].Score++;
                return;
            }

            List<Tile> candidates = GetCandidates(item);

            // Foreach neighbour that is higher and not outside world
            foreach (var c in candidates)
            {
                DfsTile(c);
            }

            List<Tile> GetCandidates(Tile t)
            {
                var candidates = new List<Tile>();

                int x;
                int y;

                x = t.Position.X - 1;
                y = t.Position.Y;

                CheckedAdd();

                x = t.Position.X + 1;
                y = t.Position.Y;

                CheckedAdd();

                x = t.Position.X;
                y = t.Position.Y + 1;

                CheckedAdd();

                x = t.Position.X;
                y = t.Position.Y - 1;

                CheckedAdd();

                return candidates;


                void CheckedAdd()
                {
                    if (x < xmax && x >= 0 && y < ymax && y >= 0 && world[t.Position.X, t.Position.Y].Height + 1 == world[x, y].Height)
                    {
                        candidates.Add(world[x, y]);
                    }
                }
            }
        }
    }


    private static void Dumpworld()
    {
        for (int y = 0; y < ymax; y++)
        {
            for (int x = 0; x < xmax; x++)
            {
                char c = '.';
                if (world[x, y].Height != null)
                {
                    c = world[x, y].Height.ToString()[0];
                }

                if (world[x, y].Visited)
                {
                    c = 'X';
                }

                if (world[x, y].Height == null)
                {
                    c = '.';
                }

                Console.Write(c);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}