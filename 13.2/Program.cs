
class Machine
{
    public long A_X { get; set; }
    public long A_Y { get; set; }

    public long B_X { get; set; }
    public long B_Y { get; set; }

    public long Prize_X { get; set; }
    public long Prize_Y { get; set; }

    public long a_n { get; set; }
    public long b_n { get; set; }

    public bool CanWin { get; set; }

    public long Prize
    {
        get
        {
            return (a_n * 3) + (b_n);
        }
    }
}




class Program
{
    private static List<Machine> machines = [];

    static void Main()
    {
        var lines = File.ReadAllLines("in.txt").ToArray();

        int line = 0;
        while (line < lines.Length)
        {
            var machine = new Machine();

            {
                var l1 = lines[line].Split(":");
                var l2 = l1[1].Split(",");
                var a = l2[0].Split("+");
                var b = l2[1].Split("+");
                var ax = Convert.ToInt64(a[1]);
                var ay = Convert.ToInt64(b[1]);
                machine.A_X = ax;
                machine.A_Y = ay;
            }

            line++;
            {
                var l1 = lines[line].Split(":");
                var l2 = l1[1].Split(",");
                var a = l2[0].Split("+");
                var b = l2[1].Split("+");
                var ax = Convert.ToInt64(a[1]);
                var ay = Convert.ToInt64(b[1]);
                machine.B_X = ax;
                machine.B_Y = ay;
            }

            line++;
            {
                var l1 = lines[line].Split(":");
                var l2 = l1[1].Split(",");
                var a = l2[0].Split("=");
                var b = l2[1].Split("=");
                var ax = Convert.ToInt64(a[1]);
                var ay = Convert.ToInt64(b[1]);
                machine.Prize_X = ax + 10000000000000;
                machine.Prize_Y = ay + 10000000000000;
            }

            machines.Add(machine);

            line++;
            line++;
        }

        foreach (var m in machines)
        {
            long a_n = 0;
            long b_n = 0;

            for (long na = 0; na <= 10000000000000; na++)
            {


                if (na == 10000000000000)
                {
                    m.CanWin = false;
                    goto earlyExit;
                }


                a_n = 0;
                b_n = 0;

                long currentX = 0;
                long currentY = 0;

                for (; a_n < na; a_n++)
                {
                    currentX += m.A_X;
                    currentY += m.A_Y;

                    if (currentX == m.Prize_X && currentY == m.Prize_Y)
                    {
                        m.CanWin = true;
                        goto earlyExit;
                    }

                    if (currentX > m.Prize_X || currentY > m.Prize_Y)
                    {
                        goto ee;
                    }
                }

                for (; b_n < 100000; b_n++)
                {
                    currentX += m.B_X;
                    currentY += m.B_Y;

                    if (currentX == m.Prize_X && currentY == m.Prize_Y)
                    {
                        m.CanWin = true;
                        goto earlyExit;
                    }

                    if (currentX > m.Prize_X || currentY > m.Prize_Y)
                    {
                        goto ee;
                    }
                }

            ee:;
            }

        earlyExit:;
            m.a_n = a_n;
            m.b_n = (b_n + 1);
        }

        Console.WriteLine(machines.Where(f => f.CanWin == true).Sum(f => f.Prize));
        Console.ReadKey();


        //var s = line.Split(" ").Select(long.Parse);

        //Dictionary<long, LinkedListNode<long>> nodes = [];
        //LinkedList<long> stones = [];

        //foreach (var item in s)
        //{
        //    var lln = stones.AddLast(item);
        //    nodes.Add(item, lln);
        //}

        //for (int n = 0; n < 25; n++)
        //{
        //    var stone = stones.First;

        //    do
        //    {
        //        if (stone.Value == 0)
        //        {
        //            stone.Value = 1;
        //        }
        //        else if (stone.Value.ToString().Length % 2 == 0)
        //        {
        //            var s1 = stone.Value.ToString().Substring(0, stone.Value.ToString().Length / 2);
        //            var s2 = stone.Value.ToString().Substring(stone.Value.ToString().Length / 2);

        //            var nxt = stones.AddAfter(stone, Convert.ToInt64(s2));
        //            stones.AddAfter(stone, Convert.ToInt64(s1));

        //            stones.Remove(stone);

        //            stone = nxt;
        //        }
        //        else
        //        {
        //            stone.Value = stone.Value * 2024;
        //        }


        //        stone = stone.Next;
        //    }
        //    while (stone != null);
        //}

        //var sum = stones.Count();

    }
}