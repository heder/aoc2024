class Program
{
    class Coord
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Id
        {
            get
            {
                return $"{X}|{Y}";
            }
        }
    }

    class Tile
    {
        public Coord Position { get; set; }

        public char Frequency { get; set; }

        public bool HasAntiFrequency { get; set; }
    }

    private static Tile[,] world;

    private static int xmax;
    private static int ymax;

    private static List<Tile> antennas = [];

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
                world[x, y] = new Tile() { Frequency = lines[y][x], Position = new Coord { X = x, Y = y } };

                if (world[x, y].Frequency != '.')
                {
                    antennas.Add(world[x, y]);
                }
            }
        }

        Dumpworld();

        // Get frequencies 
        var freqs = antennas.GroupBy(x => x.Frequency).Select(x => new { Frequency = x.Key, Count = x.Count() }).ToList();

        foreach (var f in freqs)
        {
            var antennasWithFrequency = antennas.Where(x => x.Frequency == f.Frequency).ToList();

            List<Tuple<Tile, Tile>> l = [];

            foreach (var a in antennasWithFrequency)
            {
                foreach (var b in antennasWithFrequency)
                {
                    Console.WriteLine($"{a.Position.Id} - {b.Position.Id}");

                    if (a.Position.Id == b.Position.Id)
                    {
                        continue;
                    }

                    if (l.Any(x => x.Item1.Position.Id == b.Position.Id && x.Item2.Position.Id == a.Position.Id || x.Item1.Position.Id == a.Position.Id && x.Item2.Position.Id == b.Position.Id))
                    {
                        continue;
                    }
                    else
                    {
                        l.Add(Tuple.Create(a, b));
                    }
                }
            }

            foreach (var item in l)
            {
                Console.WriteLine($"Adding to: {item.Item1.Position.Id} - {item.Item2.Position.Id}");

                var xdiff = item.Item1.Position.X - item.Item2.Position.X;
                var ydiff = item.Item1.Position.Y - item.Item2.Position.Y;

                var a1 = new Tile() { Frequency = '#', Position = new Coord { X = item.Item1.Position.X + xdiff, Y = item.Item1.Position.Y + ydiff } };
                var a2 = new Tile() { Frequency = '#', Position = new Coord { X = item.Item2.Position.X - xdiff, Y = item.Item2.Position.Y - ydiff } };

                if (a1.Position.X < 0 || a1.Position.Y < 0 || a1.Position.X >= xmax || a1.Position.Y >= ymax)
                {
                }
                else
                {
                    world[a1.Position.X, a1.Position.Y].HasAntiFrequency = true;
                }

                if (a2.Position.X < 0 || a2.Position.Y < 0 || a2.Position.X >= xmax || a2.Position.Y >= ymax)
                {
                }
                else
                {
                    world[a2.Position.X, a2.Position.Y].HasAntiFrequency = true;
                }

               // Dumpworld();
            }
        }

        int sum = 0;
        for (int y = 0; y < ymax; y++)
        {
            for (int x = 0; x < xmax; x++)
            {
                if(world[x, y].HasAntiFrequency)
                {
                    sum++;
                }
            }

            Console.WriteLine();
        }

        Console.WriteLine(sum);
        Console.ReadKey();

    }

    private static void Dumpworld()
    {
        for (int y = 0; y < ymax; y++)
        {
            for (int x = 0; x < xmax; x++)
            {
                char c = world[x, y].Frequency;

                Console.Write(c);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}