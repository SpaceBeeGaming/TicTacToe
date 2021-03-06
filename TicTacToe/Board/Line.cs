namespace TicTacToe.Board;

public class Line
{
    public IEnumerable<Box> Boxes { get; set; }

    public Line(Box box1, Box box2, Box box3)
    {
        Boxes = new List<Box>()
        {
            box1, box2, box3
        };
    }

    public bool IsFull => Boxes.All(box => box.IsOccupied is true);

    public IEnumerable<Box> GetEmptyBoxes()
    {
        return Boxes.Where(box => box.IsOccupied is false);
    }
}
