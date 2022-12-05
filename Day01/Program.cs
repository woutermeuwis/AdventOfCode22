// Read input
var input = await File.ReadAllLinesAsync("input.txt");

// Parse input
var elves = new List<int>();
var elf = 0;
foreach (var line in input)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        elves.Add(elf);
        elf = 0;
    }
    else
    {
        elf += int.Parse(line);
    }
}


// Solve
Console.WriteLine("Part 1: " + elves.Max());
Console.WriteLine("Part 2: " + elves.OrderByDescending(e => e).Take(3).Sum());

return 0;