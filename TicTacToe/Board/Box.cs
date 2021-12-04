using System.Drawing;

namespace TicTacToe.Board;

public class Box : IEquatable<Box>
{
    public Point Location { get; }

    public bool IsOccupied { get; set; }

    private char? _symbol;

    public char? Symbol
    {
        get => _symbol;
        set
        {
            _symbol = value is 'X' or 'O' ? value : throw new InvalidOperationException("Value must be either 'X' or 'O'.");
            IsOccupied = true;
        }
    }

    public Box(int x, int y)
    {
        Location = new Point(x, y);
    }

    public bool Equals(Box? other) =>
        other is not null
        && Location.Equals(other.Location)
        && Symbol.Equals(other.Symbol)
        && IsOccupied.Equals(other.IsOccupied);

    public override bool Equals(object? obj) => Equals(obj as Box);

    public override int GetHashCode() => HashCode.Combine(Location, Symbol, IsOccupied);
}
