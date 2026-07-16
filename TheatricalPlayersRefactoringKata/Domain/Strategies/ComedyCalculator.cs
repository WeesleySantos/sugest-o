using System;

namespace TheatricalPlayersRefactoringKata;

public class ComedyCalculator : IPlayTypeCalculator
{
    public int CalculateAmount(int baseAmount, int audience)
    {
        if (audience > 20)
        {
            baseAmount += 10000 + 500 * (audience - 20);
        }

        return baseAmount + 300 * audience;
    }

    public int CalculateCredits(int audience)
    {
        var credits = Math.Max(audience - 30, 0);
        return credits + (int)Math.Floor((decimal)audience / 5);
    }
}
