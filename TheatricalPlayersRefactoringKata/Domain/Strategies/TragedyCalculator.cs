using System;

namespace TheatricalPlayersRefactoringKata;

public class TragedyCalculator : IPlayTypeCalculator
{
    public int CalculateAmount(int baseAmount, int audience)
    {
        if (audience > 30)
        {
            baseAmount += 1000 * (audience - 30);
        }

        return baseAmount;
    }

    public int CalculateCredits(int audience)
    {
        return Math.Max(audience - 30, 0);
    }
}
