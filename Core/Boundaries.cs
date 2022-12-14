using System.Drawing;

namespace Core;

public class Boundaries
{
    public Point Origin { get; }
    public Size Size { get; }
    public Point Bounds => Origin + Size;

    public Boundaries(Point origin, Size size)
    {
        Origin = origin;
        Size = size;
    }

    public bool Contains(Point p) => p.X >= Origin.X && p.Y >= Origin.Y && p.X < Bounds.X && p.Y < Bounds.Y;
}
