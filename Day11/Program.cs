using System.Text.RegularExpressions;

var input = await File.ReadAllTextAsync("input.txt");

var monkeys1 = GetMonkeys(input);
for (var i = 0; i < 20; i++)
    foreach (var monkey in monkeys1.Values)
        monkey.EvalueateInventory(monkeys1, worryReduction: 3d);

var monkeys2 = GetMonkeys(input);
var divider = GetDivider(input);
for (var i = 0; i < 10000; i++)
    foreach (var monkey in monkeys2.Values)
        monkey.EvalueateInventory(monkeys2, divider: divider);

Console.WriteLine($"Part 1: {monkeys1.Values.Select(m => m.EvaluationCount).OrderByDescending(c => c).Take(2).Aggregate(1L, (a, b) => a * b)}");
Console.WriteLine($"Part 2: {monkeys2.Values.Select(m => m.EvaluationCount).OrderByDescending(c => c).Take(2).Aggregate(1L, (a, b) => a * b)}");


Dictionary<int, Monkey> GetMonkeys(string input) => new Regex(@"Monkey ([0-9]+):\r\n +Starting items: (?:([0-9]+),? ?)+\r\n +Operation: new = old ([+*]) ([0-9]+|old)\r\n +Test: divisible by ([0-9]+)\r\n +If true: throw to monkey ([0-9]+)\r\n +If false: throw to monkey ([0-9]+)")
                        .Matches(input)
                        .Select(ParseMonkey)
                        .ToDictionary(m => m.Id, m => m);

long GetDivider(string input) => new Regex(@"Monkey ([0-9]+):\r\n +Starting items: (?:([0-9]+),? ?)+\r\n +Operation: new = old ([+*]) ([0-9]+|old)\r\n +Test: divisible by ([0-9]+)\r\n +If true: throw to monkey ([0-9]+)\r\n +If false: throw to monkey ([0-9]+)")
    .Matches(input)
    .Select(m => long.Parse(m.Groups[5].Value))
    .Aggregate(1L, (a, b) => a * b);

Monkey ParseMonkey(Match match)
{
    var id = int.Parse(match.Groups[1].Value);
    var startingItems = match.Groups[2].Captures.Select(c => long.Parse(c.Value)).ToList();

    var operand = match.Groups[4].Value == "old" ? (int?)null : int.Parse(match.Groups[4].Value);
    Func<long, long> evaluate = (old) => match.Groups[3].Value == "*" ? old * (operand ?? old) : old + (operand ?? old);
    Func<long, bool> test = (value) => (value % int.Parse(match.Groups[5].Value)) == 0;

    var pass = int.Parse(match.Groups[6].Value);
    var fail = int.Parse(match.Groups[7].Value);
    return new Monkey(id, startingItems, evaluate, test, pass, fail);
}

class Monkey
{
    private List<long> _items;
    private Func<long, long> _evaluate;
    private Func<long, bool> _test;
    private int _passTarget;
    private int _failTarget;

    private long _evaluationCount = 0;

    public int Id { get; }
    public long EvaluationCount => _evaluationCount;

    public Monkey(int id, IEnumerable<long> items, Func<long, long> evaluate, Func<long, bool> test, int passTarget, int failTarget)
    {
        Id = id;

        _items = new List<long>(items);
        _evaluate = evaluate;
        _test = test;
        _passTarget = passTarget;
        _failTarget = failTarget;
    }

    public void EvalueateInventory(Dictionary<int, Monkey> monkeys, double? worryReduction = null, long? divider = null)
    {
        foreach (var item in _items)
        {
            _evaluationCount++;

            var newLevel = _evaluate(item);
            if (worryReduction is not null) newLevel = (int)Math.Floor(newLevel / worryReduction.Value);
            if (divider is not null) newLevel %= divider.Value;

            var recipient = monkeys[_test(newLevel) ? _passTarget : _failTarget];
            recipient.Catch(newLevel);
        }
        _items.Clear();
    }

    public void Catch(long item) => _items.Add(item);
}