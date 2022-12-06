// read input
var input = (await File.ReadAllTextAsync("input.txt")).ToCharArray();

Console.WriteLine($"Part 1: {GetMarkerPosition(input, 4)}");
Console.WriteLine($"Part 1: {GetMarkerPosition(input, 14)}");

int GetMarkerPosition(char[] arr, int length) => Enumerable.Range(0, arr.Length - length).FirstOrDefault(i => arr.Skip(i).Take(length).Distinct().Count() == length) + length;