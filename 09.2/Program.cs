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
        int diskposition = 0;
        for (int i = 0; i < line.Length; i++)
        {
            int length = Convert.ToInt32(line[i].ToString());
            for (int j = 0; j < length; j++)
            {
                d.Add(new Block() { FileId = index, IsEmpty = false, DiskPosition = diskposition });
                diskposition++;
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
                d.Add(new Block() { FileId = -1, IsEmpty = true, DiskPosition = diskposition });
                diskposition++;
            }
        }

    ee:;

        disk = d.ToArray();
        lastNonFreeBlock = disk.Length - 1;

        var highestFileId = disk.Max(x => x.FileId);

        DumpDisk();

        for (int i = highestFileId; i >= 0; i--)
        {
            var blocks = disk.Where(x => x.FileId == i).ToArray();
            var minblock = blocks.Min(x => x.DiskPosition);
            var maxblock = blocks.Max(x => x.DiskPosition);
            var length = maxblock - minblock + 1;

            int holeStartsAt = -1;
            bool holeFound = false;

            for (int j = 0; j < minblock; j++)
            {
                if (disk[j].IsEmpty)
                {
                    holeStartsAt = j;

                    int freeLength = 0;
                    while (j < disk.Length && disk[j].IsEmpty == true)
                    {
                        freeLength++;

                        if (freeLength == length)
                        {
                            holeFound = true;
                            goto bb;
                        }

                        j++;
                    }
                }
            }

        bb:;

            if (holeFound)
            {
                // Hole found. Move file.
                for (int j = 0; j < length; j++)
                {
                    disk[holeStartsAt + j] = disk[minblock + j];
                    disk[holeStartsAt + j].DiskPosition = holeStartsAt + j;
                    disk[minblock + j] = new Block() { FileId = -1, IsEmpty = true, DiskPosition = minblock + j };
                }
            }
        }

        Int64 sum = 0;
        for (int i = 0; i < disk.Length; i++)
        {
            if (disk[i].IsEmpty == false)
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