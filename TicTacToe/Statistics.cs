namespace TicTacToe;

public class Statistics
{
    private readonly List<int> _turnCounts = new();
    private int _xStarts;
    public int Wins { get; private set; }
    public int Losses { get; private set; }
    public int Ties { get; private set; }
    public int GamesPlayed => Wins + Losses + Ties;
    public double WinPercent => Wins * 1.0 / GamesPlayed * 100;
    public double LossPercent => Losses * 1.0 / GamesPlayed * 100;
    public double TiePercent => Ties * 1.0 / GamesPlayed * 100;
    public int AverageTurnCount => (int)Math.Round(_turnCounts.Average(), MidpointRounding.AwayFromZero);
    public double XStartPercent => _xStarts * 1.0 / GamesPlayed * 100;

    public void AddTurnCount(int turnCount) => _turnCounts.Add(turnCount);

    public void AddXStart() => _xStarts++;

    public void AddWin() => Wins++;

    public void AddLoss() => Losses++;

    public void AddTie() => Ties++;
}
