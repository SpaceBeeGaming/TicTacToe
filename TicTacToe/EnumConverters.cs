namespace TicTacToe;

public static class EnumConverters
{
    /// <summary>
    /// Converts a <see cref="GameOverType"/> into <see cref="Players"/>.
    /// </summary>
    /// <param name="gameOverType">The <see cref="GameOverType"/> to convert</param>
    /// <returns>A <see cref="Players"/> or <see langword="null"/> if conversion couldn't be performed.</returns>
    public static Players GameOverTypeToPlayersConverter(GameOverType gameOverType)
    {
        return gameOverType switch
        {
            GameOverType.X => Players.X,
            GameOverType.O => Players.O,
            GameOverType.Tie => Players.Null,
            _ => throw new ArgumentException($"{gameOverType} is not valid.", nameof(gameOverType))
        };
    }

    /// <summary>
    /// Converts a <see cref="Players"/> into <see cref="GameOverType"/>.
    /// </summary>
    /// <param name="player">The <see cref="Players"/> to convert.</param>
    /// <returns>A <see cref="GameOverType"/> or <see langword="null"/> if conversion couldn't be performed.</returns>
    public static GameOverType PlayersToGameOverTypeConverter(Players player)
    {
        return player switch
        {
            Players.X => GameOverType.X,
            Players.O => GameOverType.O,
            Players.Null => GameOverType.Tie,
            _ => throw new ArgumentException($"{player} is not valid.", nameof(player))
        };
    }
}
