namespace Core;

public static class CollectionExtensions
{
	public static Queue<T> ToQueue<T>(this IEnumerable<T> input)
	{
		var queue = new Queue<T>();
		foreach (var element in input) queue.Enqueue(element);
		return queue;
	}
}