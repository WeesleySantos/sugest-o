using System.Collections.Generic;

namespace TheatricalPlayersRefactoringKata;

public class Statement
{
    public Statement(
        string customer,
        IReadOnlyList<StatementLine> lines,
        int totalAmount,
        int totalCredits)
    {
        Customer = customer;
        Lines = lines;
        TotalAmount = totalAmount;
        TotalCredits = totalCredits;
    }

    public string Customer { get; }

    public IReadOnlyList<StatementLine> Lines { get; }

    /// <summary>
    /// Total amount owed in cents.
    /// </summary>
    public int TotalAmount { get; }

    public int TotalCredits { get; }
}
