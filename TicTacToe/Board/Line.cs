namespace TicTacToe.Board;

public sealed class Line
{
    public List<Box> Boxes { get; init; }

    public Line(Box box1, Box box2, Box box3)
    {
        Boxes = new List<Box>()
        {
            box1, box2, box3
        };
    }

    public bool IsFull => Boxes.All(box => box.IsOccupied is true);

    public IEnumerable<Box> GetEmptyBoxes() => Boxes.Where(box => box.IsOccupied is false);
}
