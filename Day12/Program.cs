using System.Drawing;

char[][] maze = (await File.ReadAllLinesAsync("input.txt")).Select(l => l.ToCharArray()).ToArray();

// get metadata
var height = maze.Length;
var width = maze[0].Length;
Size[] moves = new Size[] { new Size(1, 0), new Size(0, 1), new Size(-1, 0), new Size(0, -1) };

SolvePart1();
SolvePart2();


void SolvePart1()
{
    Point s = default, e = default;
    for (var y = 0; y < height; y++)
    {
        for (var x = 0; x < width; x++)
        {
            if (maze[y][x] == 'S') s = new Point(x, y);
            if (maze[y][x] == 'E') e = new Point(x, y);
        }
    }
    var shortest = FindShortestPath(s, e);
    Console.WriteLine($"Part 1: {FindShortestPath(s, e).DistanceTraveled}");
}

void SolvePart2()
{
    Point end = default;
    var startPoints = new List<Point>();

    for (var y = 0; y < height; y++)
    {
        for (var x = 0; x < width; x++)
        {
            if (maze[y][x] == 'S' || maze[y][x] == 'a') startPoints.Add(new Point(x, y));
            if (maze[y][x] == 'E') end = new Point(x, y);
        }
    }

    var paths = startPoints.Select(start => FindShortestPath(start, end));
    Console.WriteLine($"Part 2: {paths.Min(path => path.DistanceTraveled)}");
}

Square FindShortestPath(Point start, Point end)
{
    var paths = new Dictionary<Point, Square>();
    var verified = new Dictionary<Point, Square>();

    paths.Add(start, new Square(start, GetElevation(start), 0));


    while (true)
    {
        if (!paths.Any()) return new Square(default, default, int.MaxValue);

        var cur = paths.Values.OrderBy(s => s.GetHueristic(end)).First();
        paths.Remove(cur.Pos);
        verified.Add(cur.Pos, cur);

        var neighbours = GetNeighbours(cur.Pos);
        foreach (var neighbour in neighbours)
        {
            if (GetElevation(neighbour) > cur.Elevation + 1)
                continue;

            var square = new Square(neighbour, GetElevation(neighbour), cur.DistanceTraveled + 1);

            if (neighbour == end)
                return square;

            if (verified.ContainsKey(neighbour))
            {
                var prevVerified = verified[neighbour];
                if (prevVerified.DistanceTraveled > square.DistanceTraveled)
                {
                    verified.Remove(neighbour);
                    paths.Add(square.Pos, square);
                }
                continue;
            }

            if (paths.ContainsKey(neighbour))
            {
                if (paths[neighbour].DistanceTraveled > cur.DistanceTraveled + 1)
                    paths[neighbour] = square;
                continue;
            }

            paths.Add(neighbour, square);
        }
    }
}


int GetElevation(Point pos) => maze[pos.Y][pos.X] switch
{
    'S' => 'a',
    'E' => 'z',
    _ => maze[pos.Y][pos.X]
} - 'a';

Point[] GetNeighbours(Point pos) => moves.Select(s => pos + s).Where(s => s.X >= 0 && s.Y >= 0 && s.X < width && s.Y < height).ToArray();

class Square
{
    public Point Pos { get; }
    public int Elevation { get; }

    public int DistanceTraveled { get; }

    public Square(Point pos, int elevation, int distanceTraveled)
    {
        Pos = pos;
        Elevation = elevation;
        DistanceTraveled = distanceTraveled;
    }

    public int GetHueristic(Point end) => GetDistanceToEnd(end) + DistanceTraveled;

    private int GetDistanceToEnd(Point end) => (int)Math.Floor(Math.Sqrt(Math.Pow(end.X - Pos.X, 2) + Math.Pow(end.Y - Pos.Y, 2)));

}