namespace TicTacToe.Board;

public sealed class Line(Box box1, Box box2, Box box3)
{
    public List<Box> Boxes { get; init; } = [box1, box2, box3];

    public bool IsFull => Boxes.All(box => box.IsOccupied);

    public IEnumerable<Box> GetEmptyBoxes() => Boxes.Where(box => box.IsOccupied is false);
}
