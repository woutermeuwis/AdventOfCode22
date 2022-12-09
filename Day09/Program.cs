using System.Drawing;

var input = (await File.ReadAllLinesAsync("input.txt")).Select(line => (dir: line[0], dist: int.Parse(line[2..])));
Dictionary<char, Size> moves = new() { { 'L', new Size(-1, 0) }, { 'R', new Size(1, 0) }, { 'U', new Size(0, -1) }, { 'D', new Size(0, 1) } };

Console.WriteLine($"Part 1: {SolveWithKnots(input, 2)}");
Console.WriteLine($"Part 2: {SolveWithKnots(input, 10)}");

int SolveWithKnots(IEnumerable<(char, int)> input, int knots)
{
    HashSet<Point> visitedPositions = new() { new Point(0, 0) };
    var rope = Enumerable.Range(0, knots).Select(i => new Point()).ToArray();

    foreach (var (dir, dist) in input)
    {
        for (var i = 0; i < dist; i++)
        {
            // move head
            rope[0] += moves[dir];
            for (var j = 1; j < rope.Length; j++)
            {
                var h = rope[j - 1];
                var t = rope[j];
                if (Math.Abs(h.X - t.X) > 1 || Math.Abs(h.Y - t.Y) > 1)
                    rope[j] += new Size(getStep(h.X, t.X), getStep(h.Y, t.Y));
            }
            visitedPositions.Add(rope[knots - 1]);
        }
    }
    return visitedPositions.Count;
}

int getStep(int h, int t) => h == t ? 0 : h > t ? 1 : -1;