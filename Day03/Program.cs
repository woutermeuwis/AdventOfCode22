// Read input
var input = await File.ReadAllLinesAsync("input.txt");

// Solve
Console.WriteLine("Part 1: " + input.Select(GetItemInBothCompartments).Select(GetPriority).Sum());
Console.WriteLine("Part 2: " + GroupElves(input).Select(GetBadge).Select(GetPriority).Sum());


// Helpers
char GetItemInBothCompartments(string rucksack)
{
    var length = rucksack.Length;
    var second = rucksack.Skip(length / 2);
    return rucksack.Take(length / 2).First(c => second.Contains(c));
}

int GetPriority(char c)
{
    if (c >= 'a')
        return c - 'a' + 1;
    else
        return c - 'A' + 27;
}

string[][] GroupElves(string[] input)
{
    var groups = new string[input.Length / 3][];
    var group = new string[3];
    for (var i = 0; i < input.Length; i++)
    {
        group[i % 3] = input[i];

        if (i % 3 == 2)
        {
            groups[i / 3] = group;
            group = new string[3];
        }
    }
    return groups;
}

char GetBadge(string[] group)
{
    var allItems = group.Select(e => e.Distinct()).SelectMany(e => e);
    var result = allItems.Distinct().First(i => allItems.Count(c => c == i) == 3);
    return result;
}