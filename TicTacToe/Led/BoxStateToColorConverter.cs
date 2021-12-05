using System.Drawing;

using TicTacToe.Board;

namespace TicTacToe.Led;

public class BoxStateToColorConverter
{
    private static readonly Color _enemyColor = Color.Red;
    private static readonly Color _playerColor = Color.Blue;
    private static readonly Color _emptyColor = Color.Black;

    public static Color GetColorForBox(Box box) =>
        box.Symbol is 'X'
        ? _playerColor
        : box.Symbol is 'O'
        ? _enemyColor
        : _emptyColor;
}
