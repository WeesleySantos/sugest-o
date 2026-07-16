using System;

namespace TheatricalPlayersRefactoringKata;

/// <summary>
/// Calculates amount (in cents) and volume credits for a single performance.
/// Amounts stay in cents to preserve the existing arithmetic and statement output.
/// </summary>
public class PerformanceCalculator
{
    public int CalculateAmount(Play play, Performance performance)
    {
        var lines = play.Lines;
        if (lines < 1000) lines = 1000;
        if (lines > 4000) lines = 4000;

        var thisAmount = lines * 10;

        switch (play.Type)
        {
            case "tragedy":
                if (performance.Audience > 30)
                {
                    thisAmount += 1000 * (performance.Audience - 30);
                }
                break;
            case "comedy":
                if (performance.Audience > 20)
                {
                    thisAmount += 10000 + 500 * (performance.Audience - 20);
                }
                thisAmount += 300 * performance.Audience;
                break;
            default:
                throw new Exception("unknown type: " + play.Type);
        }

        return thisAmount;
    }

    public int CalculateCredits(Play play, Performance performance)
    {
        var credits = Math.Max(performance.Audience - 30, 0);

        if (play.Type == "comedy")
        {
            credits += (int)Math.Floor((decimal)performance.Audience / 5);
        }

        return credits;
    }
}
