using Core;
using Day15;
using System.Drawing;

await SolvePart1().RunWithTimer("Part 1: ");
await SolvePart2().RunWithTimer("Part 2: ");

async Task<int> SolvePart1()
{
    var input = await File.ReadAllLinesAsync("input.txt");
    var sensors = Helper.ParseInput(input);
    const int line = 2000000;
    var ranges = Helper.GetRowRanges(sensors, line);
    return ranges.Select(r => 1 + r.end - r.start).Sum() - Helper.SensorsAndBeaconsInRow(line, sensors).Count(x => ranges.Any(r => r.start <= x && r.end >= x));
}


async Task<long> SolvePart2()
{
    var input = await File.ReadAllLinesAsync("input.txt");
    var sensors = Helper.ParseInput(input);

    const int min = 0, max = 4000000;
    long Freq(Point p) => 4000000L * p.X + p.Y;

    var results = Enumerable.Range(min, max)
        .Select(i => (row: i, ranges: Helper.GetRowRanges(sensors, i)))
        .ToList();
    var count = results.Where(r => r.ranges.Count > 1).Count();

    var beaconRow = results.FirstOrDefault(x => x.ranges.Count() > 1);
    var beacon = new Point(beaconRow.ranges.Min(r => r.end) + 1, beaconRow.row);
    return Freq(beacon);
}

