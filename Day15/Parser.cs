using System.Drawing;
using System.Text.RegularExpressions;

namespace Day15;

public static class Helper
{
    static readonly Regex InputRegex = new Regex(@"Sensor at x=([\-0-9]*), y=([\-0-9]*): closest beacon is at x=([\-0-9]*), y=([\-0-9]*)");

    public static List<(Point sensor, Point beacon, int dist)> ParseInput(string[] input)
    {
        return input.Select(line =>
        {
            var match = InputRegex.Match(line);
            var sensor = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
            var beacon = new Point(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
            return (sensor, beacon, Distance(sensor, beacon));
        }).ToList();
    }

    public static List<(int start, int end)> GetRowRanges(List<(Point sensor, Point beacon, int dist)> sensors, int line)
    {
        var ranges = new List<(int start, int end)>();
        foreach (var s in sensors)
        {
            if (Math.Abs(s.sensor.Y - line) > s.dist) continue;

            var vertical = Math.Abs(s.sensor.Y - line);
            var horizontal = s.dist - vertical;
            var x0 = s.sensor.X - horizontal;
            var x1 = s.sensor.X + horizontal;
            var cur = (start: x0, end: x1);

            var newRanges = new List<(int start, int end)>();
            foreach (var range in ranges)
            {
                if (range.start > cur.end + 1 || range.end < cur.start - 1)
                    newRanges.Add(range);
                else
                    cur = (start: (int)Math.Min(range.start, cur.start), end: (int)Math.Max(range.end, cur.end));
            }
            newRanges.Add(cur);
            ranges = newRanges.OrderBy(s => s.start).ToList();
        }
        return ranges;
    }

    public static int[] SensorsAndBeaconsInRow(int row, List<(Point sensor, Point beacon, int dist)> sensors)
    {
        return sensors
            .Select(s => new Point[] { new Point(s.sensor.X, s.sensor.Y), new Point(s.beacon.X, s.beacon.Y) })
            .SelectMany(p => p)
            .Where(p => p.Y == row)
            .Select(p => p.X)
            .Distinct()
            .ToArray();
    }

    public static int Distance(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
}
