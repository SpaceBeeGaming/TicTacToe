using System.Drawing;

namespace TicTacToe.Board;

public partial class GameBoard
{
    private static class Console
    {
        public static (int Left, int Top) SetCursorPosition((int Left, int Top) position) => SetCursorPosition(position, Point.Empty);

        public static (int Left, int Top) SetCursorPosition((int Left, int Top) position, Point offset)
        {
            var pos = System.Console.GetCursorPosition();
            System.Console.SetCursorPosition(position.Left + offset.X, position.Top + offset.Y);
            return pos;
        }
    }
}