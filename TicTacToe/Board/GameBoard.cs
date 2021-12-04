namespace TicTacToe.Board;

public partial class GameBoard
{
    private (int x, int y) _boardOffset;
    private readonly string[] _grid = new string[]
    {
        "┌───┬───┬───┐",
        "│   │   │   │",
        "├───┼───┼───┤",
        "│   │   │   │",
        "├───┼───┼───┤",
        "│   │   │   │",
        "└───┴───┴───┘"
    };
    private readonly IEnumerable<Box> _boxes;
    private readonly IEnumerable<Line> _lines;

    #region Properties
    private readonly Box _box1 = new(2, 1);
    private readonly Box _box2 = new(6, 1);
    private readonly Box _box3 = new(10, 1);
    private readonly Box _box4 = new(2, 3);
    private readonly Box _box5 = new(6, 3);
    private readonly Box _box6 = new(10, 3);
    private readonly Box _box7 = new(2, 5);
    private readonly Box _box8 = new(6, 5);
    private readonly Box _box9 = new(10, 5);

    private readonly Row _row1;
    private readonly Row _row2;
    private readonly Row _row3;

    private readonly Column _col1;
    private readonly Column _col2;
    private readonly Column _col3;

    private readonly Diagonal _diag1;
    private readonly Diagonal _diag2;
    #endregion

    public GameBoard()
    {
        _boxes = new List<Box>()
        {
            _box1, _box2, _box3, _box4, _box5, _box6, _box7, _box8, _box9
        };

        _row1 = new Row(_box1, _box2, _box3);
        _row2 = new Row(_box4, _box5, _box6);
        _row3 = new Row(_box7, _box8, _box9);

        _col1 = new Column(_box1, _box4, _box7);
        _col2 = new Column(_box2, _box5, _box8);
        _col3 = new Column(_box3, _box6, _box9);

        _diag1 = new Diagonal(_box1, _box5, _box9);
        _diag2 = new Diagonal(_box3, _box5, _box7);

        _lines = new List<Line>()
        {
            _row1, _row2, _row3,
            _col1, _col2, _col3,
            _diag1, _diag2
        };
    }

    public IEnumerable<Box> GetEmptyBoxes() => _boxes.Where(box => box.IsOccupied is false);

    public void DrawBoard()
    {
        _boardOffset = System.Console.GetCursorPosition();
        for (var i = 0; i < _grid.Length; i++)
        {
            System.Console.WriteLine(_grid[i]);
        }
    }

    public bool DrawX(Box box) => DrawSymbol(box, 'X');

    public bool DrawX(Boxes box) => DrawX(GetBox(box));

    public bool DrawO(Box box) => DrawSymbol(box, 'O');

    private bool DrawSymbol(Box box, char symbol)
    {
        if (box.IsOccupied)
        {
            return false;
        }

        var oldPos = Console.SetCursorPosition(_boardOffset, box.Location);
        System.Console.Write(symbol);
        box.Symbol = symbol;

        Console.SetCursorPosition(oldPos);
        return true;
    }

    public (bool gameOver, char? winner) CheckForWinner()
    {
        // Iterate over all the rows, columns and diagonals.
        foreach (var line in _lines)
        {
            // Check if the line is all 'X'.
            if (line.Boxes.All(box => box.Symbol == 'X') is true)
            {
                return (true, 'X');
            }
            // Check if the line is all 'O'.
            else if (line.Boxes.All(box => box.Symbol == 'O') is true)
            {
                return (true, 'O');
            }
        }

        // Check if the grid is full.
        if (GetEmptyBoxes().Any() is false)
        {
            // We have a tie since the grid is full and we have no matches.
            return (true, null);
        }

        // Returns when there are empty boxes.
        return (false, null);
    }

    public IList<Line> GetDangerousLines() => GetLines('X');

    public Line? GetWinningLine() => GetLines('O', true).FirstOrDefault();
    
    public Box GetBox(Boxes box) => box switch
    {
        Boxes.B1 => _box1,
        Boxes.B2 => _box2,
        Boxes.B3 => _box3,
        Boxes.B4 => _box4,
        Boxes.B5 => _box5,
        Boxes.B6 => _box6,
        Boxes.B7 => _box7,
        Boxes.B8 => _box8,
        Boxes.B9 => _box9,
        _ => throw new InvalidOperationException("Cannot convert null into a Box."),
    };

    private IList<Line> GetLines(char symbol, bool shortCircuit = false)
    {
        List<Line> lines = new();
        foreach (var line in _lines)
        {
            if (line.IsFull)
            {
                continue;
            }

            if (line.Boxes.Count(box => box.Symbol == symbol) is 2)
            {
                lines.Add(line);
                if (shortCircuit)
                {
                    break;
                }
            }
        }

        return lines;
    }

}
