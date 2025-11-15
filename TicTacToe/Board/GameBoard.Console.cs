using System.Drawing;

namespace TicTacToe.Board;

public partial class GameBoard
{
    private static class Console
    {
        /// <summary>
        /// Sets the cursor position on the console.
        /// </summary>
        /// <param name="position">The position to set the cursor to.</param>
        /// <returns>The previous cursor position.</returns>
        public static (int Left, int Top) SetCursorPosition((int Left, int Top) position) => SetCursorPosition(position, Point.Empty);

        /// <summary>
        /// Sets the cursor position on the console with an offset.
        /// </summary>
        /// <param name="position">The position to set the cursor to.</param>
        /// <param name="offset">The offset to apply to the position.</param>
        /// <returns>The previous cursor position.</returns>
        public static (int Left, int Top) SetCursorPosition((int Left, int Top) position, Point offset)
        {
            (int Left, int Top) pos = System.Console.GetCursorPosition();
            System.Console.SetCursorPosition(position.Left + offset.X, position.Top + offset.Y);
            return pos;
        }
    }
}
