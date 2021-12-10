using System.Drawing;

namespace TicTacToe.Board;

public class Box : IEquatable<Box>
{
    public Point Location { get; }

    public bool IsOccupied { get; set; }

    private Players? _player;
    public Players? Player
    {
        get => _player;
        set
        {
            _player = value;
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
        && Player.Equals(other.Player)
        && IsOccupied.Equals(other.IsOccupied);

    public override bool Equals(object? obj) => Equals(obj as Box);

    public override int GetHashCode() => HashCode.Combine(Location, Player, IsOccupied);
}
