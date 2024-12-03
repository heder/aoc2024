using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        var line = File.ReadAllText("in.txt");

        bool enabled = true;
        while (true)
        {
            string toparse;

            if (enabled)
            {
                var pos = line.IndexOf("don't()");

                if (pos == -1)
                {
                    ParseMul(line);
                    Console.WriteLine(sum);
                    Console.ReadKey();
                }

                toparse = line.Substring(0, pos);
                ParseMul(toparse);

                enabled = false;
                line = line.Substring(pos);
            }
            else
            {
                var pos = line.IndexOf("do()");

                if (pos == -1)
                {
                    Console.WriteLine(sum);
                    Console.ReadKey();
                }

                enabled = true;
                line = line.Substring(pos);
            }
        }
    }

    private static int sum = 0;

    private static int ParseMul(string line)
    {
        Regex regex = new(@"mul\(\d{1,3},\d{1,3}\)");
        var x = regex.Matches(line);

        foreach (Match match in x)
        {
            var s = match.Value[4..].Trim('(', ')').Split(",");
            var v1 = Convert.ToInt32(s[0]);
            var v2 = Convert.ToInt32(s[1]);
            var result = v1 * v2;
            sum += result;
        }

        return sum;
    }
}