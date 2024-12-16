class Program
{
    public enum Direction
    {
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    public enum TileType
    {
        Empty = 0,
        Wall = 1,
        Blocker = 2,
        Robot = 3
    }

    class Coord
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    class Tile
    {
        public Coord Coordinate { get; set; } = new();
        public TileType Type { get; set; }

    }

    private static Tile[,] world;

    private static int xmax;
    private static int ymax;

    private static int currentX = 0;
    private static int currentY = 0;

    private static List<Direction> movements = [];

    static void Main()
    {
        var lines = File.ReadLines("in.txt").ToArray();

        xmax = lines[0].Length;
        ymax = lines.Count();

        for (int i = 0; i < lines.Count(); i++)
        {
            if (lines[i].Trim().Length == 0)
            {
                ymax = i;
                break;
            }
        }

        world = new Tile[xmax, ymax];

        for (int y = 0; y < ymax; y++)
        {
            for (int x = 0; x < xmax; x++)
            {
                var t = new Tile();

                t.Coordinate.X = x;
                t.Coordinate.Y = y;

                switch (lines[y][x])
                {
                    case '.':
                        t.Type = TileType.Empty;
                        break;
                    case '#':
                        t.Type = TileType.Wall;
                        break;
                    case 'O':
                        t.Type = TileType.Blocker;
                        break;
                    case '@':
                        t.Type = TileType.Robot;
                        currentX = x;
                        currentY = y;
                        break;
                }

                world[x, y] = t;
            }
        }

        for (int i = ymax + 1; i < lines.Length; i++)
        {
            foreach (var c in lines[i])
            {
                Direction movement;

                switch (c)
                {
                    case '^':
                        movement = Direction.Up;
                        break;
                    case 'v':
                        movement = Direction.Down;
                        break;
                    case '<':
                        movement = Direction.Left;
                        break;
                    case '>':
                        movement = Direction.Right;
                        break;
                    default:
                        throw new Exception("Invalid movement");
                }

                movements.Add(movement);
            }
        }

        Dumpworld();

        foreach (var m in movements)
        {
            Console.WriteLine($"Going to move {m.ToString()}");
            Move(m);
        }

        int sum = 0;
        for (int y = 0; y < ymax; y++)
        {
            for (int x = 0; x < xmax; x++)
            {
                if (world[x, y].Type == TileType.Blocker)
                {
                    sum += (100 * y) + x;
                }
            }
        }

        Console.WriteLine(sum);
        Console.ReadKey();
    }

    private static void Move(Direction m)
    {
        int deltaX;
        int deltaY;

        switch (m)
        {
            case Direction.Up:
                deltaX = 0;
                deltaY = -1;
                break;
            case Direction.Down:
                deltaX = 0;
                deltaY = 1;
                break;
            case Direction.Left:
                deltaX = -1;
                deltaY = 0;
                break;
            case Direction.Right:
                deltaX = 1;
                deltaY = 0;
                break;
            default:
                throw new Exception("Invalid movement");
        }

        // Spin to find the next empty spot in the direction of movement
        int x = currentX + deltaX;
        int y = currentY + deltaY;
        List<Tile> tomove = new();

        while (world[x, y].Type != TileType.Empty)
        {
            if (x < 0 || x >= xmax || y < 0 || y >= ymax || world[x, y].Type == TileType.Wall)
            {
                // Movement not possible
                tomove.Clear();
                return;
            }

            tomove.Add(world[x, y]);
            x += deltaX;
            y += deltaY;
        }

        tomove.Reverse();

        foreach (var t in tomove)
        {
            int destX = t.Coordinate.X + deltaX;
            int destY = t.Coordinate.Y + deltaY;

            int srcX = t.Coordinate.X;
            int srcY = t.Coordinate.Y;

            world[destX, destY].Type = TileType.Blocker;
            world[srcX, srcY].Type = TileType.Empty;

        }

        world[currentX + deltaX, currentY + deltaY].Type = TileType.Robot;
        world[currentX, currentY].Type = TileType.Empty;

        currentX += deltaX;
        currentY += deltaY;
    }

    private static void Dumpworld()
    {
        for (int y = 0; y < ymax; y++)
        {
            for (int x = 0; x < xmax; x++)
            {
                char c;
                switch (world[x, y].Type)
                {
                    case TileType.Empty:
                        c = '.';
                        break;
                    case TileType.Wall:
                        c = '#';
                        break;
                    case TileType.Blocker:
                        c = 'O';
                        break;
                    case TileType.Robot:
                        c = '@';
                        break;
                    default:
                        c = 'X';
                        break;
                }

                Console.Write(c);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}