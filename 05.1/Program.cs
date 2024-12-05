using System.Data;
using System.Formats.Asn1;

class Program
{
    class Rule
    {
        public int Page { get; set; }
        public List<int> Before { get; set; } = [];
    }

    class Update
    {
        public List<int> Pages { get; set; } = [];
        public bool IsValid { get; set; } = false;

        public int MiddleValue { get; set; }
    }




    //private static char[,] grid;

    //private static int xmax;
    //private static int ymax;

    private static Dictionary<int, Rule> rules = [];
    private static List<Update> updates = [];

    static void Main()
    {
        var lines = File.ReadLines("in.txt").ToList();

        int currentLine = 0;
        while (lines[currentLine] != string.Empty)
        {
            // Parse rules
            var line = lines[currentLine];
            var splitted = line.Split("|");
            var i1 = int.Parse(splitted[0]);
            var i2 = int.Parse(splitted[1]);

            if (!rules.ContainsKey(i1))
            {
                var r = new Rule() { Page = i1 };
                r.Before.Add(i2);
                rules.Add(i1, r);
            }
            else
            {
                rules[i1].Before.Add(i2);
            }

            currentLine++;
        }

        currentLine++;

        while (currentLine < lines.Count)
        {
            var line = lines[currentLine];
            var splitted = line.Split(",");
            var update = new Update() { Pages = splitted.Select(x => int.Parse(x)).ToList() };
            updates.Add(update);
            currentLine++;
        }



        foreach (var item in updates)
        {
            item.IsValid = true;

            for (int i = 0; i < item.Pages.Count - 1; i++)
            {
                var checking = item.Pages[i];

                if (rules.ContainsKey(checking) == false)
                {
                    item.IsValid = false;
                    goto earlyExit;
                }

                var rule = rules[checking];

                for (int j = i + 1; j < item.Pages.Count; j++)
                {
                    var mustbeBefore = item.Pages[j];
                    if (rule.Before.Contains(mustbeBefore))
                    {
                        item.IsValid = true;
                    }
                    else
                    {
                        item.IsValid = false;
                        goto earlyExit;
                    }
                }
            }

        earlyExit:;

        }

        var valid = updates.Where(x => x.IsValid);

        foreach (var item in valid)
        {
            item.MiddleValue = item.Pages[item.Pages.Count / 2] ;
        }

        var sum = valid.Sum(x => x.MiddleValue);

        Console.WriteLine(sum);
        Console.ReadKey();
    }

    private static int sum = 0;

    //private static void Search(int x, int y)
    //{
    //    if (x < xmax - 3 && grid[x, y] == 'X' && grid[x + 1, y] == 'M' && grid[x + 2, y] == 'A' && grid[x + 3, y] == 'S') { sum++; Console.WriteLine("Found XMAS"); }
    //    if (x > 2 && grid[x, y] == 'X' && grid[x - 1, y] == 'M' && grid[x - 2, y] == 'A' && grid[x - 3, y] == 'S') { sum++; Console.WriteLine("Found XMAS"); }
    //    if (y < ymax - 3 && grid[x, y] == 'X' && grid[x, y + 1] == 'M' && grid[x, y + 2] == 'A' && grid[x, y + 3] == 'S') { sum++; Console.WriteLine("Found XMAS"); }
    //    if (y > 2 && grid[x, y] == 'X' && grid[x, y - 1] == 'M' && grid[x, y - 2] == 'A' && grid[x, y - 3] == 'S') { sum++; Console.WriteLine("Found XMAS"); }

    //    if (x < xmax - 3 && y < ymax - 3 && grid[x, y] == 'X' && grid[x + 1, y + 1] == 'M' && grid[x + 2, y + 2] == 'A' && grid[x + 3, y + 3] == 'S') { sum++; Console.WriteLine("Found XMAS"); }
    //    if (x > 2 && y > 2 && grid[x, y] == 'X' && grid[x - 1, y - 1] == 'M' && grid[x - 2, y - 2] == 'A' && grid[x - 3, y - 3] == 'S') { sum++; Console.WriteLine("Found XMAS"); }
    //    if (x < xmax - 3 && y > 2 && grid[x, y] == 'X' && grid[x + 1, y - 1] == 'M' && grid[x + 2, y - 2] == 'A' && grid[x + 3, y - 3] == 'S') { sum++; Console.WriteLine("Found XMAS"); }
    //    if (x > 2 && y < ymax - 3 && grid[x, y] == 'X' && grid[x - 1, y + 1] == 'M' && grid[x - 2, y + 2] == 'A' && grid[x - 3, y + 3] == 'S') { sum++; Console.WriteLine("Found XMAS"); }
    //}

    //private static void DumpGrid()
    //{
    //    for (int y = 0; y < ymax; y++)
    //    {
    //        for (int x = 0; x < xmax; x++)
    //        {
    //            Console.Write(grid[x, y]);
    //        }

    //        Console.WriteLine();
    //    }
    //}
}
