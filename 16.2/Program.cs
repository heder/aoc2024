
// TWIMC: Riktigt fulhack till kod. Funkar dock. :-)

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
        BlockerLH = 2,
        BlockerRH = 3,
        Robot = 4
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

        public Tile LinkedWith { get; set; }

        public bool CheckedForMovement { get; set; } = false;
    }

    private static int n = 0;

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

        world = new Tile[xmax * 2, ymax];


        for (int y = 0; y < ymax; y++)
        {
            int wX = 0;

            for (int x = 0; x < xmax; x++)
            {
                var t1 = new Tile();
                t1.Coordinate.X = wX;
                t1.Coordinate.Y = y;

                switch (lines[y][x])
                {
                    case '.':
                        t1.Type = TileType.Empty;
                        break;
                    case '#':
                        t1.Type = TileType.Wall;
                        break;
                    case 'O':
                        t1.Type = TileType.BlockerLH;
                        break;
                    case '@':
                        t1.Type = TileType.Robot;
                        currentX = wX;
                        currentY = y;
                        break;
                }

                world[wX, y] = t1;

                wX++;

                var t2 = new Tile();
                t2.Coordinate.X = wX;
                t2.Coordinate.Y = y;

                switch (lines[y][x])
                {
                    case '.':
                        t2.Type = TileType.Empty;
                        break;
                    case '#':
                        t2.Type = TileType.Wall;
                        break;
                    case 'O':
                        t2.Type = TileType.BlockerRH;
                        break;
                    case '@':
                        t2.Type = TileType.Empty;
                        break;
                }

                world[wX, y] = t2;

                if (lines[y][x] == 'O')
                {
                    t1.LinkedWith = t2;
                    t2.LinkedWith = t1;
                }

                wX++;
            }
        }

        xmax *= 2;

        Dumpworld();

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

        Console.WriteLine($"Movements: {movements.Count}");

        foreach (var m in movements)
        {
            Console.WriteLine($"Current position: {n}");

            Console.WriteLine($"Going to move {m.ToString()}");
            Move(m);

            Dumpworld();

            n++;
        }

        int sum = 0;
        for (int y = 0; y < ymax; y++)
        {
            for (int x = 0; x < xmax; x++)
            {
                if (world[x, y].Type == TileType.BlockerLH)
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

        if (m == Direction.Up)
        {
            var t1 = world[currentX, currentY + deltaY];
            if (MoveVertical(deltaY, [t1]))
            {
                world[currentX + deltaX, currentY + deltaY].Type = TileType.Robot;
                world[currentX, currentY].Type = TileType.Empty;

                currentX += deltaX;
                currentY += deltaY;
            }

            Dumpworld();
        }
        else if (m == Direction.Down)
        {
            var t1 = world[currentX, currentY + deltaY];
            if (MoveVertical(deltaY, [t1]))
            {
                world[currentX + deltaX, currentY + deltaY].Type = TileType.Robot;
                world[currentX, currentY].Type = TileType.Empty;

                currentX += deltaX;
                currentY += deltaY;
            }
            Dumpworld();
        }
        else
        {
            // Spin to find the next empty spot in the direction of movement
            int x = currentX + deltaX;
            int y = currentY + deltaY;
            List<Tile> tomove = new();

            while (world[x, y].Type != TileType.Empty)
            {
                if (world[x, y].Type == TileType.Wall)
                {
                    tomove.Clear();
                    return;
                }

                tomove.Add(world[x, y]);
                x += deltaX;
                y += deltaY;

            }

            tomove.Reverse();

            foreach (var to in tomove)
            {
                ClearCheckmarks();

                int destX = to.Coordinate.X + deltaX;
                int destY = to.Coordinate.Y + deltaY;

                int srcX = to.Coordinate.X;
                int srcY = to.Coordinate.Y;

                world[destX, destY].Type = world[srcX, srcY].Type;
                world[destX, destY].LinkedWith = world[srcX, srcY].LinkedWith;
                world[destX, destY].LinkedWith.LinkedWith = world[destX, destY];

                world[srcX, srcY].Type = TileType.Empty;
                world[srcX, srcY].LinkedWith = null;

                ClearCheckmarks();

                Dumpworld();
            }

            world[currentX + deltaX, currentY + deltaY].Type = TileType.Robot;
            world[currentX, currentY].Type = TileType.Empty;

            currentX += deltaX;
            currentY += deltaY;

            Dumpworld();
        }
    }

    private static bool MoveVertical(int deltaY, List<Tile> canMove)
    {
        ClearCheckmarks();

        if (canMove.First().Type == TileType.Wall)
        {
            return false;
        }

        if (canMove.First().Type == TileType.Empty)
        {
            return true;
        }

        while (true)
        {
            var toCheck = canMove.Where(f => f.CheckedForMovement == false);

            if (deltaY == -1) toCheck = toCheck.OrderBy(f => f.Coordinate.X).OrderByDescending(f => f.Coordinate.Y).ToList();
            if (deltaY == 1) toCheck = toCheck.OrderBy(f => f.Coordinate.X).OrderBy(f => f.Coordinate.Y).ToList();

            foreach (var item in toCheck)
            {
                int x = item.Coordinate.X;
                int y = item.Coordinate.Y;

                if (item.CheckedForMovement)
                {
                    continue;
                }

                item.CheckedForMovement = true;

                while (true)
                {
                    // Step
                    y += deltaY;

                    if (world[x, y].Type == TileType.Empty)
                    {
                        break;
                    }

                    if (world[x, y].Type == TileType.Wall)
                    {
                        ClearCheckmarks();
                        return false;
                    }

                    if (world[x, y].Type != TileType.Empty)
                    {
                        world[x, y].CheckedForMovement = true;

                        if (canMove.Any(f => f.Coordinate.X == x && f.Coordinate.Y == y) == false)
                        {
                            canMove.Add(world[x, y]);
                        }
                    }
                }
            }

            // Get not checked friends
            var friends = canMove.Where(f => f.LinkedWith != null).Select(f => f.LinkedWith).Where(f => f.CheckedForMovement == false).ToList();

            if (friends.Count() == 0)
            {
                break;
            }
            else
            {
                canMove.AddRange(friends);
            }
        }

        if (deltaY == -1) canMove = canMove.OrderBy(f => f.Coordinate.Y).ToList();
        if (deltaY == 1) canMove = canMove.OrderByDescending(f => f.Coordinate.Y).ToList();

        foreach (var item in canMove)
        {
            int destX = item.Coordinate.X; // + deltaX;
            int destY = item.Coordinate.Y + deltaY;

            int srcX = item.Coordinate.X;
            int srcY = item.Coordinate.Y;

            world[destX, destY].Type = world[srcX, srcY].Type;

            world[destX, destY].LinkedWith = world[srcX, srcY].LinkedWith;
            world[destX, destY].LinkedWith.LinkedWith = world[destX, destY];

            world[srcX, srcY].Type = TileType.Empty;
            world[srcX, srcY].LinkedWith = null;


            Dumpworld();
        }

        Dumpworld();

        ClearCheckmarks();
        return true;
    }

    private static void ClearCheckmarks()
    {
        for (int y = 0; y < ymax; y++)
        {
            for (int x = 0; x < xmax; x++)
            {
                world[x, y].CheckedForMovement = false;
            }
        }
    }

    private static void Dumpworld()
    {
        if (n >= 25000)
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
                    case TileType.BlockerLH:
                        c = '[';
                        break;
                    case TileType.BlockerRH:
                        c = ']';
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
}