class Program
{
    class Report
    {
        public bool IsSafe { get; set; } = true;
        public List<int> Levels { get; set; } = new();
        public List<int> OriginalLevels { get; set; } = new();
    }

    static void Main()
    {
        var reports = new List<Report>();

        var lines = File.ReadLines("in.txt").ToArray();

        for (int i = 0; i < lines.Length; i++)
        {
            var r = new Report();
            var p = lines[i].Split(" ");
            foreach (var level in p)
            {
                r.Levels.Add(Convert.ToInt32(level));
            }

            reports.Add(r);
        }

        foreach (var report in reports)
        {
            CheckReport(report);
        }

        var toDampen = reports.Where(f => f.IsSafe == false).ToList();

        foreach (var report in toDampen)
        {
            report.OriginalLevels = report.Levels.ToList();

            for (int i = 0; i < report.OriginalLevels.Count; i++)
            {
                report.Levels = report.OriginalLevels.ToList();
                report.Levels.RemoveAt(i);

                CheckReport(report);

                if (report.IsSafe)
                {
                    break;
                }
            }
        }

        var sum = reports.Count(r => r.IsSafe);

        Console.WriteLine(sum);
        Console.ReadKey();
    }

    private static void CheckReport(Report report)
    {
        bool increasing = false;
        for (int i = 1; i < report.Levels.Count; i++)
        {
            var diff = report.Levels[i - 1] - report.Levels[i];
            if (i == 1 && diff < 0)
            {
                increasing = true;
            }

            var diff2 = Math.Abs(diff);
            if (diff2 > 0 && diff2 <= 3 && ((increasing && diff < 0) || increasing == false && diff > 0))
            {
                report.IsSafe = true;
            }
            else
            {
                report.IsSafe = false;
                break;
            }
        }
    }
}