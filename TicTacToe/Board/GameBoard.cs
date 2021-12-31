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

    /// <summary>
    /// Gets all of the empty boxes on the board.
    /// </summary>
    /// <returns>All of the empty boxes.</returns>
    public IEnumerable<Box> GetEmptyBoxes() => _boxes.Where(box => box.IsOccupied is false);

    /// <summary>
    /// Draws the game board.
    /// </summary>
    public void DrawBoard()
    {
        // Draw the game board and save the cursor offset.
        _boardOffset = System.Console.GetCursorPosition();
        for (var i = 0; i < _grid.Length; i++)
        {
            System.Console.WriteLine(_grid[i]);
        }
    }

    /// <summary>
    /// Wrapper for <see cref="DrawPlayer(Box, Players)"/> that doesn't require a reference to the actual <see cref="Box"/>.
    /// </summary>
    /// <param name="box">The instance of the <see cref="Boxes"/> to target.</param>
    /// <param name="player">The instance of <see cref="Players"/> to draw.</param>
    /// <returns></returns>
    public bool DrawPlayer(Boxes box, Players player) => DrawPlayer(GetBox(box), player);

    /// <summary>
    /// Draws the specific <paramref name="player"/> to the specific <paramref name="box"/>.
    /// </summary>
    /// <param name="box">The <see cref="Box"/> to target.</param>
    /// <param name="player">The instance of <see cref="Players"/> to draw.</param>
    /// <returns></returns>
    public bool DrawPlayer(Box box, Players player)
    {
        // Check if the box is empty.
        if (box.IsOccupied)
        {
            return false;
        }

        // Claim the box.
        box.Player = player;
        LedManager.SetBox(box, this);

        // Draw the symbol on the console.
        var oldPos = Console.SetCursorPosition(_boardOffset, box.Location);
        System.Console.Write(player);
        Console.SetCursorPosition(oldPos);

        return true;
    }

    /// <summary>
    /// Determines if the game has ended and if there is a winner.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Gets the list of lines where the opposing player to <paramref name="player"/> is one away from winning.
    /// </summary>
    /// <param name="player">
    /// <para>
    /// The instance of <see cref="Players"/> reference to.
    /// </para>
    /// <remark>
    /// If <see cref="Players.X"/> is used as input, this will return the lines where <see cref="Players.O"/> is about to win.
    /// </remark>
    /// </param>
    /// <returns>A list of dangerous <see cref="Line"/>s.</returns>
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

    /// <summary>
    /// Maps the boxes to <see cref="KeyNames"/> enum for use by the LED System.
    /// </summary>
    /// <param name="box">The instance of <see cref="Box"/> to get the key for.</param>
    /// <returns>The key associated with that box.</returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="box"/> was unexpected.</exception>
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
            throw new ArgumentException("Unknown Box.", nameof(box));
        }
    }

    public IList<Line> GetLines(Players player, int numHits, bool shortCircuit = false)
    {
        List<Line> lines = new();

        // Iterate over all the lines in the grid.
        foreach (var line in _lines)
        {
            // Skip if it is already full and we're not asking for complete rows.
            if (line.IsFull && numHits != line.Boxes.Count())
            {
                continue;
            }

            // Check if the specific line satisfies the condition for empty boxes.
            if (line.Boxes.Count(box => box.Player == player) == numHits)
            {
                lines.Add(line);

                // If so specified will exit after the first line is found.
                if (shortCircuit)
                {
                    break;
                }
            }
        }

        return lines;
    }
}
