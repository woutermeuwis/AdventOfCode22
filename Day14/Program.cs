using Day14;
using System.Drawing;

await SolvePart1();
await SolvePart2();

async Task SolvePart1()
{
    var boundaries = await Helper.GetBoundaries(false);
    var grid = Helper.InitializeGrid(boundaries);
    await Helper.DrawRocks(grid);
    Console.WriteLine($"Part 1: {Helper.SimulateSand(grid, boundaries, (Point p) => p.Y == boundaries.Bounds.Y - 1)}");
}

async Task SolvePart2()
{
    var boundaries = await Helper.GetBoundaries(true);
    var grid = Helper.InitializeGrid(boundaries);
    await Helper.DrawRocks(grid);
    Helper.DrawFloor(grid);
    Console.WriteLine($"Part 2: {Helper.SimulateSand(grid, boundaries, (Point p) => p == Helper.SandOrigin) + 1}");
}