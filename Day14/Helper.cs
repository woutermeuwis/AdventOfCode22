using Core;
using System.Drawing;

namespace Day14;

public static class Helper
{
    public static readonly Point SandOrigin = new(500, 0);

    public static readonly Size Down = new(0, 1);
    public static readonly Size DownLeft = new(-1, 1);
    public static readonly Size DownRight = new(1, 1);

    public static async Task<Boundaries> GetBoundaries(bool includeFloor)
    {
        var input = await File.ReadAllLinesAsync("input.txt");
        var coords = input.SelectMany(line => line.Split(" -> ")).Select(SplitPoint).ToList();
        coords.Add(SandOrigin);

        var minX = coords.Min(p => p.X) - 1;
        var maxX = coords.Max(p => p.X) + 1;
        var minY = coords.Min(p => p.Y);
        var maxY = coords.Max(p => p.Y);

        if (includeFloor)
        {
            maxY += 2;
            var h = 1 + maxY - minY;
            var w = 1 + maxX - minX;
            if (w < 2 * h)
            {
                var diff = 2 * h - w;
                minX -= diff;
                maxX += diff;
            }
        }
        return new(new(minX, minY), new(1 + maxX - minX, 1 + maxY - minY));
    }

    public static Grid<char> InitializeGrid(Boundaries boundaries)
    {
        var grid = new Grid<char>(boundaries.Size.Width, boundaries.Size.Height, '.', boundaries.Origin.X, boundaries.Origin.Y);
        grid.Set(SandOrigin.X, SandOrigin.Y, '+');
        return grid;
    }

    public static async Task DrawRocks(Grid<char> grid)
    {
        var input = await File.ReadAllLinesAsync("input.txt");
        foreach (var line in input)
        {
            var points = line.Split(" -> ").Select(SplitPoint).ToArray();

            // draw startpoint
            var cur = points[0];
            grid.Set(cur.X, cur.Y, '#');

            // loop other points
            for (var i = 1; i < points.Length; i++)
            {
                var next = points[i];

                if (cur.Y == next.Y) // horizontal
                {
                    var l = Math.Abs(cur.X - next.X);
                    var dir = cur.X > next.X ? -1 : 1;
                    for (var j = 1; j <= l; j++)
                        grid.Set(cur.X + (dir * j), cur.Y, '#');
                }
                else // vertical
                {
                    var l = Math.Abs(cur.Y - next.Y);
                    var dir = cur.Y > next.Y ? -1 : 1;
                    for (var j = 1; j <= l; j++)
                        grid.Set(cur.X, cur.Y + (dir * j), '#');
                }
                cur = next;
            }
        }
    }

    public static void DrawFloor(Grid<char> grid) => grid.SetRow(grid.Height - 1, '#');

    public static int SimulateSand(Grid<char> grid, Boundaries boundaries, Func<Point, bool> checkFinished)
    {
        for (var i = 0; i < int.MaxValue; i++)
        {
            var sandPosition = SandOrigin;
            while (true)
            {
                var lower = sandPosition + Down;
                var lowerLeft = sandPosition + DownLeft;
                var lowerRight = sandPosition + DownRight;

                if (boundaries.Contains(lower) && grid.Get(lower) == '.')
                    sandPosition = lower;
                else if (boundaries.Contains(lowerLeft) && grid.Get(lowerLeft) == '.')
                    sandPosition = lowerLeft;
                else if (boundaries.Contains(lowerRight) && grid.Get(lowerRight) == '.')
                    sandPosition = lowerRight;
                else
                {
                    grid.Set(sandPosition, 'o');
                    break;
                }
            }

            if (checkFinished(sandPosition))
                return i;
        }
        return -1;
    }

    private static Point SplitPoint(string input) => new(int.Parse(input.Split(',')[0]), int.Parse(input.Split(',')[1]));
}


