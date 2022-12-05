// read input
var input = await File.ReadAllLinesAsync("input.txt");

// Solve
Console.WriteLine($"Part 1: {input.Select(DoesOnePairContainTheOther).Count(c => c)}");
Console.WriteLine($"Part 2: {input.Select(DoThePairsOverlap).Count(o => !o)}");

// Helpers
bool DoesOnePairContainTheOther(string input)
{
    var ((firstLower, firstUpper), (secondLower, secondUpper)) = ParsePair(input);
    return (firstLower <= secondLower && firstUpper >= secondUpper) || (firstLower >= secondLower && firstUpper <= secondUpper);
}

bool DoThePairsOverlap(string input)
{
    var ((firstLower, firstUpper), (secondLower, secondUpper)) = ParsePair(input);
    return firstLower > secondUpper || secondLower > firstUpper;
}

((int lower, int upper) first, (int lower, int upper) second) ParsePair(string input) => (first: ParseSection(input.Split(',')[0]), second: ParseSection(input.Split(',')[1]));
(int lower, int upper) ParseSection(string section) => (lower: int.Parse(section.Split('-')[0]), upper: int.Parse(section.Split('-')[1]));
