class Program
{
    public enum Direction
    {
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    class Coord
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    class Tile
    {
        private bool _blocked;
        public bool Blocked
        {
            get
            {
                return _blocked || BlockerCandidate;
            }
            set
            {
                _blocked = value;
            }
        }
        public bool BlockerCandidate { get; set; } = false;
        public bool Visited { get; set; } = false;

        public bool VisitedOnCleanRun { get; set; } = false;

        public List<Direction> VisitedDirection { get; set; } = [];
    }

    private static Tile[,] world;

    private static int xmax;
    private static int ymax;

    private static int startingX = 0;
    private static int startingY = 0;

    private static Direction CurrentDirection = Direction.Up;
    private static int currentX = 0;
    private static int currentY = 0;

    private static int currentBlockerCandidateX = 0;
    private static int currentBlockerCandidateY = 0;

    private static List<Coord> blockers = [];

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
                world[x, y] = new Tile();

                if (lines[y][x] == '^')
                {
                    startingX = x;
                    startingY = y;

                    currentX = x;
                    currentY = y;
                    world[x, y].Visited = true;
                }
                else if (lines[y][x] == '#')
                {
                    world[x, y].Blocked = true;
                }
            }
        }

        Dumpworld();

        // Clean run 
        while (true)
        {
            if (currentX >= xmax - 1 || currentX <= 0 || currentY <= 0 || currentY >= ymax - 1)
            {
                ResetWorld();
                goto ccc;
            }

            if (Move(true))
            {
                goto ccc;
            }
        }

    ccc:;

        Dumpworld();

        for (int y = 0; y < ymax; y++)
        {
            Console.WriteLine(y);

            for (int x = 0; x < xmax; x++)
            {
                // If node not in initial path, not visited on clean run, and not a blocker candidate, skip
                if (world[x, y].VisitedOnCleanRun == false) continue;

                SetBlockerCandidate(x, y);

                while (true)
                {
                    if (currentX >= xmax - 1 || currentX <= 0 || currentY <= 0 || currentY >= ymax - 1)
                    {
                        ResetWorld();
                        goto runnext;
                    }

                    if (Move(false))
                    {
                        goto runnext;
                    }
                }

            runnext:;
            }
        }

        Console.WriteLine(blockers.Count);
        Console.ReadKey();
    }

    private static void ResetWorld()
    {
        for (int y = 0; y < ymax; y++)
        {
            for (int x = 0; x < xmax; x++)
            {
                world[x, y].Visited = false;
                world[x, y].VisitedDirection.Clear();
                world[x, y].BlockerCandidate = false;
            }
        }

        CurrentDirection = Direction.Up;
        currentX = startingX;
        currentY = startingY;
    }

    private static void SetBlockerCandidate(int x, int y)
    {
        currentBlockerCandidateX = x;
        currentBlockerCandidateY = y;
        world[currentBlockerCandidateX, currentBlockerCandidateY].BlockerCandidate = true;
    }

    private static bool Move(bool clean)
    {
        if (CurrentDirection == Direction.Up)
        {
            if (world[currentX, currentY - 1].Blocked == true)
            {
                CurrentDirection = Direction.Right;
            }
            else
            {
                currentY--;
            }
        }
        else if (CurrentDirection == Direction.Down)
        {
            if (world[currentX, currentY + 1].Blocked == true)
            {
                CurrentDirection = Direction.Left;
            }
            else
            {
                currentY++;
            }
        }
        else if (CurrentDirection == Direction.Left)
        {
            if (world[currentX - 1, currentY].Blocked == true)
            {
                CurrentDirection = Direction.Up;
            }
            else
            {
                currentX--;
            }
        }
        else if (CurrentDirection == Direction.Right)
        {
            if (world[currentX + 1, currentY].Blocked == true)
            {
                CurrentDirection = Direction.Down;
            }
            else
            {
                currentX++;
            }
        }

        // Loop detection
        if (world[currentX, currentY].Visited == true &&  world[currentX, currentY].VisitedDirection.Contains(CurrentDirection))
        {
            blockers.Add(new Coord() { X = currentX, Y = currentY });
            ResetWorld();
            return true;
        }

        if (clean)
        {
            world[currentX, currentY].VisitedOnCleanRun = true;
        }
        else
        {
            world[currentX, currentY].Visited = true;
        }

        world[currentX, currentY].VisitedDirection.Add(CurrentDirection);

        return false;
    }

    private static void Dumpworld()
    {
        for (int y = 0; y < ymax; y++)
        {
            for (int x = 0; x < xmax; x++)
            {
                char c;

                if (x == currentX && y == currentY)
                {
                    switch (CurrentDirection)
                    {
                        case Direction.Up:
                            c = '^';
                            break;
                        case Direction.Down:
                            c = 'v';
                            break;
                        case Direction.Left:
                            c = '<';
                            break;
                        case Direction.Right:
                            c = '>';
                            break;
                        default:
                            c = '?';
                            break;
                    }
                }
                else if (world[x, y].Visited)
                {
                    c = 'o';
                }
                else if (world[x, y].VisitedOnCleanRun)
                {
                    c = 'x';
                }
                else if (world[x, y].Blocked)
                {
                    if (world[x, y].BlockerCandidate)
                    {
                        c = '%';
                    }
                    else
                    {
                        c = '#';
                    }
                }
                else
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