namespace TicTacToe;

public class Statistics
{
    private readonly List<int> _turnCounts = new();
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Ties { get; set; }
    public int GamesPlayed => Wins + Losses + Ties;
    public double WinPercent => Wins * 1.0 / GamesPlayed * 100;
    public double LossPercent => Losses * 1.0 / GamesPlayed * 100;
    public double TiePercent => Ties * 1.0 / GamesPlayed * 100;
    public int AverageTurnCount => (int)Math.Round(_turnCounts.Average(), MidpointRounding.AwayFromZero);

    public void AddTurnCount(int turnCount)
    {
        _turnCounts.Add(turnCount);
    }
}
