namespace TicTacToe.Board;

public abstract class Line
{
    public IEnumerable<Box> Boxes { get; set; }

    protected Line(Box box1, Box box2, Box box3)
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
