
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
            // (a_n * a_x) + (b_n * b_x) = p_x
            // (a_n * a_y) + (b_n * b_y) = p_y

            // Cramer's rule!
            m.a_n = (m.Prize_X * m.B_Y - m.Prize_Y * m.B_X) / (m.A_X * m.B_Y - m.A_Y * m.B_X);
            m.b_n = (m.A_X * m.Prize_Y - m.A_Y * m.Prize_X) / (m.A_X * m.B_Y - m.A_Y * m.B_X);

            if (m.a_n * m.A_X + m.b_n * m.B_X != m.Prize_X || m.a_n * m.A_Y + m.b_n * m.B_Y != m.Prize_Y)
            {
                m.CanWin = false;
            }
            else
            {
                m.CanWin = true;
            }
        }

        Console.WriteLine(machines.Where(f => f.CanWin == true).Sum(f => f.Prize));
        Console.ReadKey();
    }
}