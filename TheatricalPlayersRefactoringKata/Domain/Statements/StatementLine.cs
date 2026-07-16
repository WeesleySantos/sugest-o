namespace TheatricalPlayersRefactoringKata;

public class StatementLine
{
    public StatementLine(
        string playName,
        int amount,
        int credits,
        int seats)
    {
        PlayName = playName;
        Amount = amount;
        Credits = credits;
        Seats = seats;
    }

    public string PlayName { get; }

    /// <summary>
    /// Amount owed in cents.
    /// </summary>
    public int Amount { get; }

    public int Credits { get; }

    public int Seats { get; }
}
