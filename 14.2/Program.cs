using System.Drawing;

class Program
{
    class Robot
    {
        public int currentX { get; set; }
        public int currentY { get; set; }

        public int velocityX { get; set; }
        public int velocityY { get; set; }
    }

    private static int xmax = 100;
    private static int ymax = 102;

    static void Main()
    {
        var lines = File.ReadLines("in.txt").ToArray();

        List<Robot> robots = [];

        foreach (var item in lines)
        {
            var s1 = item.Split(' ');

            var s2 = s1[0].Split('=')[1];
            var s3 = s1[1].Split('=')[1];

            var s4 = s2.Split(',').Select(int.Parse).ToArray();
            var s5 = s3.Split(',').Select(int.Parse).ToArray();

            robots.Add(new Robot()
            {
                currentX = s4[0],
                currentY = s4[1],
                velocityX = s5[0],
                velocityY = s5[1]
            });

        }

        for (int i = 1; i <= 10000; i++)
        {
            foreach (var robot in robots)
            {
                var nextX = robot.currentX + robot.velocityX;
                var nextY = robot.currentY + robot.velocityY;

                if (nextX < 0)
                {
                    robot.currentX = xmax + 1 - Math.Abs(nextX);
                }
                else if (nextX > xmax)
                {
                    robot.currentX = nextX - xmax - 1;
                }
                else
                {
                    robot.currentX = nextX;
                }

                if (nextY < 0)
                {
                    robot.currentY = ymax + 1 - Math.Abs(nextY);
                }
                else if (nextY > ymax)
                {
                    robot.currentY = nextY - ymax - 1;
                }
                else
                {
                    robot.currentY = nextY;
                }
            }

            Console.Clear();
            Console.WriteLine($"Elapsed: {i}");
            DumpWorld(i);
        }

        Console.WriteLine(q1 * q2 * q3 * q4);
        Console.ReadKey();


        // Kolla igenom 10k små png:er manuellt? Ja, varför inte? ;-)
        void DumpWorld(int iteration)
        {
            using Bitmap bm = new Bitmap(xmax + 1, ymax + 1);

            for (int x = 0; x < xmax; x++)
            {
                for (int y = 0; y < ymax; y++)
                {
                    bm.SetPixel(x, y, Color.Black);
                }
            }

            foreach (var robot in robots)
            {
                bm.SetPixel(robot.currentX, robot.currentY, Color.White);
            }

            string filePath = $"{iteration}.png";
            bm.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}


