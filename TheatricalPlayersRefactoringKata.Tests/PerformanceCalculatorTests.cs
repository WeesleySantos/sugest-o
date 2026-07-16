using System;
using Xunit;

namespace TheatricalPlayersRefactoringKata.Tests;

public class PerformanceCalculatorTests
{
    private readonly PerformanceCalculator _calculator = new();

    [Fact]
    public void CalculateAmount_ClampsLinesBelowMinimumTo1000()
    {
        var play = new Play("Short", 500, "tragedy");
        var performance = new Performance("short", 30);

        // base = 1000 * 10 = 10000 cents; audience <= 30 → no extra
        Assert.Equal(10000, _calculator.CalculateAmount(play, performance));
    }

    [Fact]
    public void CalculateAmount_ClampsLinesAboveMaximumTo4000()
    {
        var play = new Play("Long", 5000, "tragedy");
        var performance = new Performance("long", 30);

        // base = 4000 * 10 = 40000 cents; audience <= 30 → no extra
        Assert.Equal(40000, _calculator.CalculateAmount(play, performance));
    }

    [Fact]
    public void CalculateAmount_Tragedy_AudienceAtMost30_IsBaseOnly()
    {
        var play = new Play("Hamlet", 2000, "tragedy");
        var performance = new Performance("hamlet", 30);

        Assert.Equal(20000, _calculator.CalculateAmount(play, performance));
    }

    [Fact]
    public void CalculateAmount_Tragedy_AudienceAbove30_AddsTenPerExtraSpectator()
    {
        var play = new Play("Hamlet", 2000, "tragedy");
        var performance = new Performance("hamlet", 35);

        // base 20000 + 1000 * (35 - 30) = 25000 cents
        Assert.Equal(25000, _calculator.CalculateAmount(play, performance));
    }

    [Fact]
    public void CalculateAmount_Comedy_AddsThreePerSpectator()
    {
        var play = new Play("Comedy", 2000, "comedy");
        var performance = new Performance("comedy", 20);

        // base 20000 + 300 * 20 = 26000 (audience not > 20, no large audience bonus)
        Assert.Equal(26000, _calculator.CalculateAmount(play, performance));
    }

    [Fact]
    public void CalculateAmount_Comedy_AudienceAbove20_AddsLargeAudienceBonus()
    {
        var play = new Play("Comedy", 2000, "comedy");
        var performance = new Performance("comedy", 25);

        // base 20000 + 10000 + 500*(25-20) + 300*25 = 20000 + 10000 + 2500 + 7500 = 40000
        Assert.Equal(40000, _calculator.CalculateAmount(play, performance));
    }

    [Fact]
    public void CalculateCredits_AudienceAtMost30_IsZero()
    {
        var play = new Play("Hamlet", 2000, "tragedy");
        var performance = new Performance("hamlet", 30);

        Assert.Equal(0, _calculator.CalculateCredits(play, performance));
    }

    [Fact]
    public void CalculateCredits_AudienceAbove30_OnePerExtraSpectator()
    {
        var play = new Play("Hamlet", 2000, "tragedy");
        var performance = new Performance("hamlet", 40);

        Assert.Equal(10, _calculator.CalculateCredits(play, performance));
    }

    [Fact]
    public void CalculateCredits_Comedy_AddsFloorAudienceDividedByFiveBonus()
    {
        var play = new Play("Comedy", 2000, "comedy");
        var performance = new Performance("comedy", 35);

        // max(35-30, 0) + floor(35/5) = 5 + 7 = 12
        Assert.Equal(12, _calculator.CalculateCredits(play, performance));
    }

    [Fact]
    public void CalculateAmount_UnknownType_Throws()
    {
        var play = new Play("Weird", 2000, "pastoral");
        var performance = new Performance("weird", 40);

        var ex = Assert.Throws<Exception>(() => _calculator.CalculateAmount(play, performance));
        Assert.Equal("unknown type: pastoral", ex.Message);
    }

    [Fact]
    public void LegacyExample_AmountsAndCreditsMatchStatementTotals()
    {
        var hamlet = new Play("Hamlet", 4024, "tragedy");
        var asLike = new Play("As You Like It", 2670, "comedy");
        var othello = new Play("Othello", 3560, "tragedy");

        var hamletPerf = new Performance("hamlet", 55);
        var asLikePerf = new Performance("as-like", 35);
        var othelloPerf = new Performance("othello", 40);

        Assert.Equal(65000, _calculator.CalculateAmount(hamlet, hamletPerf));
        Assert.Equal(25, _calculator.CalculateCredits(hamlet, hamletPerf));

        Assert.Equal(54700, _calculator.CalculateAmount(asLike, asLikePerf));
        Assert.Equal(12, _calculator.CalculateCredits(asLike, asLikePerf));

        Assert.Equal(45600, _calculator.CalculateAmount(othello, othelloPerf));
        Assert.Equal(10, _calculator.CalculateCredits(othello, othelloPerf));

        var totalAmount = 65000 + 54700 + 45600;
        var totalCredits = 25 + 12 + 10;
        Assert.Equal(165300, totalAmount);
        Assert.Equal(47, totalCredits);
    }
}
