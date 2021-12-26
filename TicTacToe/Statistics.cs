namespace TicTacToe;

public class Statistics
{
    private readonly List<int> _turnCounts = new();
    private int _xStarts;
    public int Wins { get; private set; }
    public int Losses { get; private set; }
    public int Ties { get; private set; }
    public int GamesPlayed => Wins + Losses + Ties;
    public double WinPercent => Wins * 100.0 / GamesPlayed;
    public double LossPercent => Losses * 100.0 / GamesPlayed;
    public double TiePercent => Ties * 100.0 / GamesPlayed;
    public int AverageTurnCount => (int)Math.Round(_turnCounts.Average(), MidpointRounding.AwayFromZero);
    public double XStartPercent => _xStarts * 100.0 / GamesPlayed;

    public void AddTurnCount(int turnCount) => _turnCounts.Add(turnCount);

    public void AddXStart() => _xStarts++;

    public void AddWin() => Wins++;

    public void AddLoss() => Losses++;

    public void AddTie() => Ties++;
}
