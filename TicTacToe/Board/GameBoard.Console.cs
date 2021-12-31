using System.Drawing;

namespace TicTacToe.Board;

public partial class GameBoard
{
    private static class Console
    {
        public static (int Left, int Top) SetCursorPosition((int Left, int Top) position) => SetCursorPosition(position, Point.Empty);

        /// <summary>
        /// Sets the cursor position specified in <paramref name="position"/> tuple with an <paramref name="offset"/>.
        /// </summary>
        /// <param name="position">The (Left,Top) tuple to set the cursor to.</param>
        /// <param name="offset">Offset to add to the position.</param>
        /// <returns>The old position.</returns>
        public static (int Left, int Top) SetCursorPosition((int Left, int Top) position, Point offset)
        {
            var pos = System.Console.GetCursorPosition();
            System.Console.SetCursorPosition(position.Left + offset.X, position.Top + offset.Y);
            return pos;
        }
    }
}