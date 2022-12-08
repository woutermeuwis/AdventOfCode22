var input = (await File.ReadAllLinesAsync("input.txt")).Select(line => line.ToCharArray()).ToArray();

var visibleTrees = 0;
var highestScenic = 0;

for (var y = 0; y < input.Length; y++)
{
    var row = input[y];
    for (var x = 0; x < input[y].Length; x++)
    {
        var col = input.Select(r => r[x]).ToArray();
        var tree = row[x];
        var left = row[..x];
        var up = col[..y];
        var right = row[(x + 1)..];
        var down = col[(y + 1)..];

        if (tree == 0) continue;

        if (y == 0 || y == input.Length - 1 || x == 0 || x == row.Length - 1
            || new[] { left.Max(), right.Max(), up.Max(), down.Max() }.Min() < tree)
            visibleTrees++;

        int GetIndexOfHigherThen(char[] arr, int treshold)
        {
            if (arr.Any())
                for (var i = 0; i < arr.Length; i++)
                    if (arr[i] >= treshold) return ++i;
            return arr.Length;
        }

        var sceneL = GetIndexOfHigherThen(left.Reverse().ToArray(), tree);
        var sceneU = GetIndexOfHigherThen(up.Reverse().ToArray(), tree);
        var sceneR = GetIndexOfHigherThen(right, tree);
        var sceneD = GetIndexOfHigherThen(down, tree);

        highestScenic = Math.Max(highestScenic, sceneL * sceneU * sceneR * sceneD);
    }
}

Console.WriteLine($"Part 1: {visibleTrees}");
Console.WriteLine($"Part 2: {highestScenic}");