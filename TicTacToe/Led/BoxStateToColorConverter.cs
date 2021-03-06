using System.Drawing;

using TicTacToe.Board;

namespace TicTacToe.Led;

public static class BoxStateToColorConverter
{
    private static readonly Color _oColor = Color.Red;
    private static readonly Color _xColor = Color.Blue;
    private static readonly Color _emptyColor = Color.Black;

    public static Color GetColorForBox(Box box) =>
        box.Player switch
        {
            Players.X => _xColor,
            Players.O => _oColor,
            _ => _emptyColor,
        };
}