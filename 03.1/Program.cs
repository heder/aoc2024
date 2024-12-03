using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        var line = File.ReadAllText("in.txt");

        Regex regex = new(@"mul\(\d{1,3},\d{1,3}\)");
        var x = regex.Matches(line);

        int sum = 0;
        foreach (Match match in x)
        {
            var s = match.Value[4..].Trim('(', ')').Split(",");
            var v1 = Convert.ToInt32(s[0]);
            var v2 = Convert.ToInt32(s[1]);
            var result = v1 * v2;
            sum += result;
        }

        Console.WriteLine(sum);
        Console.ReadKey();
    }
}