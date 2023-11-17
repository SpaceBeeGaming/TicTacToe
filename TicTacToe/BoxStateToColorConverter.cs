using System.Drawing;

using TicTacToe.Board;

namespace TicTacToe;

public static class BoxStateToColorConverter
{
    private static readonly Color oColor = Color.Red;
    private static readonly Color xColor = Color.Blue;
    private static readonly Color emptyColor = Color.Black;
    public static Color GetColorForBox(this Box box) =>
    box.Player switch
    {
        Players.X => xColor,
        Players.O => oColor,
        Players.Null => emptyColor,
        _ => throw new InvalidOperationException($"{box.Player} is not valid.")
    };
}