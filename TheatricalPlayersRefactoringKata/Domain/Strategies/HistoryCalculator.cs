using System;

namespace TheatricalPlayersRefactoringKata;

public class HistoryCalculator : IPlayTypeCalculator
{
    private readonly TragedyCalculator _tragedyCalculator = new();
    private readonly ComedyCalculator _comedyCalculator = new();

    public int CalculateAmount(int baseAmount, int audience)
    {
        return _tragedyCalculator.CalculateAmount(baseAmount, audience)
            + _comedyCalculator.CalculateAmount(baseAmount, audience);
    }

    public int CalculateCredits(int audience)
    {
        return Math.Max(audience - 30, 0);
    }
}
