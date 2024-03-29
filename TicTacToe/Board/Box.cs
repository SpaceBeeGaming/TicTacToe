﻿using System.Drawing;

namespace TicTacToe.Board;

public sealed class Box(int x, int y) : IEquatable<Box>
{
    public Point Location { get; } = new Point(x, y);

    public bool IsOccupied => Player is not Players.Null;

    public Players Player { get; set; }

    public bool Equals(Box? other) =>
        other is not null
        && Location.Equals(other.Location)
        && Player.Equals(other.Player);

    public override bool Equals(object? obj) => Equals(obj as Box);

    public override int GetHashCode() => HashCode.Combine(Location, Player);
}
