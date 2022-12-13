await SolvePart1();
await SolvePart2();

async Task SolvePart1()
{
	var input = await File.ReadAllLinesAsync("input.txt");
	var pairs = new List<(Node, Node)>();
	for (var i = 0; i < input.Length; i += 3)
	{
		var first = ParseNode(input[i]);
		var second = ParseNode(input[i + 1]);
		pairs.Add((first, second));
	}

	var sum = 0;
	for (var i = 0; i < pairs.Count; i++)
		if (CompareNodes(pairs[i].Item1, pairs[i].Item2) == -1)
			sum += i + 1;
	Console.WriteLine($"Part 1: {sum}");
}

async Task SolvePart2()
{
	var input = await File.ReadAllLinesAsync("input.txt");
	var parsed = input.Where(l => !string.IsNullOrWhiteSpace(l)).Select(ParseNode).ToList();
	parsed.Add(CreateDividerNode(2));
	parsed.Add(CreateDividerNode(6));
	parsed.Sort(CompareNodes);

	var product = 1;
	for (var i = 0; i < parsed.Count; i++)
	{
		if (parsed[i] is ListNode { Nodes.Count: 1 } outer
		    && outer.Nodes[0] is ListNode { Nodes.Count: 1 } inner
		    && inner.Nodes[0] is NumberNode { Number: 2 or 6 })
			product *= (i+1);
	}

	Console.WriteLine($"Part 2: {product}");
}

Node CreateDividerNode(int number)
{
	var outer = new ListNode(null);
	var inner = new ListNode(outer);
	var final = new NumberNode(inner, number);
	inner.Add(final);
	outer.Add(inner);
	return outer;
}

Node ParseNode(string line)
{
	var cur = string.Empty;
	Node? curNode = null, rootNode = null;
	foreach (var c in line)
	{
		switch (c)
		{
			case '[' when rootNode is null:
				curNode = new ListNode(null);
				rootNode = curNode;
				break;
			case '[':
				var node = new ListNode(curNode);
				curNode.Add(node);
				curNode = node;
				break;
			case ']' when string.IsNullOrEmpty(cur):
				curNode = curNode?.Parent;
				break;
			case ']':
				curNode?.Add(int.Parse(cur));
				curNode = curNode?.Parent;
				cur = string.Empty;
				break;
			case ',' when string.IsNullOrEmpty(cur):
			case ' ':
				break;
			case ',':
				curNode?.Add(int.Parse(cur));
				cur = string.Empty;
				break;
			default:
				cur += c;
				break;
		}
	}
	return rootNode ?? throw new ArgumentNullException();
}

int CompareNodes(Node a, Node b)
{
	if (a is NumberNode c && b is NumberNode d)
	{
		if (c.Number == d.Number) return 0;
		return c.Number < d.Number ? -1 : 1;
	}
	if (a is NumberNode e && b is ListNode f) return CompareNodes(e.ToListNode(), f);
	if (a is ListNode g && b is NumberNode h) return CompareNodes(g, h.ToListNode());

	var firstList = a as ListNode ?? throw new ArgumentNullException();
	var secondList = b as ListNode ?? throw new ArgumentNullException();

	for (var i = 0; i < firstList.Nodes.Count; i++)
	{
		if (secondList.Nodes.Count <= i)
			return 1;

		var result = CompareNodes(firstList.Nodes[i], secondList.Nodes[i]);
		if (result != 0)
			return result;
	}
	if (secondList.Nodes.Count > firstList.Nodes.Count)
		return -1;
	return 0;
}

abstract class Node
{
	public Node? Parent { get; }

	public Node(Node? parent) => Parent = parent;

	public abstract void Add(Node node);

	public abstract void Add(int number);
}

class ListNode : Node
{
	public List<Node> Nodes { get; }

	public ListNode(Node? parent) : base(parent)
	{
		Nodes = new List<Node>();
	}

	public override void Add(Node node) => Nodes.Add(node);
	public override void Add(int number) => Nodes.Add(new NumberNode(this, number));

	public override string ToString() => $"[ {string.Join(", ", Nodes)} ]";
}

class NumberNode : Node
{
	public int Number { get; }

	public NumberNode(Node parent, int number) : base(parent)
	{
		Number = number;
	}

	public ListNode ToListNode()
	{
		var node = new ListNode(Parent);
		node.Add(this);
		return node;
	}

	public override void Add(Node node) => throw new NotImplementedException();
	public override void Add(int number) => throw new NotImplementedException();

	public override string ToString() => Number.ToString();
}