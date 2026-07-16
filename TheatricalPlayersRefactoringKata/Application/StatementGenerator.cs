using System.Collections.Generic;

namespace TheatricalPlayersRefactoringKata;

public class StatementGenerator
{
    private readonly PerformanceCalculator _calculator;

    public StatementGenerator()
        : this(new PerformanceCalculator())
    {
    }

    public StatementGenerator(PerformanceCalculator calculator)
    {
        _calculator = calculator;
    }

    public Statement Generate(
        Invoice invoice,
        IReadOnlyDictionary<string, Play> plays)
    {
        var lines = new List<StatementLine>();
        var totalAmount = 0;
        var totalCredits = 0;

        foreach (var performance in invoice.Performances)
        {
            var play = plays[performance.PlayId];
            var amount = _calculator.CalculateAmount(play, performance);
            var credits = _calculator.CalculateCredits(play, performance);

            lines.Add(new StatementLine(
                play.Name,
                amount,
                credits,
                performance.Audience));

            totalAmount += amount;
            totalCredits += credits;
        }

        return new Statement(
            invoice.Customer,
            lines,
            totalAmount,
            totalCredits);
    }
}
