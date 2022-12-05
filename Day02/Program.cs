// Read
var input = await File.ReadAllLinesAsync("input.txt");

//Solve
Console.WriteLine("Part 1: " + input.Select(i => GetInputScore(i[0]) + GetScore(i[0], i[2])).Sum());
Console.WriteLine("Part 2: " + input.Select(i => GetInputScore(GetPlayerInput(i[0], i[2])) + (3 * (i[2] - 'X'))).Sum());

// Helpers
int GetInputScore(char input) => input switch { 'X' => 1, 'Y' => 2, 'Z' => 3, _ => 0 };
int GetScore(char opponent, char player) => (((opponent - 'A') - (player - 'X') + 3) % 3) switch { 2 => 6, 0 => 3, 1 => 0, _ => 0 };
char GetPlayerInput(char opponent, char result) => (char)(result switch { 'X' => 'X' + ((opponent - 'A' + 2) % 3), 'Y' => 'X' + opponent - 'A', 'Z' => 'X' + ((opponent - 'A' + 1) % 3), _ => 0 });