class Program
{
    class Block
    {
        public int FileId = -1;
        public bool IsEmpty = false;
        public int DiskPosition = -1;
    }

    static Block[] disk;

    static int firstFreeBlock = 0;
    static int lastNonFreeBlock = -1;

    static void Main()
    {
        var line = File.ReadAllText("in.txt").ToArray();

        List<Block> d = [];

        int index = 0;
        for (int i = 0; i < line.Length; i++)
        {
            int length = Convert.ToInt32(line[i].ToString());
            for (int j = 0; j < length; j++)
            {
                d.Add(new Block() { FileId = index, IsEmpty = false, DiskPosition = i });
            }

            index++;
            i++;

            if (i >= line.Length)
            {
                goto ee;
            }

            int space = Convert.ToInt32(line[i].ToString());
            for (int j = 0; j < space; j++)
            {
                d.Add(new Block() { FileId = -1, IsEmpty = true, DiskPosition = i });
            }
        }

    ee:;

        disk = d.ToArray();
        lastNonFreeBlock = disk.Length - 1;

        DumpDisk();

        while (true)
        {
            SetNextFree();
            SetLastNonFree();

            if (firstFreeBlock == lastNonFreeBlock + 1)
            {
                break;
            }

            disk[firstFreeBlock] = disk[lastNonFreeBlock];
            disk[firstFreeBlock].DiskPosition = firstFreeBlock;

            disk[lastNonFreeBlock] = new Block() { FileId = -1, IsEmpty = true, DiskPosition = lastNonFreeBlock };
        }

        DumpDisk();

        Int64 sum = 0;
        for (int i = 0; i <= lastNonFreeBlock; i++)
        {
            sum += (i * disk[i].FileId);
        }

        Console.WriteLine(sum);
        Console.ReadKey();
    }


    public static void SetNextFree()
    {
        for (int i = firstFreeBlock; i < disk.Length; i++)
        {
            if (disk[i].IsEmpty == true)
            {
                firstFreeBlock = i;
                break;
            }
        }
    }

    public static void SetLastNonFree()
    {
        for (int i = lastNonFreeBlock; i >= 0; i--)
        {
            if (disk[i].IsEmpty == false)
            {
                lastNonFreeBlock = i;
                break;
            }
        }
    }


    private static void DumpDisk()
    {
        for (int i = 0; i < disk.Length; i++)
        {
            if (disk[i].IsEmpty)
            {
                Console.Write(".");
            }
            else
            {
                Console.Write(disk[i].FileId.ToString());
            }
        }

        Console.Write(Environment.NewLine);
    }
}