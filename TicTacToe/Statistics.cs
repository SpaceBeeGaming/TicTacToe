namespace TicTacToe;

public class Statistics
{
    private readonly List<int> _turnCounts = new();
    private readonly List<long> _gameDurationsMS = new();
    private int _xStarts;
    public int Wins { get; private set; }
    public int Losses { get; private set; }
    public int Ties { get; private set; }
    public int GamesPlayed => Wins + Losses + Ties;
    public double WinPercent => Wins * 100d / GamesPlayed;
    public double LossPercent => Losses * 100d / GamesPlayed;
    public double TiePercent => Ties * 100d / GamesPlayed;
    public double XStartPercent => _xStarts * 100d / GamesPlayed;
    public int AverageTurnCount => (int)Math.Round(_turnCounts.Average(), MidpointRounding.AwayFromZero);
    public TimeSpan AverageGameDuration => TimeSpan.FromMilliseconds(_gameDurationsMS.Average());
    public TimeSpan TotalGameDuration => TimeSpan.FromMilliseconds(_gameDurationsMS.Sum());

    public void AddTurnCount(int turnCount) => _turnCounts.Add(turnCount);
    public void AddGameDuration(long gameDurationMS) => _gameDurationsMS.Add(gameDurationMS);

    public void AddXStart() => _xStarts++;

    public void AddWin() => Wins++;

    public void AddLoss() => Losses++;

    public void AddTie() => Ties++;
}
