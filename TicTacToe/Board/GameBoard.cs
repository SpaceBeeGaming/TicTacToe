using TicTacToe.Led;

namespace TicTacToe.Board;

public partial class GameBoard
{
    #region Private Fields
    private (int x, int y) _boardOffset;
    private readonly string _grid =
        """
        ┌───┬───┬───┐
        │   │   │   │
        ├───┼───┼───┤
        │   │   │   │
        ├───┼───┼───┤
        │   │   │   │
        └───┴───┴───┘
        """;
    private readonly List<Box> _boxes;
    private readonly List<Line> _lines;
    #endregion

    public GameBoard()
    {
        Box _box1 = new(2, 1);
        Box _box2 = new(6, 1);
        Box _box3 = new(10, 1);
        Box _box4 = new(2, 3);
        Box _box5 = new(6, 3);
        Box _box6 = new(10, 3);
        Box _box7 = new(2, 5);
        Box _box8 = new(6, 5);
        Box _box9 = new(10, 5);

        _boxes = new List<Box>()
        {
            _box1, _box2, _box3, _box4, _box5, _box6, _box7, _box8, _box9
        };

        _lines = new List<Line>()
        {
            new Line(_box1, _box2, _box3), // Row 1
            new Line(_box4, _box5, _box6), // Row 2
            new Line(_box7, _box8, _box9), // Row 3

            new Line(_box1, _box4, _box7), // Col 1
            new Line(_box2, _box5, _box8), // Col 2
            new Line(_box3, _box6, _box9), // Col 3

            new Line(_box1, _box5, _box9), // Diag -
            new Line(_box3, _box5, _box7), // Diag +
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
        System.Console.WriteLine(_grid);
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
    public (bool gameOver, Players winner) CheckForWinner()
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
            return (true, Players.Null);
        }

        // Returns when there are empty boxes and no winners.
        return (false, Players.Null);
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
        Boxes.B1 => _boxes[0], // Box 1
        Boxes.B2 => _boxes[1], // Box 2
        Boxes.B3 => _boxes[2], // Box 3
        Boxes.B4 => _boxes[3], // Box 4
        Boxes.B5 => _boxes[4], // Box 5
        Boxes.B6 => _boxes[5], // Box 6
        Boxes.B7 => _boxes[6], // Box 7
        Boxes.B8 => _boxes[7], // Box 8
        Boxes.B9 => _boxes[8], // Box 9
        _ => throw new ArgumentException("Cannot convert into a Box.", nameof(box)), // Error
    };

    public Boxes GetBox(Box box)
    {
        return box == _boxes[0] ? Boxes.B1 // Box 1
             : box == _boxes[1] ? Boxes.B2 // Box 2
             : box == _boxes[2] ? Boxes.B3 // Box 3
             : box == _boxes[3] ? Boxes.B4 // Box 4
             : box == _boxes[4] ? Boxes.B5 // Box 5
             : box == _boxes[5] ? Boxes.B6 // Box 6
             : box == _boxes[6] ? Boxes.B7 // Box 7
             : box == _boxes[7] ? Boxes.B8 // Box 8
             : box == _boxes[8] ? Boxes.B9 // Box 9
             : throw new ArgumentException("Unknown Box.", nameof(box)); // Error
    }

    /// <summary>
    /// Maps the boxes to <see cref="KeyNames"/> enum for use by the LED System.
    /// </summary>
    /// <param name="box">The instance of <see cref="Box"/> to get the key for.</param>
    /// <returns>The key associated with that box.</returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="box"/> was unexpected.</exception>
    public KeyNames GetKeyNameFromBox(Box box) => GetBox(box) switch
    {
        Boxes.B1 => KeyNames.NumPad7,
        Boxes.B2 => KeyNames.NumPad8,
        Boxes.B3 => KeyNames.NumPad9,
        Boxes.B4 => KeyNames.NumPad4,
        Boxes.B5 => KeyNames.NumPad5,
        Boxes.B6 => KeyNames.NumPad6,
        Boxes.B7 => KeyNames.NumPad1,
        Boxes.B8 => KeyNames.NumPad2,
        Boxes.B9 => KeyNames.NumPad3,
        _ => throw new ArgumentException("Unknown box.", nameof(box))
    };

    public IList<Line> GetLines(Players player, int numHits, bool shortCircuit = false)
    {
        List<Line> lines = new();

        // Iterate over all the lines in the grid.
        foreach (Line line in _lines)
        {
            // Skip if it is already full and we're not asking for complete rows.
            if (line.IsFull && numHits != line.Boxes.Count)
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
