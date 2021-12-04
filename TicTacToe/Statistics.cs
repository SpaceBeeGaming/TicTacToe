namespace TicTacToe;

public class Statistics
{
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Ties { get; set; }
    public int GamesPlayed => Wins + Losses + Ties;
    public double WinPercent => Wins * 1.0 / GamesPlayed * 100;
}
