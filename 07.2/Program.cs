
class Expression
{
    public Int64 Sum { get; set; }
    public List<Int64> Factors { get; set; } = [];
    public bool CanBeMadeValid { get; set; } = false;
}



class Program
{
    static void Main()
    {
        var lines = File.ReadLines("in.txt").ToArray();

        List<Expression> expressions = [];

        for (int i = 0; i < lines.Length; i++)
        {
            var e = new Expression();
            var s = lines[i].Split(':');
            e.Sum = Convert.ToInt64(s[0].Trim());
            e.Factors = s[1].Trim().Split(' ').Select(x => Convert.ToInt64(x.Trim())).ToList();
            expressions.Add(e);
        }

        string chars = "*+|";

        foreach (var e in expressions)
        {
            var numOfOperators = e.Factors.Count - 1;
            List<string> strings = [];
            Permute("", 0);
            strings = strings.Where(s => s.Length == numOfOperators).ToList();

            for (int i = 0; i < strings.Count; i++)
            {
                Int64 s = e.Factors[0];
                for (int j = 0; j < e.Factors.Count - 1; j++)
                {
                    switch (strings[i][j])
                    {
                        case '*':
                            s *= e.Factors[j + 1];
                            break;
                        case '+':
                            s += e.Factors[j + 1];
                            break;
                        case '|':
                            s = Convert.ToInt64(s.ToString() + e.Factors[j + 1].ToString());
                            break;
                    }
                }

                if (s == e.Sum)
                {
                    e.CanBeMadeValid = true;
                    goto ee;
                }
            }

        ee:;

            void Permute(string pre, int level)
            {
                level += 1;
                foreach (char c in chars)
                {
                    strings.Add(pre + c);
                    if (level < numOfOperators)
                    {
                        Permute(pre + c, level);
                    }
                }
            }
        }

        var sum = expressions.Where(x => x.CanBeMadeValid).Sum(f => f.Sum);

        Console.WriteLine(sum);
        Console.ReadKey();
    }
}