class Program
{
    static void Main()
    {
        var line = File.ReadAllText("in.txt");
        var s = line.Split(" ").Select(long.Parse);
        Dictionary<long, long> buckets = [];

        foreach (var item in s)
        {
            buckets.Add(item, 1);
        }

        for (int n = 0; n < 75; n++)
        {
            Console.WriteLine(n);

            Dictionary<long, long> newBuckets = [];

            foreach (var b in buckets)
            {
                if (b.Key == 0)
                {
                    if (newBuckets.ContainsKey(1))
                    {
                        newBuckets[1] += b.Value;
                    }
                    else
                    {
                        newBuckets.Add(1, b.Value);
                    }

                    buckets[0] = 0;
                }
                else if (b.Key.ToString().Length % 2 == 0)
                {
                    var s1 = b.Key.ToString().Substring(0, b.Key.ToString().Length / 2);
                    var s2 = b.Key.ToString().Substring(b.Key.ToString().Length / 2);

                    if (newBuckets.ContainsKey(Convert.ToInt64(s1)))
                    {
                        newBuckets[Convert.ToInt64(s1)] += b.Value;
                    }
                    else
                    {
                        newBuckets.Add(Convert.ToInt64(s1), b.Value);
                    }

                    if (newBuckets.ContainsKey(Convert.ToInt64(s2)))
                    {
                        newBuckets[Convert.ToInt64(s2)] += b.Value;
                    }
                    else
                    {
                        newBuckets.Add(Convert.ToInt64(s2), b.Value);
                    }

                    buckets[b.Key] = 0;
                }
                else
                {
                    var v = b.Key * 2024;

                    if (newBuckets.ContainsKey(v))
                    {
                        newBuckets[v] += b.Value;
                    }
                    else
                    {
                        newBuckets.Add(v, b.Value);
                    }

                    buckets[b.Key] = 0;
                }

            }

            foreach (var item in newBuckets)
            {
                if (buckets.ContainsKey(item.Key))
                {
                    buckets[item.Key] += item.Value;
                }
                else
                {
                    buckets.Add(item.Key, item.Value);
                }
            }

            buckets.Where(f => f.Value == 0).ToList().ForEach(f => buckets.Remove(f.Key));
        }

        var sum = buckets.Sum(f => f.Value);

        Console.WriteLine(sum);
        Console.ReadKey();
    }
}