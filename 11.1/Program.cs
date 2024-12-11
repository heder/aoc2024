class Program
{
    static LinkedList<long> stones = [];

    static void Main()
    {
        var line = File.ReadAllText("in.txt");

        var s = line.Split(" ").Select(long.Parse);

        Dictionary<long, LinkedListNode<long>> nodes = [];
        LinkedList<long> stones = [];

        foreach (var item in s)
        {
            var lln = stones.AddLast(item);
            nodes.Add(item, lln);
        }
        
        for (int n = 0; n < 25; n++)
        {
            var stone = stones.First;

            do
            {
                if (stone.Value == 0)
                {
                    stone.Value = 1;
                }
                else if (stone.Value.ToString().Length % 2 == 0)
                {
                    var s1 = stone.Value.ToString().Substring(0, stone.Value.ToString().Length / 2);
                    var s2 = stone.Value.ToString().Substring(stone.Value.ToString().Length / 2);

                    var nxt = stones.AddAfter(stone, Convert.ToInt64(s2));
                    stones.AddAfter(stone, Convert.ToInt64(s1));

                    stones.Remove(stone);

                    stone = nxt;
                }
                else
                {
                    stone.Value = stone.Value * 2024;
                }


                stone = stone.Next;
            }
            while (stone != null);
        }

        var sum = stones.Count();

        Console.WriteLine(sum);
        Console.ReadKey();
    }
}