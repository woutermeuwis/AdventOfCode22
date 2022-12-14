using System.Drawing;
using System.Text;

namespace Core;

public sealed class Grid<T>
{
    private T[][] _grid;

    public int Width { get; }
    public int Height { get; }
    public int XOffset { get; }
    public int YOffset { get; }

    public Grid(int width, int height, T initializer) : this(width, height, initializer, 0, 0)
    { }

    public Grid(int width, int height, T initializer, int xOffset, int yOffset)
    {
        Width = width;
        Height = height;
        XOffset = xOffset;
        YOffset = yOffset;
        _grid = Enumerable.Range(0, height).Select(i => Enumerable.Range(0, width).Select(i => initializer).ToArray()).ToArray();
    }

    public T Get(Point p) => Get(p.X, p.Y);
    public T Get(int x, int y) => _grid[y - YOffset][x - XOffset];

    public void Set(Point p, T value) => Set(p.X, p.Y, value);
    public void Set(int x, int y, T value) => _grid[y - YOffset][x - XOffset] = value;

    public void SetRow(int y, T value)
    {
        for (var i = XOffset; i < XOffset + Width; i++)
            Set(i, y, value);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        foreach (var line in _grid)
        {
            foreach (var element in line)
            {
                sb.Append(element?.ToString());
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
