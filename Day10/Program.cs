using System.Text;

var input = await File.ReadAllLinesAsync("input.txt");
int X = 1;
var Register = new List<int> { 0 };

foreach (var line in input)
{
    if (line == "noop")
    {
        Register.Add(X);
    }
    else
    {
        Register.Add(X);
        Register.Add(X);
        X += int.Parse(line.Split(' ')[1]);
    }
}

var part1 = 0;
for (var i = 20; i < Register.Count; i += 40)
    part1 += Register[i] * i;

var CRT = new StringBuilder();
for (var i = 1; i <= 240; i++)
{
    var pos = Register[i];
    CRT.Append(Math.Abs(pos - ((i % 40) - 1)) <= 1 ? '#' : '.');
    if (i % 40 == 0)
        CRT.AppendLine();
}

Console.WriteLine($"Part 1: {part1}");
Console.Write(CRT.ToString());
