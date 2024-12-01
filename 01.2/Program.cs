class Program
{
    static void Main()
    {
        var lines = File.ReadLines("in.txt").ToArray();

        var l1 = new List<int>();
        var l2 = new List<int>();

        for (int i = 0; i < lines.Length; i++)
        {
            var p = lines[i].Split("   ");

            l1.Add(Convert.ToInt32(p[0].Trim()));
            l2.Add(Convert.ToInt32(p[1].Trim()));
        }

        int sum = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            var v1 = l1[i];
            var v2 = l2.Where(f => f == v1).Count();

            var score = v1 * v2;
            sum += score;
        }

        Console.WriteLine(sum);
        Console.ReadKey();
    }
}