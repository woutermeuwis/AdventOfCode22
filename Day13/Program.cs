using Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

var input = await File.ReadAllLinesAsync("input.txt");
SolvePart1(input);
SolvePart2(input);

void SolvePart1(IEnumerable<string> file)
{
	var q = file.Where(l => l.Any()).Select(JsonConvert.DeserializeObject).ToQueue();
	var count = q.Count / 2;
	var sum = 0;
	for (var i = 1; i <= count; i++)
		if (CompareNodes(q.Dequeue(), q.Dequeue()) == -1)
			sum += i;
	Console.WriteLine($"Part 1: {sum}");
}

void SolvePart2(IEnumerable<string> file)
{
	var dividers = new[]
	{
		JsonConvert.DeserializeObject("[[2]]"),
		JsonConvert.DeserializeObject("[[6]]")
	};
	var list = file.Where(l => l.Any()).Select(JsonConvert.DeserializeObject).ToList();
	list.AddRange(dividers);
	list.Sort(CompareNodes);
	Console.WriteLine($"Part 2: {(list.IndexOf(dividers[0]) + 1) * (list.IndexOf(dividers[1]) + 1)}");
}

int CompareNodes(object? a, object? b)
{
	if (a is JArray && b is JArray) return CompareArrays((JArray)a, (JArray)b);
	if (a is JValue && b is JArray) return CompareArrays(new JArray { a }, (JArray)b);
	if (a is JArray && b is JValue) return CompareArrays((JArray)a, new JArray { b });
	if (a is JValue { Value: long intA } && b is JValue { Value: long intB }) return CompareValues(intA, intB);
	return 0;
}

int CompareValues(long a, long b) => a == b ? 0 : a < b ? -1 : 1;

int CompareArrays(JArray a, JArray b)
{
	for (var i = 0; i < a.Count; i++)
	{
		if (b.Count <= i) return 1;
		var result = CompareNodes(a[i], b[i]);
		if (result != 0) return result;
	}
	return b.Count > a.Count ? -1 : 0;
}