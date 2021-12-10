using TicTacToe.Led;

namespace TicTacToe.Board;

public partial class GameBoard
{
    #region Private Fields
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

    private readonly Box _box1 = new(2, 1);
    private readonly Box _box2 = new(6, 1);
    private readonly Box _box3 = new(10, 1);
    private readonly Box _box4 = new(2, 3);
    private readonly Box _box5 = new(6, 3);
    private readonly Box _box6 = new(10, 3);
    private readonly Box _box7 = new(2, 5);
    private readonly Box _box8 = new(6, 5);
    private readonly Box _box9 = new(10, 5);

    private readonly Line _row1;
    private readonly Line _row2;
    private readonly Line _row3;

    private readonly Line _col1;
    private readonly Line _col2;
    private readonly Line _col3;

    private readonly Line _diag1;
    private readonly Line _diag2;
    #endregion

    public GameBoard()
    {
        _boxes = new List<Box>()
        {
            _box1, _box2, _box3, _box4, _box5, _box6, _box7, _box8, _box9
        };

        _row1 = new Line(_box1, _box2, _box3);
        _row2 = new Line(_box4, _box5, _box6);
        _row3 = new Line(_box7, _box8, _box9);

        _col1 = new Line(_box1, _box4, _box7);
        _col2 = new Line(_box2, _box5, _box8);
        _col3 = new Line(_box3, _box6, _box9);

        _diag1 = new Line(_box1, _box5, _box9);
        _diag2 = new Line(_box3, _box5, _box7);

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

    public bool DrawPlayer(Boxes box, Players player) => DrawPlayer(GetBox(box), player);

    public bool DrawPlayer(Box box, Players player)
    {
        if (box.IsOccupied)
        {
            return false;
        }

        box.Player = player;
        LedManager.SetBox(box, this);

        var oldPos = Console.SetCursorPosition(_boardOffset, box.Location);
        System.Console.Write(player);
        Console.SetCursorPosition(oldPos);
        return true;
    }

    public (bool gameOver, Players? winner) CheckForWinner()
    {
        // Iterate over all the rows, columns and diagonals.
        foreach (var line in _lines)
        {
            // Check if the line is all 'X'.
            if (line.Boxes.All(box => box.Player == Players.X) is true)
            {
                return (true, Players.X);
            }
            // Check if the line is all 'O'.
            else if (line.Boxes.All(box => box.Player == Players.O) is true)
            {
                return (true, Players.O);
            }
        }

        // Check if the grid is full.
        if (GetEmptyBoxes().Any() is false)
        {
            // We have a tie since the grid is full and we have no winners.
            return (true, null);
        }

        // Returns when there are empty boxes and no winners.
        return (false, null);
    }

    public IList<Line> GetDangerousLines(Players player) => GetLines(Game.GetOpposingPlayer(player), 2);

    public Line? GetWinningLine(Players player) => GetLines(player, 2, true).FirstOrDefault();

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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "This would make the result quite unreadable.")]
    public KeyNames GetKeyNameFromBox(Box box)
    {
        // Maps the boxes to their respective keys on the NumPad.
        if (box == _box1)
        {
            return KeyNames.NumPad7;
        }
        else if (box == _box2)
        {
            return KeyNames.NumPad8;
        }
        else if (box == _box3)
        {
            return KeyNames.NumPad9;
        }
        else if (box == _box4)
        {
            return KeyNames.NumPad4;
        }
        else if (box == _box5)
        {
            return KeyNames.NumPad5;
        }
        else if (box == _box6)
        {
            return KeyNames.NumPad6;
        }
        else if (box == _box7)
        {
            return KeyNames.NumPad1;
        }
        else if (box == _box8)
        {
            return KeyNames.NumPad2;
        }
        else if (box == _box9)
        {
            return KeyNames.NumPad3;
        }
        else
        {
            throw new InvalidOperationException("Unknown Box known.");
        }
    }

    private IList<Line> GetLines(Players player, int numHits, bool shortCircuit = false)
    {
        List<Line> lines = new();
        foreach (var line in _lines)
        {
            if (line.IsFull)
            {
                continue;
            }

            if (line.Boxes.Count(box => box.Player == player) == numHits)
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
