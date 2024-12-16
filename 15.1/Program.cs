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
        Reindeer = 3,
        End = 4
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
        public int Score { get; set; } = -1;
    }

    private static Tile[,] world;

    private static int xmax;
    private static int ymax;

    private static int currentX = 0;
    private static int currentY = 0;
    private static Direction currentDirection = Direction.Right;
    private static int currentScore = 0;

    private static List<Direction> movements = [];

    static void Main()
    {
        var lines = File.ReadLines("in.txt").ToArray();

        xmax = lines[0].Length;
        ymax = lines.Count();

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
                    case 'S':
                        t.Type = TileType.Reindeer;
                        currentX = x;
                        currentY = y;
                        break;
                    case 'E':
                        t.Type = TileType.End;
                        break;
                }

                world[x, y] = t;
            }
        }

        Dumpworld();

        world[currentX, currentY].Score = currentScore; // 0;

        Traverse();
        
        Console.WriteLine();
        Console.ReadKey();


        void Traverse()
        {
            while 

            var directions = GetFreeDirections();

            foreach (var item in directions)
            {
                Move(item);
                Dumpworld();
                Console.WriteLine(currentScore);
            }
        }


        void Move(Direction d)
        {
            // New direction, turn first
            if (d != currentDirection)
            {
                currentScore += 1000;
                world[currentX, currentY].Score = currentScore;
                currentDirection = d;
            }

            world[currentX, currentY].Type = TileType.Empty;

            switch (currentDirection)
            {
                case Direction.Up:
                    currentY--;
                    break;
                case Direction.Down:
                    currentY++;
                    break;
                case Direction.Left:
                    currentX--;
                    break;
                case Direction.Right:
                    currentX++;
                    break;
            }

            world[currentX, currentY].Type = TileType.Reindeer;

            currentScore += 1;
            world[currentX, currentY].Score = currentScore;
        }



        List<Direction> GetFreeDirections()
        {
            var t = new List<Direction>();

            if (world[currentX - 1, currentY].Type == TileType.Empty)
            {
                t.Add(Direction.Left);
            }

            if (world[currentX + 1, currentY].Type == TileType.Empty)
            {
                t.Add(Direction.Right);
            }

            if (world[currentX, currentY - 1].Type == TileType.Empty)
            {
                t.Add(Direction.Up);
            }

            if (world[currentX, currentY + 1].Type == TileType.Empty)
            {
                t.Add(Direction.Down);
            }

            return t;
        }


    }


    //for (int i = ymax + 1; i < lines.Length; i++)
    //{
    //    foreach (var c in lines[i])
    //    {
    //        Direction movement;

    //        switch (c)
    //        {
    //            case '^':
    //                movement = Direction.Up;
    //                break;
    //            case 'v':
    //                movement = Direction.Down;
    //                break;
    //            case '<':
    //                movement = Direction.Left;
    //                break;
    //            case '>':
    //                movement = Direction.Right;
    //                break;
    //            default:
    //                throw new Exception("Invalid movement");
    //        }

    //        movements.Add(movement);
    //    }
    //}




    //private static void Move(Direction m)
    //{
    //    int deltaX;
    //    int deltaY;

    //    switch (m)
    //    {
    //        case Direction.Up:
    //            deltaX = 0;
    //            deltaY = -1;
    //            break;
    //        case Direction.Down:
    //            deltaX = 0;
    //            deltaY = 1;
    //            break;
    //        case Direction.Left:
    //            deltaX = -1;
    //            deltaY = 0;
    //            break;
    //        case Direction.Right:
    //            deltaX = 1;
    //            deltaY = 0;
    //            break;
    //        default:
    //            throw new Exception("Invalid movement");
    //    }

    //    // Spin to find the next empty spot in the direction of movement
    //    int x = currentX + deltaX;
    //    int y = currentY + deltaY;
    //    List<Tile> tomove = new();

    //    while (world[x, y].Type != TileType.Empty)
    //    {
    //        if (x < 0 || x >= xmax || y < 0 || y >= ymax || world[x, y].Type == TileType.Wall)
    //        {
    //            // Movement not possible
    //            tomove.Clear();
    //            return;
    //        }

    //        tomove.Add(world[x, y]);
    //        x += deltaX;
    //        y += deltaY;
    //    }

    //    tomove.Reverse();

    //    foreach (var t in tomove)
    //    {
    //        int destX = t.Coordinate.X + deltaX;
    //        int destY = t.Coordinate.Y + deltaY;

    //        int srcX = t.Coordinate.X;
    //        int srcY = t.Coordinate.Y;

    //        world[destX, destY].Type = TileType.Blocker;
    //        world[srcX, srcY].Type = TileType.Empty;

    //    }

    //    world[currentX + deltaX, currentY + deltaY].Type = TileType.Robot;
    //    world[currentX, currentY].Type = TileType.Empty;

    //    currentX += deltaX;
    //    currentY += deltaY;
    //}

    private static void Dumpworld()
    {
        for (int y = 0; y < ymax; y++)
        {
            for (int x = 0; x < xmax; x++)
            {
                char c;

                //if (world[x, y].Score > -1)
                //{
                //    c = world[x, y].Score.ToString();
                //}
                //else
                //{

                    switch (world[x, y].Type)
                    {
                        case TileType.Empty:
                            c = '.';
                            break;
                        case TileType.Wall:
                            c = '#';
                            break;
                        case TileType.Reindeer:
                            c = 'S';
                            break;
                        case TileType.End:
                            c = 'E';
                            break;
                        default:
                            c = 'X';
                            break;
                    }
                //}

                Console.Write(c);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}