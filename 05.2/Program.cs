using System.Data;

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

        var inValid = updates.Where(x => x.IsValid == false).ToList();

        // Reorder until valid
        foreach (var item in inValid)
        {

        rerun:;
            Console.WriteLine(item.Pages.Select(item => item.ToString()).Aggregate((x, y) => x + "," + y));

            for (int i = 0; i < item.Pages.Count - 1; i++)
            {
                var checking = item.Pages[i];

                if (rules.ContainsKey(checking) == false)
                {
                    item.IsValid = false;

                    var v1 = item.Pages[i];
                    var v2 = item.Pages[i + 1];

                    item.Pages[i] = v2;
                    item.Pages[i + 1] = v1;

                    goto rerun;
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

                        var v1 = item.Pages[i];
                        var v2 = item.Pages[j];

                        item.Pages[i] = v2;
                        item.Pages[j] = v1;

                        goto rerun;
                    }
                }
            }

        }

        foreach (var item in inValid)
        {
            item.MiddleValue = item.Pages[item.Pages.Count / 2];
        }

        var sum = inValid.Sum(x => x.MiddleValue);

        Console.WriteLine(sum);
        Console.ReadKey();
    }
}
