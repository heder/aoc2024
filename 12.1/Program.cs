using System.Text;

class Program
{
    class Coord
    {
        public int X { get; set; }
        public int Y { get; set; }

        class Tile
        {
            public Coord Position { get; set; }
            public string PlantType { get; set; }

            public string GetOgcGeoPolygon()
            {
                return $"POLYGON(({Position.X} {Position.Y}, {Position.X + 1} {Position.Y}, {Position.X + 1} {Position.Y + 1}, {Position.X} {Position.Y + 1}, {Position.X} {Position.Y}))";
            }
        }

        private static Tile[,] world;

        private static int xmax;
        private static int ymax;

        private static List<Tile> antennas = [];

        static void Main()
        {
            var lines = File.ReadLines("in.txt").ToList();

            xmax = lines[0].Length;
            ymax = lines.Count();

            world = new Tile[xmax, ymax];

            for (int y = 0; y < ymax; y++)
            {
                for (int x = 0; x < xmax; x++)
                {
                    world[x, y] = new Tile() { PlantType = lines[y][x].ToString(), Position = new Coord { X = x, Y = y } };
                }
            }

            Dumpworld();

            StringBuilder sb = new();
            for (int y = 0; y < ymax; y++)
            {
                for (int x = 0; x < xmax; x++)
                {
                    sb.AppendLine($"insert into u11 (plant_type, geom) values ('{world[x, y].PlantType}', geometry::STGeomFromText('{world[x, y].GetOgcGeoPolygon()}', 0));");
                }
            }

            Console.WriteLine(sb.ToString());
            System.IO.File.WriteAllText("out.sql", sb.ToString());

            Console.ReadKey();

        }

        private static void Dumpworld()
        {
            for (int y = 0; y < ymax; y++)
            {
                for (int x = 0; x < xmax; x++)
                {
                    string c = world[x, y].PlantType;

                    Console.Write(c);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
