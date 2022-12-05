// read input
var input = await File.ReadAllLinesAsync("input.txt");

// Solve
Console.WriteLine($"Part 1: {Solve(input, CraneType.CrateMover9000)}");
Console.WriteLine($"Part 2: {Solve(input, CraneType.CrateMover9001)}");


string Solve(string[] input, CraneType craneType)
{
    // Parse input
    var separationIndex = input.ToList().IndexOf("");
    var stackInput = input.Take(separationIndex).Reverse();
    var movesInput = input.Skip(separationIndex + 1);

    var stacks = new Dictionary<int, Stack<char>>();
    foreach (var line in stackInput.Skip(1))
    {
        for (var i = 0; i < line.Length; i++)
        {
            if (!char.IsLetter(line[i])) continue;

            var index = (int)Math.Ceiling(i / 4d);
            if (!stacks.ContainsKey(index))
                stacks.Add(index, new Stack<char>());

            stacks[index].Push(line[i]);
        }
    }

    var moves = new List<(int count, int source, int dest)>();
    var moveRegex = new System.Text.RegularExpressions.Regex("move ([0-9]+) from ([0-9]+) to ([0-9]+)");
    foreach (var line in movesInput)
    {
        var match = moveRegex.Match(line);
        moves.Add((
            count: int.Parse(match.Groups[1].Value),
            source: int.Parse(match.Groups[2].Value),
            dest: int.Parse(match.Groups[3].Value)
            ));
    }

    // execute moves
    foreach (var move in moves)
    {
        var source = stacks[move.source];
        var dest = stacks[move.dest];
        switch (craneType)
        {
            case CraneType.CrateMover9000:
                for (var i = 0; i < move.count; i++) dest.Push(source.Pop());
                break;
            case CraneType.CrateMover9001:
                Stack<char> crane = new Stack<char>();
                for (var i = 0; i < move.count; i++) crane.Push(source.Pop());
                for (var i = 0; i < move.count; i++) dest.Push(crane.Pop());
                break;
        }
    }
    return string.Join("", stacks.Values.Select(s => s.Peek()));
}

enum CraneType { CrateMover9000, CrateMover9001 };
