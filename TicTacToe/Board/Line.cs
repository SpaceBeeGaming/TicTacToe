namespace TicTacToe.Board;

public sealed class Line(Box box1, Box box2, Box box3)
{
    public List<Box> Boxes { get; init; } = [box1, box2, box3];

    public bool IsFull => Boxes.All(static box => box.IsOccupied);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0100:Remove redundant equality", Justification = "More readable")]
    public IEnumerable<Box> GetEmptyBoxes() => Boxes.Where(static box => box.IsOccupied is false);
}
