namespace TheatricalPlayersRefactoringKata;

/// <summary>
/// Calculates amount (in cents) and volume credits for a single performance.
/// Amounts stay in cents to preserve the existing arithmetic and statement output.
/// </summary>
public class PerformanceCalculator
{
    private readonly PlayTypeCalculatorFactory _calculatorFactory;

    public PerformanceCalculator()
        : this(new PlayTypeCalculatorFactory())
    {
    }

    public PerformanceCalculator(PlayTypeCalculatorFactory calculatorFactory)
    {
        _calculatorFactory = calculatorFactory;
    }

    public int CalculateAmount(Play play, Performance performance)
    {
        var lines = play.Lines;
        if (lines < 1000) lines = 1000;
        if (lines > 4000) lines = 4000;

        var baseAmount = lines * 10;
        var calculator = _calculatorFactory.Get(play.Type);
        return calculator.CalculateAmount(baseAmount, performance.Audience);
    }

    public int CalculateCredits(Play play, Performance performance)
    {
        var calculator = _calculatorFactory.Get(play.Type);
        return calculator.CalculateCredits(performance.Audience);
    }
}
